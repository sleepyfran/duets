module Duets.Simulation.Events.Time

open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Calendar
open Duets.Simulation.Character
open Duets.Simulation.Market
open Duets.Simulation.Time

let private runYearlyEffects time state =
    if Calendar.Query.isFirstMomentOfYear time then
        [ state.GenreMarkets |> GenreMarket.update ]
    else
        []

let private runDailyEffects time state =
    match Calendar.Query.dayMomentOf time with
    | Morning ->
        Albums.DailyUpdate.dailyUpdate state
        @ Albums.ReviewGeneration.generateReviewsForLatestAlbums state
        @ Concerts.DailyUpdate.dailyUpdate state
        @ Place.RentalExpiration.expireRentals state time
    | Midday -> SocialNetworks.DailyUpdate.dailyUpdate state
    | _ -> []

let rec private runCurrentTimeChecks time state =
    Concerts.Scheduler.moveFailedConcerts state time
    @ Notifications.createNotifications state time
    @ AttributeChange.applyAfterTimeChange state
    @ SocialNetworks.Reposts.applyToLatestAfterTimeChange state

/// Runs all the events associated with the current time of the game, this
/// includes all daily, yearly and current time effects such as updating the
/// streams of albums, failing concerts that haven't happened, etc.
let internal run effect =
    match effect with
    | TimeAdvanced time ->
        [ runDailyEffects time
          runYearlyEffects time
          runCurrentTimeChecks time ]
        |> ContinueChain
        |> Some
    | Wait times ->
        [ fun state -> AdvanceTime.advanceDayMoment' state times
          AttributeChange.applyAfterWait times ]
        |> ContinueChain
        |> Some
    | _ -> None
