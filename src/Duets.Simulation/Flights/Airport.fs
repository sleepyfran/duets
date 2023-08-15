module Duets.Simulation.Flights.Airport

open Duets.Entities
open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames
open Duets.Simulation
open Duets.Simulation.Time
open Duets.Simulation.Navigation

/// Makes the character go through the airport's security check, which will
/// check for any non-permitted items and take them away from the character.
let passSecurityCheck state =
    Queries.Inventory.get state
    |> List.filter (fun item ->
        match item.Type with
        | Consumable(Drink _) -> true
        | _ -> false)
    |> List.map ItemRemovedFromInventory

/// Boards the plane to the given flight, returning how many hours the trip will
/// take and sets the situation to in-flight.
let boardPlane flight =
    let flightTimeInSeconds = Queries.Flights.flightTime flight

    let flightTimeInMinutes = flightTimeInSeconds / 60<second / minute>

    let situationEffect = Situations.onboardedInPlane flight

    [ situationEffect; FlightUpdated { flight with AlreadyUsed = true } ],
    flightTimeInMinutes

/// Passes as many day moments needed for the flight to complete and leaves
/// the character in the destination's airport.
let leavePlane state flight =
    let dayMomentsNeeded =
        AirportInteraction.WaitUntilLanding flight
        |> Interaction.Airport
        |> Queries.InteractionTime.timeRequired

    let destinationAirport =
        Queries.World.placeIdsByTypeInCity
            flight.Destination
            PlaceTypeIndex.Airport
        |> List.head (* All cities must have an airport. *)

    [ yield! AdvanceTime.advanceDayMoment' state dayMomentsNeeded
      Navigation.travelTo flight.Destination destinationAirport state
      Situations.freeRoam ]
