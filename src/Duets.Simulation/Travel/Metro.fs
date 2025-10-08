module Duets.Simulation.Travel.Metro

open Duets.Entities
open Duets.Simulation

/// Attempts to wait for the next train to arrive if the character is currently
/// in a metro station and not overlapping with a train. Otherwise, does nothing.
let waitForNextTrain state =
    // TODO: We only consider the first line for now. Maybe take the one with the shortest time?
    Queries.Metro.currentStationLines state
    |> List.tryHead
    |> Option.map (fun line ->
        let minutesToNextTrain =
            Queries.Metro.timeToOverlapWithTrain state line

        let currentTurnMinutes = Queries.Calendar.currentTurnMinutes state
        let total = currentTurnMinutes + minutesToNextTrain

        [ TurnTimeUpdated total ])
    |> Option.defaultValue []
