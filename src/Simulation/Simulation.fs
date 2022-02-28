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

let private runTimeDependentEffects state time =
    runDailyEffects state time
    |> (@) (runYearlyEffects state time)

let private getAssociatedEffects state effect =
    match effect with
    | SongStarted (band, _) -> improveBandSkillsAfterComposing state band
    | SongImproved (band, _) -> improveBandSkillsAfterComposing state band
    | SongPracticed (band, _) -> improveBandSkillsAfterComposing state band
    | TimeAdvanced date -> runTimeDependentEffects state date
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

let rec private tick' (appliedEffects, updatedState) nextEffects =
    match nextEffects with
    | effect :: rest ->
        let state =
            State.Root.applyEffect updatedState effect

        // TODO: Delay effect gathering
        // Delay execution of these effects to allow associated effect chaining.
        // (Example: album update -> fame update -> concert update w/ new fame)
        let associatedEffects = getAssociatedEffects updatedState effect

        tick' (appliedEffects @ [ effect ], state) (associatedEffects @ rest)
    | [] -> (appliedEffects, updatedState)

/// Ticks the simulation, which applies the given effect to the state and
/// retrieves associated effects (for example: compose song -> improve skills)
/// and applies them to the state, gathering any associated effects of that one
/// until no effects are left and ticking the clock for how much the time
/// should be advanced for the given effect.
///
/// Returns a tuple with the list of all the effects that were applied in the
/// order in which they were applied and the updated state.
let tick state effect =
    let timeEffects =
        timeAdvanceOfEffect effect
        |> advanceDayMoment state.Today

    tick' ([], state) (timeEffects @ [ effect ])
