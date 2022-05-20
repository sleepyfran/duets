module Simulation.Situations

open Entities

/// Sets the current situation to free roam.
let freeRoam = SituationChanged FreeRoam

/// Sets the current situation that the character is in to in concert with the
/// given ongoing concert. This should also be used to update the ongoing
/// concert situation.
let inConcert ongoingConcert =
    InConcert ongoingConcert |> SituationChanged