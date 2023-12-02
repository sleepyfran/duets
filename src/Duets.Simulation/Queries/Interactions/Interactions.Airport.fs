namespace Duets.Simulation.Queries.Internal.Interactions

open Duets.Data.Items
open Duets.Entities
open Duets.Entities.SituationTypes
open Duets.Simulation

[<RequireQualifiedAccess>]
module Airport =
    let private inFlightItems origin destination =
        let originDrinks = Drink.Beer.byLocation |> Map.find origin

        let destinationDrinks = Drink.Beer.byLocation |> Map.find destination

        originDrinks
        @ destinationDrinks
        @ Drink.Coffee.all
        @ Drink.SoftDrinks.all
        |> List.map (fun (item, price) -> (item, price * 3m))

    let private airportInteractions state cityId roomType =
        let todayFlight = Queries.Flights.availableForBoarding state

        let shopInteractions =
            Restaurant.interactions cityId roomType @ Cafe.interactions roomType

        match todayFlight, roomType with
        | Some _, RoomType.SecurityControl ->
            [ AirportInteraction.PassSecurity |> Interaction.Airport ]
        | Some flight, RoomType.BoardingGate ->
            [ AirportInteraction.BoardAirplane flight |> Interaction.Airport ]
        | _ -> []
        @ shopInteractions

    let private airplaneInteractions _ flight =
        [ yield!
              inFlightItems flight.Origin flight.Destination
              |> Shop.interactions
          AirportInteraction.WaitUntilLanding flight |> Interaction.Airport ]

    let internal interactions state cityId roomType =
        let situation = Queries.Situations.current state

        match situation with
        | Airport(Flying flight) -> airplaneInteractions state flight
        | _ -> airportInteractions state cityId roomType
