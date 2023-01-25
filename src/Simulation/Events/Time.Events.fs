module Simulation.Events.Time

open Entities
open Simulation
open Simulation.Calendar
open Simulation.Character
open Simulation.Market

let private runYearlyEffects time state =
    if Calendar.Query.isFirstMomentOfYear time then
        [ state.GenreMarkets |> GenreMarket.update ]
    else
        []

let private runDailyEffects time state =
    match Calendar.Query.dayMomentOf time with
    | Morning ->
        Albums.DailyUpdate.dailyUpdate state
        |> (@) (Concerts.DailyUpdate.dailyUpdate state)
    | _ -> []

let rec private runCurrentTimeChecks time state =
    Concerts.Scheduler.moveFailedConcerts state time
    @ Notifications.createHappeningSoon state time
      @ AttributeChange.applyAfterTimeChange state

/// Runs all the events associated with the current time of the game, this
/// includes all daily, yearly and current time effects such as updating the
/// streams of albums, failing concerts that haven't happened, etc.
let internal run effect =
    match effect with
    | TimeAdvanced time ->
        [ runDailyEffects time
          runYearlyEffects time
          runCurrentTimeChecks time ]
    | Wait times -> [ AttributeChange.applyAfterWait times ]
    | _ -> []
