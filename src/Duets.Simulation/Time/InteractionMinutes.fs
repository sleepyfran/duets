module Duets.Simulation.Time.InteractionMinutes

open Duets.Entities
open Duets.Simulation

/// Returns the number of minutes that a given effect takes to perform.
let effectMinutes =
    function
    | Ate _ -> 10<minute>
    | BookRead _ -> 45<minute>
    | CareerShiftPerformed(_, shiftDuration, _) ->
        (shiftDuration / 1<dayMoments>) * Config.Time.minutesPerDayMoment
    | Drank _ -> 5<minute>
    | GamePlayed _ -> 25<minute>
    | SongImproved _
    | SongStarted _ -> 30<minute>
    | _ -> 0<minute>
