[<RequireQualifiedAccess>]
module Simulation.Simulation


open Common
open Entities
open Simulation.Albums.DailyUpdate
open Simulation.Market
open Simulation.Skills.ImproveSkills
open Simulation.Time.AdvanceTime
open Simulation.Queries

let private runYearlyEffects state time =
    if Calendar.isFirstMomentOfYear time then
        [ state.GenreMarkets |> GenreMarket.update ]
    else
        []

let private runDailyEffects state time =
    match Calendar.dayMomentOf time with
    | Morning -> dailyUpdate state
    | _ -> []

let private runTimeDependentEffects state time =
    runDailyEffects state time
    |> (@) (runYearlyEffects state time)

let private run state effect =
    match effect with
    | SongStarted (band, _) -> improveBandSkillsAfterComposing state band
    | SongImproved (band, _) -> improveBandSkillsAfterComposing state band
    | SongPracticed (band, _) -> improveBandSkillsAfterComposing state band
    | TimeAdvanced date -> runTimeDependentEffects state date
    | _ -> []
    |> List.append [ effect ]

/// Returns how many times the time has to be advanced for the given effect.
let private timeAdvanceOfEffect effect =
    match effect with
    | SongStarted _ -> 1
    | SongImproved _ -> 1
    | AlbumRecorded _ -> 56 // One week
    | SongPracticed _ -> 1
    | _ -> 0

/// Takes an effect and runs all the correspondent simulation functions
/// gathering their effects as well and adding them to a final list with all the
/// effects that were created. Useful for situations in which an effect should
/// trigger other effects such as starting a new song or improving an existing
/// one, which should trigger an improvement in the band's skills. Returns the
/// resulting State after applying all the operations.
let runOne state effect =
    let effects =
        run state effect
        |> List.append (
            timeAdvanceOfEffect effect
            |> advanceDayMoment state.Today
            |> List.map (run state)
            |> List.concat
        )

    List.fold State.Root.applyEffect state effects
    |> Tuple.two effects
