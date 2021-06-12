module Simulation.Time.AdvanceTime

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
    |> Calendar.withDayMoment Dawn

/// Advances the current time to the next day moment. For example, if currently
/// it's morning, this will advance to midday. Also handles the cases in which
/// it's already midnight, in which case it'll return the dawn of next day.
let advanceTimeOnce (currentTime: Date) =
    if Calendar.dayMomentOf currentTime = Midnight then
        advanceDay currentTime
    else
        advanceOnce currentTime

/// Advances the time by the given number of times.
let advanceTimeTimes (currentTime: Date) times =
    [ 1 .. times ]
    |> List.fold (fun time _ -> advanceTimeOnce time) currentTime
