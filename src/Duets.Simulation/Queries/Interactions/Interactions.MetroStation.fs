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
            [] (* TODO: Implement travelling interactions. *)
        | _, false ->
            (* If the time does not overlap, let the player wait for the next train. *)
            [ TravelInteraction.WaitForMetro |> Interaction.Travel ]
        | _ -> []
