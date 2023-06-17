module Duets.Entities.World

open Aether
open Duets.Entities

/// Creates an empty world.
let empty = { Cities = Map.empty }

/// Creates a new world with the given cities inside of it.
let create (cities: City list) =
    { Cities = cities |> List.map (fun c -> c.Id, c) |> Map.ofList }

[<RequireQualifiedAccess>]
module Graph =
    /// Creates a graph with the given starting node as the starting point, that
    /// node as the only node available and no connections.
    let from id content =
        { StartingNode = id
          Nodes = [ (id, content) ] |> Map.ofList
          Connections = Map.empty }

    /// Adds a new node with no connections to the given graph.
    let addNode<'a> id content =
        Optic.map Lenses.World.Graph.nodes_ (Map.add id content)

    /// Adds a connection from the first node ID to the second in the given
    /// direction. Also adds a second connection from the second node ID to
    /// the first as the opposite direction of the given one, so that if the
    /// given direction is north then the second connection will be generated
    /// in the south.
    let addConnection fromNodeId toNodeId direction graph =
        let oppositeDirection =
            match direction with
            | North -> South
            | NorthEast -> SouthWest
            | East -> West
            | SouthEast -> NorthWest
            | South -> North
            | SouthWest -> NorthEast
            | West -> East
            | NorthWest -> SouthEast

        graph
        |> Optic.map
            (Lenses.World.Graph.nodeConnections_ fromNodeId)
            (Map.add direction toNodeId)
        |> Optic.map
            (Lenses.World.Graph.nodeConnections_ toNodeId)
            (Map.add oppositeDirection fromNodeId)

[<RequireQualifiedAccess>]
module Place =
    /// Creates a place with the given initial room and no exits.
    let create id name quality placeType rooms zone =
        { Id = PlaceId id
          Name = name
          Quality = quality
          Type = placeType
          OpeningHours = PlaceOpeningHours.AlwaysOpen
          Rooms = rooms
          Zone = zone }

    /// Changes the opening hours to a certain days and day moments.
    let changeOpeningHours openingHours place =
        { place with
            OpeningHours = openingHours }

[<RequireQualifiedAccess>]
module City =
    let private addToPlaceByTypeIndex place index =
        let mapKey =
            match place.Type with
            | Airport -> PlaceTypeIndex.Airport
            | Bar _ -> PlaceTypeIndex.Bar
            | Cafe _ -> PlaceTypeIndex.Cafe
            | ConcertSpace _ -> PlaceTypeIndex.ConcertSpace
            | Home -> PlaceTypeIndex.Home
            | Hospital -> PlaceTypeIndex.Hospital
            | RehearsalSpace _ -> PlaceTypeIndex.RehearsalSpace
            | Restaurant _ -> PlaceTypeIndex.Restaurant
            | Studio _ -> PlaceTypeIndex.Studio

        Map.change
            mapKey
            (function
            | Some list -> list @ [ place.Id ] |> Some
            | None -> [ place.Id ] |> Some)
            index

    let private addToZoneIndex place =
        Map.change place.Zone.Id (function
            | Some list -> list @ [ place.Id ] |> Some
            | None -> [ place.Id ] |> Some)

    /// Creates a city with only one initial starting node.
    let create id costModifier place =
        { Id = id
          PlaceByTypeIndex = addToPlaceByTypeIndex place Map.empty
          PlaceIndex = [ (place.Id, place) ] |> Map.ofList
          PlaceCostModifier = costModifier
          ZoneIndex = addToZoneIndex place Map.empty }

    /// Adds a new place to the city.
    let addPlace place city =
        Optic.map
            Lenses.World.City.placeByTypeIndex_
            (addToPlaceByTypeIndex place)
            city
        |> Optic.map Lenses.World.City.placeIndex_ (Map.add place.Id place)
        |> Optic.map Lenses.World.City.zoneIndex_ (addToZoneIndex place)
