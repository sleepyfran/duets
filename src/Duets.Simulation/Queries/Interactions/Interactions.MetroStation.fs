namespace Duets.Simulation.Queries.Internal.Interactions

open Duets.Entities
open Duets.Entities.SituationTypes
open Duets.Simulation

[<RequireQualifiedAccess>]
module MetroStation =
    let internal interactions state =
        let situation = Queries.Situations.current state

        let currentLine =
            Queries.Metro.tryCurrentStationLine state
            |> Option.get (* We know we are in a metro station, so safe to unwrap. *)

        let timeOverlaps =
            Queries.Metro.timeOverlapsWithWaitingTime state currentLine

        match situation, timeOverlaps with
        | Travelling Metro, _ ->
            Queries.Metro.tryCurrentStation state
            |> Option.bind (Queries.Metro.stationConnections state)
            |> Option.map (fun connections ->
                TravelInteraction.TravelByMetroTo(connections, currentLine)
                |> Interaction.Travel
                |> List.singleton)
            |> Option.defaultValue []
        | _, false ->
            (* If the time does not overlap, let the player wait for the next train. *)
            [ TravelInteraction.WaitForMetro |> Interaction.Travel ]
        | _ -> []
