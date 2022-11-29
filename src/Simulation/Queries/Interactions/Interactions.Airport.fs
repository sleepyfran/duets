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

    let private airplaneInteractions _ _ = []

    let interactions state =
        let situation =
            Queries.Situations.current state

        match situation with
        | Airport (Flying flight) -> airplaneInteractions state flight
        | _ -> airportInteractions state
