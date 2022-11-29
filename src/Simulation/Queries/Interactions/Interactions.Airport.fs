namespace Simulation.Queries.Internal.Interactions

open Entities
open Entities.SituationTypes
open Simulation

[<RequireQualifiedAccess>]
module Airport =
    let private airportInteractions state =
        let todayFlight =
            Queries.Flights.today state

        match todayFlight with
        | Some flight ->
            [ AirportInteraction.BoardAirplane flight
              |> Interaction.Airport ]
        | _ -> []

    let private airplaneInteractions state flight defaultInteractions =
        defaultInteractions
        |> List.filter (fun interaction ->
            match interaction with
            | Interaction.FreeRoam FreeRoamInteraction.Map -> false
            | Interaction.FreeRoam FreeRoamInteraction.Wait -> false
            | _ -> true)

    let interactions state defaultInteractions =
        let situation =
            Queries.Situations.current state

        match situation with
        | Airport (Flying flight) ->
            airplaneInteractions state flight defaultInteractions
        | _ -> airportInteractions state
