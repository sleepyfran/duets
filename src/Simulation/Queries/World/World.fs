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

    /// Returns the content of the given coordinates and an optional
    /// ID to a room inside that place (if any).
    let rec coordinates state coords =
        let (cityId, _) = state.CurrentPosition

        let city =
            Optic.get (Lenses.FromState.World.city_ cityId) state
            |> Option.get

        match coords with
        | Room (placeId, roomId) ->
            let cityNode =
                Optic.get (Lenses.FromState.World.node_ cityId placeId) state
                |> Option.get

            match cityNode with
            | CityNode.Place place ->
                let roomContent =
                    Optic.get (Lenses.World.Graph.node_ roomId) place.Rooms
                    |> Option.get

                { City = city
                  Content =
                    ResolvedPlaceCoordinates
                        { Coordinates = RoomCoordinates(placeId, roomId)
                          Place = place
                          Room = roomContent } }
            | _ ->
                failwith "Cannot reference outside node with place coordinates"
        | Node nodeId ->
            let cityNode =
                Optic.get (Lenses.FromState.World.node_ cityId nodeId) state
                |> Option.get

            match cityNode with
            | CityNode.OutsideNode outsideNode ->
                { City = city
                  Content =
                    ResolvedOutsideCoordinates
                        { Coordinates = nodeId
                          Node = outsideNode } }
            | CityNode.Place place ->
                coordinates state (Room(nodeId, place.Rooms.StartingNode))

    /// Returns the content of the current position of the player and an optional
    /// ID to a room inside that place (if any).
    let currentPosition state =
        let (_, nodeCoordinates) = state.CurrentPosition

        coordinates state nodeCoordinates

    /// <summary>
    /// Returns the resolved coordinates of a place or None if the given coordinates
    /// point to a non-place node.
    /// </summary>
    let coordinatesOfPlace state coords =
        let resolvedCoords = coordinates state coords

        match resolvedCoords.Content with
        | ResolvedPlaceCoordinates placeCoords -> Some placeCoords
        | _ -> None

    /// <summary>
    /// Returns the resolved coordinates of an ourtside node or None if the given
    /// coordinates point to a non-outside node.
    /// </summary>
    let coordinatesOfOutsideNode state coords =
        let resolvedCoords = coordinates state coords

        match resolvedCoords.Content with
        | ResolvedOutsideCoordinates outsideCoords -> Some outsideCoords
        | _ -> None
