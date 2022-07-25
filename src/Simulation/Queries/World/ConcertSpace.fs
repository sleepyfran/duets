namespace Simulation.Queries.World

open Aether
open Aether.Operators
open Common
open Entities
open Data.World

module ConcertSpace =
    let private mapConcertSpaceFromCityNode fn cityNode =
        match cityNode with
        | CityNode.Place place ->
            match place.SpaceType with
            | ConcertSpace space -> fn place space
            | _ -> None
        | _ -> None

    let private mapConcertSpaceFromCoords fn nodeContent =
        match nodeContent with
        | ResolvedPlaceCoordinates coords ->
            match coords.Place.SpaceType with
            | ConcertSpace space -> fn coords.Place space
            | _ -> None
        | _ -> None

    /// Returns all concert spaces in the given city.
    let allInCity cityId =
        let graphNodesLenses =
            Lenses.World.cityGraph_ cityId
            >?> Lenses.World.Graph.nodes_

        Optic.get graphNodesLenses (World.get ())
        |> Option.map List.ofSeq
        |> Option.defaultValue []
        |> List.choose (fun kvp ->
            mapConcertSpaceFromCityNode
                (fun place space -> Some(kvp.Key, place, space))
                kvp.Value)
        |> List.distinctBy Tuple.fst3

    /// Retrieves a concert space given its node ID and the ID of the city
    /// that contains it.
    let byId cityId nodeId =
        let graphNodesLenses =
            Lenses.World.cityGraph_ cityId
            >?> Lenses.World.Graph.nodes_

        Optic.get graphNodesLenses (World.get ())
        |> Option.defaultValue Map.empty
        |> Map.tryFind nodeId
        |> Option.bind (
            mapConcertSpaceFromCityNode (fun place space -> Some(place, space))
        )

    let private closestRoom matchRoom state =
        let position = currentPosition state

        mapConcertSpaceFromCoords
            (fun place _ ->
                let (currentPlaceId, currentNodeId) =
                    match position.Content with
                    | ResolvedPlaceCoordinates coords -> coords.Coordinates
                    | ResolvedOutsideCoordinates coords ->
                        (coords.Coordinates, place.Rooms.StartingNode)

                availableDirections currentNodeId place.Rooms
                |> List.tryPick (fun (_, nodeId) ->
                    contentOf place.Rooms nodeId
                    |> matchRoom currentPlaceId nodeId))
            position.Content

    /// Finds the closest backstage space connected to the current position.
    let closestBackstage =
        closestRoom (fun currentPlaceId nodeId room ->
            match room with
            | Room.Backstage -> Room(currentPlaceId, nodeId) |> Some
            | _ -> None)

    /// Finds the closest stage connected to the current position.
    let closestStage =
        closestRoom (fun currentPlaceId nodeId room ->
            match room with
            | Room.Stage -> Room(currentPlaceId, nodeId) |> Some
            | _ -> None)
