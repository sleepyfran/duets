module Duets.Entities.World

open Aether
open Duets.Common
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

    /// Returns all the nodes in the graph.
    let nodes graph = graph.Nodes |> List.ofMapValues

    /// Changes the node with the given ID to the given node.
    let changeNode nodeId node graph =
        { graph with
            Nodes = Map.change nodeId node graph.Nodes }

[<RequireQualifiedAccess>]
module Node =
    /// Creates a new node with an auto-generated ID and the given content.
    let create id (content: 'a) = { Id = id; Content = content }

[<RequireQualifiedAccess>]
module Place =
    /// Creates a place with the given initial room and no exits.
    let create name quality placeType rooms zone =
        let inferredId = Identity.Reproducible.createFor name

        { Id = inferredId
          Name = name
          Quality = quality
          PlaceType = placeType
          OpeningHours = PlaceOpeningHours.AlwaysOpen
          Rooms = rooms
          Zone = zone }

    /// Changes the opening hours to a certain days and day moments.
    let changeOpeningHours openingHours place =
        { place with
            OpeningHours = openingHours }

    /// Changes a specific room in the place.
    let changeRoom roomId mapping place =
        { place with
            Rooms = Graph.changeNode roomId mapping place.Rooms }

    module Type =
        /// Transforms a PlaceType into its index counterpart.
        let toIndex placeType =
            match placeType with
            | Airport -> PlaceTypeIndex.Airport
            | Bar -> PlaceTypeIndex.Bar
            | Bookstore -> PlaceTypeIndex.Bookstore
            | Cafe -> PlaceTypeIndex.Cafe
            | Casino -> PlaceTypeIndex.Casino
            | ConcertSpace _ -> PlaceTypeIndex.ConcertSpace
            | Gym -> PlaceTypeIndex.Gym
            | Home -> PlaceTypeIndex.Home
            | Hotel _ -> PlaceTypeIndex.Hotel
            | Hospital -> PlaceTypeIndex.Hospital
            | MerchandiseWorkshop -> PlaceTypeIndex.MerchandiseWorkshop
            | RadioStudio _ -> PlaceTypeIndex.RadioStudio
            | RehearsalSpace _ -> PlaceTypeIndex.RehearsalSpace
            | Restaurant -> PlaceTypeIndex.Restaurant
            | Studio _ -> PlaceTypeIndex.Studio

[<RequireQualifiedAccess>]
module Zone =
    /// Creates a zone with the given name and an ID based on it.
    let create name =
        let inferredId = Identity.Reproducible.createFor name

        { Id = inferredId.ToString()
          Name = name }

[<RequireQualifiedAccess>]
module City =
    let private addToPlaceByTypeIndex place index =
        let mapKey = Place.Type.toIndex place.PlaceType

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
    let create id costOfLiving utcOffset place =
        { Id = id
          PlaceByTypeIndex = addToPlaceByTypeIndex place Map.empty
          PlaceIndex = [ (place.Id, place) ] |> Map.ofList
          CostOfLiving = costOfLiving
          ZoneIndex = addToZoneIndex place Map.empty
          Timezone = Utc(utcOffset) }

    /// Changes the timezone of the city.
    let changeTimezone timezone city = { city with Timezone = timezone }

    /// Adds a new place to the city.
    let addPlace place city =
        Optic.map
            Lenses.World.City.placeByTypeIndex_
            (addToPlaceByTypeIndex place)
            city
        |> Optic.map Lenses.World.City.placeIndex_ (Map.add place.Id place)
        |> Optic.map Lenses.World.City.zoneIndex_ (addToZoneIndex place)

    // Adds a new place to the city.
    let addPlace' city place = addPlace place city

[<RequireQualifiedAccess>]
module Room =
    /// Creates a room with the given type and no required items for entrance.
    let create roomType =
        { RoomType = roomType
          RequiredItemsForEntrance = None }

    /// Adds a required item for entrance to the given room.
    let changeRequiredItemForEntrance requiredItems room =
        { room with
            RequiredItemsForEntrance = Some requiredItems }

    /// Removes the required item for entrance from the given room.
    let removeRequiredItemForEntrance room =
        { room with
            RequiredItemsForEntrance = None }

[<RequireQualifiedAccess>]
module Coordinates =
    /// Returns the PlaceCoordinates of the given room coordinates.
    let toPlaceCoordinates (coords: RoomCoordinates) : PlaceCoordinates =
        let cityId, placeId, _ = coords
        cityId, placeId
