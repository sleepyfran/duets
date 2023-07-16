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

    let private airportInteractions state =
        let todayFlight = Queries.Flights.availableForBoarding state

        match todayFlight with
        | Some flight ->
            [ AirportInteraction.BoardAirplane flight |> Interaction.Airport ]
        | _ -> []

    let private airplaneInteractions _ flight defaultInteractions =
        let allowedInteractions =
            Queries.InteractionCommon.filterOutMovementAndTime
                defaultInteractions

        [ yield! allowedInteractions
          yield!
              inFlightItems flight.Origin flight.Destination
              |> Shop.interactions
          AirportInteraction.WaitUntilLanding flight |> Interaction.Airport ]

    let internal interactions state defaultInteractions =
        let situation = Queries.Situations.current state

        match situation with
        | Airport(Flying flight) ->
            airplaneInteractions state flight defaultInteractions
        | _ -> airportInteractions state @ defaultInteractions
