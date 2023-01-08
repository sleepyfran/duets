module Simulation.Events.Time

open Entities
open Simulation
open Simulation.Calendar
open Simulation.Market

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
    @ Notifications.createHappeningSoon state time
      @ Place.ClosingTime.checkCurrentPlace state time

/// Runs all the events associated with the current time of the game, this
/// includes all daily, yearly and current time effects such as updating the
/// streams of albums, failing concerts that haven't happened, etc.
let internal run time state =
    runDailyEffects state time
    |> (@) (runYearlyEffects state time)
    |> (@) (runCurrentTimeChecks state time)
