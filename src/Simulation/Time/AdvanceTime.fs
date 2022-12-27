module Simulation.Time.AdvanceTime

open Common
open Entities
open Simulation

/// Advances the current time to the next day moment by the given number of
/// times. For example, if currently it's morning, this will advance to midday.
/// Also handles the cases in which it's already midnight, in which case it'll
/// return the dawn of next day.
let advanceDayMoment (currentTime: Date) (times: int<dayMoments>) =
    [ 1 .. (times / 1<dayMoments>) ]
    |> List.mapFold
        (fun time _ ->
            Calendar.Query.next time
            |> fun advancedTime -> (TimeAdvanced advancedTime, advancedTime))
        currentTime
    |> fst

/// Same as advanceDayMoment but queries the current time automatically.
let advanceDayMoment' state times =
    let currentDate = Queries.Calendar.today state

    advanceDayMoment currentDate times
