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
    let from (node: Node<'a>) =
        { StartingNode = node.Id
          Nodes = [ (node.Id, node.Content) ] |> Map.ofList
          Connections = Map.empty }

    /// Creates a graph with the given nodes as the only nodes available and no
    /// connections. Chooses the head of the node list as the starting node. If
    /// an empty list is given, the function fails.
    let fromMany (nodes: Node<'a> list) =
        let startingNode = nodes |> List.head

        { StartingNode = startingNode.Id
          Nodes = nodes |> List.map (fun n -> n.Id, n.Content) |> Map.ofList
          Connections = Map.empty }

    /// Adds a new node with no connections to the given graph.
    let addNode (node: Node<'a>) =
        Optic.map Lenses.World.Graph.nodes_ (Map.add node.Id node.Content)

    /// Adds a connection from the first node ID to the second in the given
    /// direction. Also adds a second connection from the second node ID to
    /// the first as the opposite direction of the given one, so that if the
    /// given direction is north then the second connection will be generated
    /// in the south.
    let connect fromNodeId toNodeId direction graph =
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

    /// Adds many connections to the given graph.
    let connectMany connections graph =
        connections
        |> List.fold
            (fun g (fromNodeId, toNodeId, direction) ->
                connect fromNodeId toNodeId direction g)
            graph

[<RequireQualifiedAccess>]
module Node =
    /// Creates a new node with an auto-generated ID and the given content.
    let create id (content: 'a) = { Id = id; Content = content }

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
