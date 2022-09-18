module Entities.World

open Aether
open Entities

/// Creates an empty world.
let empty = { Cities = Map.empty }

/// Creates a new world with the given cities inside of it.
let create (cities: City list) =
    { Cities =
        cities
        |> List.map (fun c -> c.Id, c)
        |> Map.ofList }

[<RequireQualifiedAccess>]
module Graph =
    /// Creates a graph with the given starting node as the starting point, that
    /// node as the only node available and no connections.
    let from (startingNode: Node<'a>) =
        { StartingNode = startingNode.Id
          Nodes =
            [ (startingNode.Id, startingNode.Content) ]
            |> Map.ofList
          Connections = Map.empty }

    /// Adds a new node with no connections to the given graph.
    let addNode<'a> (node: Node<'a>) =
        Optic.map Lenses.World.Graph.nodes_ (Map.add node.Id node.Content)

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
module Node =
    /// Creates a new node with an auto-generated ID and the given content.
    let create id (content: 'a) = { Id = id; Content = content }

[<RequireQualifiedAccess>]
module Place =
    let private addToIndex (node: Node<RoomType>) index =
        let mapKey =
            match node.Content with
            | RoomType.Backstage -> RoomTypeIndex.Backstage
            | RoomType.Bar _ -> RoomTypeIndex.Bar
            | RoomType.Bedroom -> RoomTypeIndex.Bedroom
            | RoomType.Kitchen -> RoomTypeIndex.Kitchen
            | RoomType.LivingRoom -> RoomTypeIndex.LivingRoom
            | RoomType.Lobby -> RoomTypeIndex.Lobby
            | RoomType.MasteringRoom -> RoomTypeIndex.MasteringRoom
            | RoomType.RecordingRoom -> RoomTypeIndex.RecordingRoom
            | RoomType.RehearsalRoom -> RoomTypeIndex.RehearsalRoom
            | RoomType.Stage -> RoomTypeIndex.Stage

        Map.change
            mapKey
            (fun list ->
                match list with
                | Some list -> list @ [ node.Id ] |> Some
                | None -> [ node.Id ] |> Some)
            index

    /// Creates a place with the given initial room and no exits.
    let create name quality spaceType startingNode =
        { Name = name
          Quality = quality
          SpaceType = spaceType
          Rooms = Graph.from startingNode
          RoomIndex = addToIndex startingNode Map.empty
          Exits = Map.empty }

    /// Adds a room to the place.
    let addRoom room place =
        Optic.map Lenses.World.Place.rooms_ (Graph.addNode room) place
        |> Optic.map Lenses.World.Place.roomIndex_ (addToIndex room)

    /// Adds a connection between two room nodes in the specified direction.
    let addConnection
        (fromRoom: Node<'r>)
        (toRoom: Node<'r>)
        direction
        (place: Place)
        =
        Optic.map
            Lenses.World.Place.rooms_
            (Graph.addConnection fromRoom.Id toRoom.Id direction)
            place

    /// Adds an exit from the given room node to the specified city node.
    let addExit (roomNode: Node<'r>) (cityNode: Node<CityNode>) (place: Place) =
        Optic.map
            Lenses.World.Place.exits
            (Map.add roomNode.Id cityNode.Id)
            place

[<RequireQualifiedAccess>]
module City =
    let private addToIndex (node: Node<CityNode>) index =
        match node.Content with
        | CityNode.Place place ->
            let mapKey =
                match place.SpaceType with
                | ConcertSpace _ -> SpaceTypeIndex.ConcertSpace
                | Home -> SpaceTypeIndex.Home
                | Hospital -> SpaceTypeIndex.Hospital
                | RehearsalSpace _ -> SpaceTypeIndex.RehearsalSpace
                | Studio _ -> SpaceTypeIndex.Studio

            Map.change
                mapKey
                (fun list ->
                    match list with
                    | Some list -> list @ [ node.Id ] |> Some
                    | None -> [ node.Id ] |> Some)
                index
        | _ -> index

    /// Creates a city with only one initial starting node.
    let create id name (startingNode: Node<CityNode>) =
        { Id = id
          Name = name
          Graph = Graph.from startingNode
          Index = addToIndex startingNode Map.empty }

    /// Adds a node to the city, which adds it to the graph and to the index by
    /// its type.
    let addNode (node: Node<CityNode>) city =
        Optic.map Lenses.World.City.graph_ (Graph.addNode node) city
        |> Optic.map Lenses.World.City.index_ (addToIndex node)

    /// Adds a connection between nodes of the city.
    let addConnection fromNodeId toNodeId direction =
        Optic.map
            Lenses.World.City.graph_
            (Graph.addConnection fromNodeId toNodeId direction)
