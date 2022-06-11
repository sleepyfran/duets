namespace Entities

open Entities

[<AutoOpen>]
module WorldTypes =
    /// Defines all possible cardinal directions that can be used to traverse
    /// the world map.
    type Direction =
        | North
        | NorthEast
        | East
        | SouthEast
        | South
        | SouthWest
        | West
        | NorthWest

    /// Defines all the different objects that can appear in the game world.
    type ObjectType = Instrument of InstrumentType

    /// Unique ID of a node, which represents a space inside of the world.
    type NodeId = Identity

    /// Defines all possible errors why the entrance to a node might be denied.
    [<RequireQualifiedAccess>]
    type EntranceError =
        | CannotEnterStageOutsideConcert
        | CannotEnterBackstageOutsideConcert

    /// Defines all connections that a node has in each of its directions.
    type NodeConnections = Map<Direction, NodeId>

    /// Represents a graph that can be used to create connections between
    /// different nodes in each of the available directions. All connected
    /// nodes are considered undirected, meaning that if A and B are connected
    /// then the player can go from A to B and B to A. These connections are
    /// made in the opposite direction, so if A connects through the North to
    /// B, then B connects through the South to A.
    type Graph<'a> =
        { StartingNode: NodeId
          Nodes: Map<NodeId, 'a>
          Connections: Map<NodeId, NodeConnections> }

    /// Defines the relationship between a node ID and its content.
    type Node<'a> = { Id: NodeId; Content: 'a }

    // Defines all types of rooms that player can be in.
    [<RequireQualifiedAccess>]
    type Room =
        | Backstage
        | Bar
        | Bedroom
        | Kitchen
        | LivingRoom
        | Lobby
        | MasteringRoom
        | RecordingRoom
        | RehearsalRoom
        | Stage

    /// Defines all the different types of places that the game supports.
    type SpaceType =
        | ConcertSpace of ConcertSpace
        | Home
        | Hospital
        | RehearsalSpace of RehearsalSpace
        | Studio of Studio

    /// Defines a place inside of the game world, which wraps a given space
    /// (could be any inside space like a rehearsal place or a concert hall), the
    /// rooms that the place itself contains and the exits that connect that
    /// place with the outside.
    type Place =
        { Rooms: Graph<Room>
          Exits: Map<NodeId, NodeId>
          Name: string
          Quality: Quality
          SpaceType: SpaceType }

    /// Defines all the different terms that can be used to describe a street.
    type OutsideNodeDescriptor =
        | Beautiful
        | Boring
        | Central
        | Historical

    /// Defines all types of streets available in the game. This changes the
    /// way the street is described to the user.
    [<RequireQualifiedAccess>]
    type OutsideNodeType =
        | Street
        | Square
        | Boulevard

    /// Defines a street in the game, which communicates different places
    /// in the world.
    type OutsideNode =
        { Name: string
          Descriptors: OutsideNodeDescriptor list
          Type: OutsideNodeType }

    /// Defines a node in the game, which represents one space inside of the
    /// map that the player can be in.
    [<RequireQualifiedAccess>]
    type CityNode =
        | Place of Place
        | OutsideNode of OutsideNode

    /// Unique identifier of a city.
    type CityId = Identity

    type RoomCoordinates = NodeId * NodeId
    type OutsideCoordinates = NodeId

    /// Defines the coordinates to a specific point inside a city.
    type NodeCoordinates =
        | Room of RoomCoordinates
        | Node of OutsideCoordinates

    /// Defines all errors that can happen when trying to move to another node.
    type MovementError = | CanNotEnterStageOutsideAConcert

    /// Defines the coordinates to a specific point in the game world.
    type WorldCoordinates = CityId * NodeCoordinates

    /// Defines a city in the world as a connection of nodes with one of them
    /// being the entrypoint. Nodes can be rooms, places or streets that
    /// connect with each other via a direction that the user will use to
    /// navigate the map.
    type City =
        { Id: CityId
          Name: string
          Graph: Graph<CityNode> }

    /// Resolved coordinates for nodes that contain rooms, with the place and
    /// the room that the coordinates referred to.
    type ResolvedRoomCoordinates =
        { Coordinates: RoomCoordinates
          Place: Place
          Room: Room }

    /// Resolved coordinates for nodes that do not contain rooms.
    type ResolvedOutsideCoordinates =
        { Coordinates: OutsideCoordinates
          Node: OutsideNode }

    /// Resolved coordinates with all fields. Includes the city, the given
    /// coordinates, the content of the node and the content of the room, if any.
    type ResolvedCoordinates =
        | ResolvedPlaceCoordinates of ResolvedRoomCoordinates
        | ResolvedOutsideCoordinates of ResolvedOutsideCoordinates

    /// Contains the city and the resolved coordinates of a node and room.
    type ResolvedCityCoordinates =
        { City: City
          Content: ResolvedCoordinates }

    /// Defines the game world which contains all cities.
    type World = { Cities: Map<CityId, City> }
