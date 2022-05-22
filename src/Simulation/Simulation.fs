[<RequireQualifiedAccess>]
module Simulation.Simulation


open Entities
open Simulation.Market
open Simulation.Skills.ImproveSkills
open Simulation.Time.AdvanceTime
open Simulation.Queries

let private runYearlyEffects state time =
    if Calendar.Query.isFirstMomentOfYear time then
        [ state.GenreMarkets |> GenreMarket.update ]
    else
        []

let private runDailyEffects state time =
    match Calendar.Query.dayMomentOf time with
    | Morning ->
        Albums.DailyUpdate.dailyUpdate state
        |> (@) (Concerts.DailyUpdate.dailyUpdate state)
    | _ -> []

let rec private runCurrentTimeChecks state time =
    Concerts.Scheduler.moveFailedConcerts state time

let private runTimeDependentEffects time state =
    runDailyEffects state time
    |> (@) (runYearlyEffects state time)
    |> (@) (runCurrentTimeChecks state time)

let private runPlaceDependentEffects (_, coords) state =
    let resolvedCoords =
        Queries.World.Common.coordinates state coords

    match resolvedCoords.Content with
    | ResolvedPlaceCoordinates roomCoordinates ->
        let placeId, _ = roomCoordinates.Coordinates

        match roomCoordinates.Room with
        | Room.Stage -> Concerts.Scheduler.startScheduledConcerts state placeId
        | _ -> []
    | _ -> []

let private getAssociatedEffects effect =
    match effect with
    | SongStarted (band, _) ->
        [ Composition.improveBandSkillsAfterComposing band ]
    | SongImproved (band, _) ->
        [ Composition.improveBandSkillsAfterComposing band ]
    | SongPracticed (band, _) ->
        [ Composition.improveBandSkillsAfterComposing band ]
    | TimeAdvanced date -> [ runTimeDependentEffects date ]
    | WorldMoveTo coords -> [ runPlaceDependentEffects coords ]
    | _ -> []

/// Returns how many times the time has to be advanced for the given effect.
let private timeAdvanceOfEffect effect =
    match effect with
    | SongStarted _ -> 1
    | SongImproved _ -> 1
    | AlbumRecorded _ -> 56 // One week
    | SongPracticed _ -> 1
    | Wait times -> times
    | _ -> 0

let rec private tick' (appliedEffects, lastState) nextEffectFns =
    match nextEffectFns with
    | effectFn :: rest ->
        effectFn lastState
        |> List.fold
            (fun (currentEffectChain, currentState) effect ->
                let state =
                    State.Root.applyEffect currentState effect

                let associatedEffects =
                    getAssociatedEffects effect

                tick' (currentEffectChain @ [ effect ], state) associatedEffects)
            (appliedEffects, lastState)
        |> fun acc -> tick' acc rest
    | [] -> (appliedEffects, lastState)

/// Ticks the simulation, which applies the given effect to the state and
/// retrieves associated effects (for example: compose song -> improve skills)
/// and applies them to the state, gathering any associated effects of that one
/// until no effects are left and ticking the clock for how much the time
/// should be advanced for the given effect.
///
/// Returns a tuple with the list of all the effects that were applied in the
/// order in which they were applied and the updated state.
let tick currentState effect =
    let timeEffects =
        [ fun state ->
              timeAdvanceOfEffect effect
              |> advanceDayMoment state.Today ]

    let effectFn = fun _ -> [ effect ]

    tick' ([], currentState) (timeEffects @ [ effectFn ])
