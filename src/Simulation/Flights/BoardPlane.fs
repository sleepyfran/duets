module Simulation.Flights.BoardPlane

open Entities
open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames
open Simulation

/// Makes the character go through the airport's security check, which will
/// check for any non-permitted items and take them away from the character.
let passSecurityCheck state =
    Queries.Inventory.get state
    |> List.filter (fun item ->
        match item.Type with
        | Consumable (Drink _) -> true
        | _ -> false)
    |> List.map ItemRemovedFromInventory

let private calculateFlightTime flight =
    Queries.World.distanceBetween flight.Origin flight.Destination
    |> (*) 8<second / km>

/// Boards the plane to the given flight, returning how many hours the trip will
/// take and sets the situation to in-flight.
let boardPlane flight =
    let flightTimeInSeconds =
        calculateFlightTime flight

    let flightTimeInMinutes =
        flightTimeInSeconds / 60<second / minute>

    let situationEffects =
        Situations.onboardedInPlane flight

    situationEffects, flightTimeInMinutes
