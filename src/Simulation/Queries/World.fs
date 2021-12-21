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

    /// Returns the current coordinates in which the player is located. These
    /// coordinates are basically the ID of the city, node and the content that
    /// the position includes.
    let currentPosition state =
        let (currentCityId, currentNodeId) = state.CurrentPosition

        let positionContent =
            Optic.get
                (Lenses.FromState.World.position_ currentCityId currentNodeId)
                state
            |> Option.get

        (currentCityId, currentNodeId, positionContent)
