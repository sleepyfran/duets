module Simulation.Time.AdvanceTime

open Common
open Fugit.Shorthand
open Entities

let private advanceOnce currentTime =
    match Calendar.dayMomentOf currentTime with
    | Dawn -> Morning
    | Morning -> Midday
    | Midday -> Sunset
    | Sunset -> Dusk
    | Dusk -> Night
    | Night -> Midnight
    | Midnight -> Dawn
    |> Calendar.withDayMoment' currentTime

let private advanceDay (currentTime: Date) =
    currentTime + oneDay
    |> Calendar.withDayMoment Midnight

let private advanceTimeOnce (currentTime: Date) =
    (if Calendar.dayMomentOf currentTime = Night then
         advanceDay currentTime
     else
         advanceOnce currentTime)

/// Advances the current time to the next day moment by the given number of
/// times. For example, if currently it's morning, this will advance to midday.
/// Also handles the cases in which it's already midnight, in which case it'll
/// return the dawn of next day.
let advanceDayMoment (currentTime: Date) times =
    [ 1 .. times ]
    |> List.mapFold
        (fun time _ ->
            advanceTimeOnce time
            |> fun advancedTime -> (TimeAdvanced advancedTime, advancedTime))
        currentTime
    |> fst
