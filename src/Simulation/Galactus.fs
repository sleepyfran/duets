/// This name was deliberately chosen because I have no idea how to properly
/// name this module, so what's better than referencing this piece of art:
/// https://www.youtube.com/watch?v=y8OnoxKotPQ
module Simulation.Galactus

open Entities
open Simulation.Albums.DailyUpdate
open Simulation.Skills.ImproveSkills
open Simulation.Time.AdvanceTime
open Simulation.Queries

let private runTimeDependentEffects state time =
    match Calendar.dayMomentOf time with
    | Morning -> dailyUpdate state
    | _ -> []

let private run state effect =
    match effect with
    | SongStarted (band, _) -> improveBandSkillsAfterComposing state band
    | SongImproved (band, _) -> improveBandSkillsAfterComposing state band
    | TimeAdvanced date -> runTimeDependentEffects state date
    | _ -> []
    |> List.append [ effect ]

/// Returns how many times the time has to be advanced for the given effect.
let private timeAdvanceOfEffect effect =
    match effect with
    | SongStarted _ -> 1
    | SongImproved _ -> 1
    | AlbumRecorded _ -> 2
    | _ -> 0

/// Selects the max time advance that can be applied out all of the effects.
let private timeAdvanceOfEffects effects =
    effects
    |> List.map timeAdvanceOfEffect
    |> List.max

/// Takes an effect and runs all the correspondent simulation functions
/// gathering their effects as well and adding them to a final list with all the
/// effects that were created. Useful for situations in which an effect should
/// trigger other effects such as starting a new song or improving an existing
/// one, which should trigger an improvement in the band's skills.
let runOne state effect =
    run state effect
    |> List.append [ timeAdvanceOfEffect effect
                     |> advanceTimeTimes state.Today
                     |> TimeAdvanced ]

/// Takes multiple effects and runs them through `runOne` combining all
/// the resulting effects into a list.
let runMultiple state effects =
    effects
    |> List.map (run state)
    |> List.concat
    |> List.append [ timeAdvanceOfEffects effects
                     |> advanceTimeTimes state.Today
                     |> TimeAdvanced ]
