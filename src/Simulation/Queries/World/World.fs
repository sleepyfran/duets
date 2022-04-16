namespace Simulation.Queries.World

open Aether
open Common
open Entities

module Common =
    /// Returns all cities available in the game world.
    let allCities state =
        Optic.get Lenses.FromState.World.cities_ state
        |> List.ofMapValues

    /// Returns a specific city given its ID.
    let cityById state cityId =
        Optic.get (Lenses.FromState.World.city_ cityId) state

    /// Returns the content of the given node in the graph.
    let contentOf (graph: Graph<'a>) id =
        Optic.get (Lenses.World.Graph.node_ id) graph
        |> Option.get

    /// Returns a list of directions that indicated which
    let availableDirections id (graph: Graph<'a>) =
        Optic.get (Lenses.World.Graph.nodeConnections_ id) graph
        |> Option.defaultValue Map.empty
        |> List.ofSeq
        |> List.map (fun keyValue -> (keyValue.Key, keyValue.Value))

    /// Returns the coordinates of an exit, if there's any, linked to the
    /// current node.
    let exitsOfNode currentNodeId exits =
        exits
        |> Map.tryFind currentNodeId
        |> Option.map Node

    /// Returns the room ID if the given coordinates are inside a room, or None
    /// otherwise.
    let roomIdFromCoordinates coordinates =
        match coordinates with
        | Room (_, roomId) -> Some roomId
        | Node _ -> None

    /// Returns the content of the current position of the player and an optional
    /// ID to a room inside that place (if any).
    let currentPosition state =
        let (cityId, nodeCoordinates) = state.CurrentPosition

        let city =
            Optic.get (Lenses.FromState.World.city_ cityId) state
            |> Option.get

        let nodeId =
            match nodeCoordinates with
            | Room (placeId, _) -> placeId
            | Node nodeId -> nodeId

        let cityNodeContent =
            Optic.get (Lenses.FromState.World.node_ cityId nodeId) state
            |> Option.get

        {| City = city
           Coordinates = nodeCoordinates
           NodeContent = cityNodeContent |}