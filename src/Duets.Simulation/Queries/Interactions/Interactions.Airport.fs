namespace Duets.Simulation.Queries.Internal.Interactions

open Duets.Data.World
open Duets.Entities
open Duets.Entities.SituationTypes
open Duets.Simulation

[<RequireQualifiedAccess>]
module Airport =
    let private airportInteractions state =
        let todayFlight =
            Queries.Flights.availableForBoarding state

        match todayFlight with
        | Some flight ->
            [ AirportInteraction.BoardAirplane flight
              |> Interaction.Airport ]
        | _ -> []

    let private airplaneInteractions _ flight defaultInteractions =
        let nonMovementInteractions =
            defaultInteractions
            |> List.filter (fun interaction ->
                match interaction with
                | Interaction.FreeRoam FreeRoamInteraction.Map -> false
                | Interaction.FreeRoam FreeRoamInteraction.Wait -> false
                | _ -> true)

        [ yield! nonMovementInteractions
          yield!
              Shop.interactions
                  { AvailableItems = AirplaneItems.drinks @ AirplaneItems.food
                    PriceModifier = 10<multiplier> }
          AirportInteraction.WaitUntilLanding flight
          |> Interaction.Airport ]

    let interactions state defaultInteractions =
        let situation =
            Queries.Situations.current state

        match situation with
        | Airport (Flying flight) ->
            airplaneInteractions state flight defaultInteractions
        | _ -> airportInteractions state @ defaultInteractions
