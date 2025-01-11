module Duets.Simulation.Travel.Metro

open Duets.Entities
open Duets.Simulation

/// Attempts to wait for the next train to arrive if the character is currently
/// in a metro station and not overlapping with a train. Otherwise, does nothing.
let waitForNextTrain state =
    let currentMetroLine = Queries.Metro.tryCurrentStationLine state

    match currentMetroLine with
    | Some line ->
        let minutesToNextTrain = Queries.Metro.timeToOverlapWithTrain state line
        let currentTurnMinutes = Queries.Calendar.currentTurnMinutes state
        let total = currentTurnMinutes + minutesToNextTrain

        [ TurnTimeUpdated total ]
    | None -> []
