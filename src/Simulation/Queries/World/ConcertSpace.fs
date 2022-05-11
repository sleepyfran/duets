namespace Simulation.Queries.World

open Aether
open Aether.Operators
open Entities
open Simulation.Queries.World

module ConcertSpace =
    let private mapConcertSpaceFromCityNode fn cityNode =
        match cityNode with
        | CityNode.Place place ->
            match place.Space with
            | ConcertSpace space -> fn place space
            | _ -> None
        | _ -> None

    let private mapConcertSpaceFromCoords fn nodeContent =
        match nodeContent with
        | ResolvedPlaceCoordinates coords ->
            match coords.Place.Space with
            | ConcertSpace space -> fn coords.Place space
            | _ -> None
        | _ -> None

    /// Returns all concert spaces in the given city.
    let allInCity state cityId =
        let graphNodesLenses =
            Lenses.FromState.World.cityGraph_ cityId
            >?> Lenses.World.Graph.nodes_

        Optic.get graphNodesLenses state
        |> Option.map List.ofSeq
        |> Option.defaultValue []
        |> List.choose (fun kvp ->
            mapConcertSpaceFromCityNode
                (fun _ space -> Some(kvp.Key, space))
                kvp.Value)
        |> List.distinctBy fst

    /// Retrieves a concert space given its node ID and the ID of the city
    /// that contains it.
    let byId state cityId nodeId =
        let graphNodesLenses =
            Lenses.FromState.World.cityGraph_ cityId
            >?> Lenses.World.Graph.nodes_

        Optic.get graphNodesLenses state
        |> Option.defaultValue Map.empty
        |> Map.tryFind nodeId
        |> Option.bind (mapConcertSpaceFromCityNode (fun _ -> Some))

    let private closestRoom state matchRoom =
        let position = Common.currentPosition state

        mapConcertSpaceFromCoords
            (fun place _ ->
                let (currentPlaceId, currentNodeId) =
                    match position.Content with
                    | ResolvedPlaceCoordinates coords -> coords.Coordinates
                    | ResolvedOutsideCoordinates coords ->
                        (coords.Coordinates, place.Rooms.StartingNode)

                Common.availableDirections currentNodeId place.Rooms
                |> List.tryPick (fun (_, nodeId) ->
                    Common.contentOf place.Rooms nodeId
                    |> matchRoom currentPlaceId nodeId))
            position.Content

    /// Finds the closest backstage space connected to the current position.
    let closestBackstage state =
        closestRoom state (fun currentPlaceId nodeId room ->
            match room with
            | Room.Backstage -> Room(currentPlaceId, nodeId) |> Some
            | _ -> None)

    /// Finds the closest stage connected to the current position.
    let closestStage state =
        closestRoom state (fun currentPlaceId nodeId room ->
            match room with
            | Room.Stage -> Room(currentPlaceId, nodeId) |> Some
            | _ -> None)
