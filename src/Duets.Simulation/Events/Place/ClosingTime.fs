module Duets.Simulation.Events.Place.ClosingTime (* Open all the doors and let you out into the wooooorld... *)

open Duets.Entities
open Duets.Simulation

/// Checks if the given time is outside of the opening hours of the place
/// the character is currently in and, if so, creates a PlaceClosed effect.
let checkCurrentPlace state =
    let currentPlace = Queries.World.currentPlace state
    let currentTime = Queries.Calendar.today state

    let currentlyClosed =
        Queries.World.placeCurrentlyOpen currentPlace currentTime |> not

    if currentlyClosed then
        [ Effect.PlaceClosed currentPlace ]
    else
        []
