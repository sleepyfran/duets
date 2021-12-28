namespace Simulation.Queries

open Aether
open Entities

module World =
    /// Returns the content of the given node in the graph.
    let contentOf id (graph: Graph<'a>) =
        Optic.get (Lenses.World.Graph.node_ id) graph
        |> Option.get

    /// Returns a list of directions that indicated which
    let availableDirections id (graph: Graph<'a>) =
        Optic.get (Lenses.World.Graph.nodeConnections_ id) graph
        |> Option.defaultValue Map.empty
        |> List.ofSeq
        |> List.map (fun keyValue -> (keyValue.Key, keyValue.Value))

    /// Returns the content of the current position of the player and an optional
    /// ID to a room inside that place (if any).
    let currentPosition state =
        let (currentCityId, currentNodeId) = state.CurrentPosition

        let city =
            Optic.get (Lenses.FromState.World.city_ currentCityId) state
            |> Option.get

        let positionContent =
            Optic.get
                (Lenses.FromState.World.node_ currentCityId currentNodeId)
                state
            |> Option.get

        {| City = city
           NodeId = currentNodeId
           NodeContent = positionContent |}
