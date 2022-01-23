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
        |> Option.map List.ofMapValues
        |> Option.defaultValue []
        |> List.choose World.Node.concertSpace
        |> List.distinctBy (fun space -> space.Name)

    /// Returns a specific city given its ID.
    let cityById state cityId =
        Optic.get (Lenses.FromState.World.city_ cityId) state

    /// Retrieves a concert space given its node ID and the ID of the city
    /// that contains it.
    let concertSpaceByName state cityId name =
        let graphNodesLenses =
            Lenses.FromState.World.cityGraph_ cityId
            >?> Lenses.World.Graph.nodes_

        Optic.get graphNodesLenses state
        |> Option.map List.ofMapValues
        |> Option.defaultValue []
        |> List.choose World.Node.concertSpace
        |> List.tryFind (fun space -> space.Name = name)

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
