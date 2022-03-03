namespace Simulation.Queries

open Aether
open Aether.Operators
open Common
open Entities

module World =
    /// Returns all cities available in the game world.
    let allCities state =
        Optic.get Lenses.FromState.World.cities_ state
        |> List.ofMapValues

    /// Returns all concert spaces in the given city.
    let allConcertSpacesOfCity state cityId =
        let graphNodesLenses =
            Lenses.FromState.World.cityGraph_ cityId
            >?> Lenses.World.Graph.nodes_

        Optic.get graphNodesLenses state
        |> Option.map List.ofSeq
        |> Option.defaultValue []
        |> List.choose
            (fun kvp ->
                match kvp.Value with
                | ConcertPlace place -> Some(kvp.Key, place.Space)
                | _ -> None)
        |> List.distinctBy fst

    /// Returns a specific city given its ID.
    let cityById state cityId =
        Optic.get (Lenses.FromState.World.city_ cityId) state

    /// Retrieves a concert space given its node ID and the ID of the city
    /// that contains it.
    let concertSpaceById state cityId nodeId =
        let graphNodesLenses =
            Lenses.FromState.World.cityGraph_ cityId
            >?> Lenses.World.Graph.nodes_

        Optic.get graphNodesLenses state
        |> Option.defaultValue Map.empty
        |> Map.tryFind nodeId
        |> Option.bind
            (fun node ->
                match node with
                | ConcertPlace place -> Some place.Space
                | _ -> None)

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
