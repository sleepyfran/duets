namespace Duets.Simulation.Queries.Internal.Interactions

open Duets.Entities
open Duets.Entities.SituationTypes
open Duets.Simulation

[<RequireQualifiedAccess>]
module MetroStation =
    let internal interactions state =
        let situation = Queries.Situations.current state

        let currentLines = Queries.Metro.currentStationLines state

        let atLeastOneLineOverlaps =
            currentLines
            |> List.map (Queries.Metro.timeOverlapsWithWaitingTime state)
            |> List.exists id

        match situation, atLeastOneLineOverlaps with
        | Travelling Metro, _ ->
            Queries.Metro.tryCurrentStation state
            |> Option.map (Queries.Metro.stationLineConnections state)
            |> Option.map (
                TravelInteraction.TravelByMetroTo
                >> Interaction.Travel
                >> List.singleton
            )
            |> Option.defaultValue []
            |> (@) [ TravelInteraction.LeaveMetro |> Interaction.Travel ]
        | _, false ->
            (* If the time does not overlap, let the player wait for the next train. *)
            [ TravelInteraction.WaitForMetro |> Interaction.Travel ]
        | _ -> []
