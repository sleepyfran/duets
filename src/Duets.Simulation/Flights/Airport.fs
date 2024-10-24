module Duets.Simulation.Flights.Airport

open Duets.Common
open Duets.Entities
open Duets.Data
open Duets.Simulation
open Duets.Simulation.Time
open Duets.Simulation.Navigation
open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames

/// Makes the character go through the airport's security check, which will
/// check for any non-permitted items and take them away from the character.
let passSecurityCheck state =
    let itemRemovalEffects =
        Queries.Inventory.character state
        |> List.filter (fun item ->
            item
            |> Item.Property.has (function
                | Drinkable _ -> true
                | _ -> false))
        |> List.map (fun item -> ItemRemovedFromCharacterInventory item)

    let movementEffect =
        Navigation.enter World.Ids.Airport.boardingGate state
        |> Result.unwrap (* The airport is guaranteed to have an open boarding gate. *)

    [ yield! itemRemovalEffects; movementEffect; AirportSecurityPassed ]

/// Boards the plane to the given flight, returning how many hours the trip will
/// take and sets the situation to in-flight.
let boardPlane flight =
    let flightTimeInSeconds = Queries.Flights.flightTime flight

    let flightTimeInMinutes = flightTimeInSeconds / 60<second / minute>

    let situationEffect = Situations.onboardedInPlane flight

    [ situationEffect
      FlightUpdated { flight with AlreadyUsed = true }
      FlightBoarded flight ],
    flightTimeInMinutes

/// Passes as many day moments needed for the flight to complete and leaves
/// the character in the destination's airport.
let leavePlane state flight =
    let destinationAirport =
        Queries.World.placeIdsByTypeInCity
            flight.Destination
            PlaceTypeIndex.Airport
        |> List.head (* All cities must have an airport. *)

    [ Navigation.travelTo flight.Destination destinationAirport state
      FlightLanded flight
      Situations.freeRoam ]
