[<AutoOpen>]
module Simulation.Concerts.Live.Common

open Aether
open Common
open Entities
open Simulation

type OngoingConcertEventResponse<'r> =
    { Effects: Effect list
      OngoingConcert: OngoingConcert
      Points: int
      Result: 'r }

let private addPoints ongoingConcert points =
    Optic.map
        Lenses.Concerts.Ongoing.points_
        (fun currentPoints ->
            currentPoints + (points * 1<quality>)
            |> Math.clamp 0<quality> 100<quality>)
        ongoingConcert

let private addEvent event =
    Optic.map Lenses.Concerts.Ongoing.events_ (List.append [ event ])

module internal Response =
    /// Returns an empty response that contains only the given ongoing concert
    /// with no modifications.
    let empty ongoingConcert =
        { Effects = []
          OngoingConcert = ongoingConcert
          Points = 0
          Result = () }

    /// Returns an empty response that contains only the given ongoing concert
    /// with no modifications and a result.
    let empty' ongoingConcert result =
        { Effects = []
          OngoingConcert = ongoingConcert
          Points = 0
          Result = result }

    /// Adds the given points to the given ongoing concert making sure that the
    /// value does not go above 100 or below 0, adds the given event as well and
    /// then creates a response with all the given parameters and the updated state.
    let forEvent ongoingConcert event points =
        addPoints ongoingConcert points
        |> addEvent event
        |> fun updatedOngoingConcert ->
            { Effects = []
              OngoingConcert = updatedOngoingConcert
              Points = points
              Result = () }

    /// Adds the given points to the given ongoing concert making sure that the
    /// value does not go above 100 or below 0, adds the given event as well and
    /// then creates a response with all the given parameters and the updated state.
    let forEvent' ongoingConcert event points result =
        addPoints ongoingConcert points
        |> addEvent event
        |> fun updatedOngoingConcert ->
            { Effects = []
              OngoingConcert = updatedOngoingConcert
              Points = points
              Result = result }

    /// Adds the given set of effects to the response.
    let addEffects effects response = { response with Effects = effects }

    /// Adds an event to the response.
    let addEvent event response =
        { response with OngoingConcert = addEvent event response.OngoingConcert }

    /// Adds the given points to the point counter of the result and to the
    /// ongoing concert.
    let addPoints points response =
        { Effects = response.Effects
          OngoingConcert = addPoints response.OngoingConcert points
          Points = response.Points + points
          Result = response.Result }

    /// Maps the result of the response applying the current value to the given
    /// function and setting the result to the return value of the function.
    let mapResult mapFn response =
        { Effects = response.Effects
          OngoingConcert = response.OngoingConcert
          Points = response.Points
          Result = mapFn response.Result }

/// Defines a penalization for an action which affects different things in an
/// ongoing concert. The number should be *negative*, since the points are
/// summed to the current count as is.
type internal Penalization = PointPenalization of points: int

/// Defines a limit in the amount of times that an action can be performed during
/// a concert. If NoLimit then the action can be performed unlimited times, when
/// the limit is penalized then a penalization is applied when surpassing the
/// limit and no action disallows the execution of the action if it'd be over
/// the limit.
type internal Limit =
    | NoLimit
    | Penalized of limit: int<times> * penalization: Penalization
    | NoAction of limit: int<times>

/// Defines all the different qualities that can be used to calculate the
/// point increase for an action during a concert.
type internal AffectingQuality =
    | BandSkills of skills: SkillId
    | CharacterSkill of skill: SkillId
    | SongQuality of song: FinishedSongWithQuality
    | SongPractice of song: FinishedSong

/// A multiplier that will be applied after calculating the final points from
/// the affecting qualities.
type internal Multiplier = int

/// Defines an event to be performed in the concert. This is the intermediate
/// data type to be passed into the runAction function which takes care of
/// all the nitty-gritty of calculating the outcomes of events based on the
/// given rules.
type internal ConcertAction =
    { Event: ConcertEvent
      Limit: Limit
      Effects: Effect list
      AffectingQualities: AffectingQuality list
      Multipliers: Multiplier list }

/// Defines the result of an event in the concert.
type ConcertEventResult =
    /// Indicates an action with done with no rating required.
    | Done
    /// A performance that got less than 25% of the maximum points.
    | LowPerformance
    /// A performance that got between 25% and 50% of the maximum points.
    | AveragePerformance
    /// A performance that got between 50% and 75% of the maximum points.
    | GoodPerformance
    /// A performance that got between 75% and 100% of the maximum points.
    | GreatPerformance
    /// Performance was not done because it was repeated too many times.
    | TooManyRepetitionsNotDone
    /// Performance was done but penalized because it was repeated too many times.
    | TooManyRepetitionsPenalized

let private applyPenalization ongoingConcert action penalization =
    match penalization with
    | PointPenalization points ->
        Response.forEvent'
            ongoingConcert
            action.Event
            points
            TooManyRepetitionsPenalized

let private characterSkillLevel state characterId skillId =
    Queries.Skills.characterSkillWithLevel state characterId skillId
    |> snd
    |> float

let private bandAverageSkillLevel state skillId =
    let band = Queries.Bands.currentBand state

    band.Members
    |> List.averageBy (fun currentMember ->
        characterSkillLevel state currentMember.CharacterId skillId
        |> float)

let private playableCharacterSkillLevel state skillId =
    let character = Queries.Characters.playableCharacter state

    characterSkillLevel state character.Id skillId

let private averageAffectingQualities state action =
    action.AffectingQualities
    |> List.averageBy (fun affectingQuality ->
        match affectingQuality with
        | BandSkills skillId -> bandAverageSkillLevel state skillId
        | CharacterSkill skillId -> playableCharacterSkillLevel state skillId
        | SongQuality (_, quality) -> quality / 1<quality> |> float
        | SongPractice (FinishedSong song) ->
            song.Practice / 1<practice> |> float)

let private multipliersOf action =
    List.sumBy (fun multiplier -> float multiplier / 100.0) action.Multipliers

let rec internal performAction state ongoingConcert action =
    let timesPerformedEvent =
        Concert.Ongoing.timesDoneEvent ongoingConcert action.Event

    match action.Limit with
    | NoLimit -> performAction' state ongoingConcert action
    | Penalized (limit, penalization) ->
        if timesPerformedEvent < limit then
            performAction' state ongoingConcert action
        else
            applyPenalization ongoingConcert action penalization
            |> Response.addEffects action.Effects
    | NoAction limit ->
        if timesPerformedEvent < limit then
            performAction' state ongoingConcert action
        else
            Response.empty' ongoingConcert TooManyRepetitionsNotDone
    |> fun response ->
        (*
        Update the current concert in the situation, which is where we store
        the transient states, such as in concert, while we are still performing
        them.
        *)
        Response.addEffects
            [ Situations.inConcert response.OngoingConcert ]
            response

and private performAction' state ongoingConcert action =
    if List.isEmpty action.AffectingQualities then
        // An action with no affecting qualities does not really require any
        // calculations. Just use the multiplier section to sum the points.
        let points = List.sum action.Multipliers

        Response.forEvent' ongoingConcert action.Event points Done
    else
        ratePerformance state ongoingConcert action
    |> Response.addEffects action.Effects

and private ratePerformance state ongoingConcert action =
    let averageQualities = averageAffectingQualities state action

    let multipliers =
        if List.isEmpty action.Multipliers then
            1.0
        else
            multipliersOf action

    let pointIncrease = averageQualities * multipliers

    let projectedMaximum = 100.0 * multipliers

    let performanceStep = projectedMaximum / 4.0

    let result =
        match pointIncrease with
        | points when points < performanceStep -> LowPerformance
        | points when points < performanceStep * 2.0 -> AveragePerformance
        | points when points < performanceStep * 3.0 -> GoodPerformance
        | _ -> GreatPerformance

    Response.forEvent'
        ongoingConcert
        action.Event
        (Math.ceilToNearest pointIncrease)
        result
