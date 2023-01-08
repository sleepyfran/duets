module Simulation.Events.Place.ClosingTime (* Open all the doors and let you out into the wooooorld... *)

open Entities
open Simulation

/// Checks if the given time is outside of the opening hours of the place
/// the character is currently in and, if so, creates a PlaceClosed effect.
let checkCurrentPlace state time =
    let currentPlace = Queries.World.currentPlace state

    let currentlyClosed =
        Queries.World.placeCurrentlyOpen currentPlace time |> not

    if currentlyClosed then
        [ Effect.PlaceClosed currentPlace ]
    else
        []
