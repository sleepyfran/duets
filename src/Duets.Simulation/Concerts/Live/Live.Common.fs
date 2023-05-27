[<AutoOpen>]
module Duets.Simulation.Concerts.Live.Common

open Aether
open Duets.Common
open Duets.Entities
open Duets.Simulation

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
    let addEffects effects response =
        { response with
            Effects = response.Effects @ effects }

    /// Adds one effect to the response.
    let addEffect effect = addEffects [ effect ]

    /// Adds an event to the response.
    let addEvent event response =
        { response with
            OngoingConcert = addEvent event response.OngoingConcert }

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

/// Defines all the different rules that can be used to calculate the
/// point increase for an action during a concert.
type internal ScoreRule =
    /// Increases the points the higher the average skill of the band is.
    | BandSkills of skills: SkillId
    /// Decreases the points the higher the drunkenness amount of the character.
    | CharacterDrunkenness
    /// Increases the points the higher the skill of the character is.
    | CharacterSkill of skill: SkillId
    /// Increases the points the higher the quality of the song.
    | SongQuality of song: Finished<Song>
    /// Increases the points the higher the practice of the song.
    | SongPractice of song: Finished<Song>

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
      ScoreRules: ScoreRule list
      Multipliers: Multiplier list }

/// Adds extra details as of why a certain result was obtained.
type ConcertEventResultReason =
    | CharacterDrunk
    | LowPractice
    | LowSkill
    | LowQuality

/// Defines the result of an event in the concert.
type ConcertEventResult =
    /// Indicates an action with done with no rating required.
    | Done
    /// A performance that got less than 25% of the maximum points.
    | LowPerformance of ConcertEventResultReason list
    /// A performance that got between 25% and 50% of the maximum points.
    | AveragePerformance of ConcertEventResultReason list
    /// A performance that got between 50% and 75% of the maximum points.
    | GoodPerformance of ConcertEventResultReason list
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
        characterSkillLevel state currentMember.CharacterId skillId |> float)

let private playableCharacterSkillLevel state skillId =
    let character = Queries.Characters.playableCharacter state

    characterSkillLevel state character.Id skillId

let private averageableAffectingQuality state affectingQuality =
    match affectingQuality with
    | BandSkills skillId -> [ bandAverageSkillLevel state skillId ]
    | CharacterSkill skillId -> [ playableCharacterSkillLevel state skillId ]
    | SongQuality(Finished(_, quality)) -> [ quality / 1<quality> |> float ]
    | SongPractice(Finished(song, _)) ->
        [ song.Practice / 1<practice> |> float ]
    | _ -> []

/// Computes the qualities (values between 0 and 100) that after being averaged
/// give the base score of the action with the reasons for that score.
let private baseScoreWithReasons state action =
    let reasons, qualities =
        action.ScoreRules
        |> List.fold
            (fun (reasons, scores) aq ->
                match aq with
                | BandSkills skillId ->
                    let avgSkill = bandAverageSkillLevel state skillId

                    if avgSkill < 50 then
                        (reasons @ [ LowSkill ], scores @ [ avgSkill ])
                    else
                        (reasons, scores @ [ avgSkill ])
                | CharacterSkill skillId ->
                    let avgSkill = playableCharacterSkillLevel state skillId

                    if avgSkill < 50 then
                        (reasons @ [ LowSkill ], scores @ [ avgSkill ])
                    else
                        (reasons, scores @ [ avgSkill ])
                | SongQuality(Finished(_, quality)) ->
                    let q = quality / 1<quality> |> float

                    if q < 50 then
                        (reasons @ [ LowQuality ], scores @ [ q ])
                    else
                        (reasons, scores @ [ q ])
                | SongPractice(Finished(song, _)) ->
                    let p = song.Practice / 1<practice> |> float

                    if p < 50 then
                        (reasons @ [ LowPractice ], scores @ [ p ])
                    else
                        (reasons, scores @ [ p ])
                | _ -> (reasons, scores))
            ([], [])

    if List.isEmpty qualities then
        [], 100.0
    else
        let avgQualities = qualities |> List.average
        reasons, avgQualities

/// Computes the average modifier to apply based on the score rules that are
/// modifiers (value between 0.0 and 1.0) rather than a quality (value between
/// 0 and 100).
let private modifiersWithReasons state action =
    let reasons, modifiers =
        action.ScoreRules
        |> List.fold
            (fun (reasons, modifiers) aq ->
                match aq with
                | CharacterDrunkenness ->
                    let characterDrunkenness =
                        Queries.Characters.playableCharacterAttribute
                            state
                            CharacterAttribute.Drunkenness

                    let res amount =
                        (reasons @ [ CharacterDrunk ], modifiers @ [ amount ])

                    match characterDrunkenness with
                    | amount when amount < 5 -> (reasons, modifiers)
                    | amount when amount < 25 -> res 0.75
                    | amount when amount < 50 -> res 0.5
                    | amount when amount < 70 -> res 0.25
                    | _ -> res 0.05
                | _ -> (reasons, modifiers))
            ([], [])

    if List.isEmpty modifiers then
        [], 1.0
    else
        let avgModifiers = modifiers |> List.average

        reasons, avgModifiers

let private multipliersOf action =
    List.sumBy (fun multiplier -> float multiplier / 100.0) action.Multipliers

let rec internal performAction state ongoingConcert action =
    let timesPerformedEvent =
        Concert.Ongoing.timesDoneEvent ongoingConcert action.Event

    match action.Limit with
    | NoLimit -> performAction' state ongoingConcert action
    | Penalized(limit, penalization) ->
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
    if List.isEmpty action.ScoreRules then
        // An action with no affecting qualities does not really require any
        // calculations. Just use the multiplier section to sum the points.
        let points = List.sum action.Multipliers

        Response.forEvent' ongoingConcert action.Event points Done
    else
        ratePerformance state ongoingConcert action
    |> Response.addEffects action.Effects

and private ratePerformance state ongoingConcert action =
    let qualityReasons, baseScore = baseScoreWithReasons state action

    let multipliers =
        if List.isEmpty action.Multipliers then
            1.0
        else
            multipliersOf action

    let modifierReasons, modifier = modifiersWithReasons state action

    let pointIncrease = baseScore * modifier * multipliers

    let projectedMaximum = 100.0 * multipliers

    let performanceStep = projectedMaximum / 4.0

    let resultReasons = qualityReasons @ modifierReasons

    let result =
        match pointIncrease with
        | points when points < performanceStep -> LowPerformance resultReasons
        | points when points < performanceStep * 2.0 ->
            AveragePerformance resultReasons
        | points when points < performanceStep * 3.0 ->
            GoodPerformance resultReasons
        | _ -> GreatPerformance

    Response.forEvent'
        ongoingConcert
        action.Event
        (Math.ceilToNearest pointIncrease)
        result
