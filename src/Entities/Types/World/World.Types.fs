namespace Entities

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

    /// Defines all types of rooms that are available inside a place.
    type InsideNode =
        | ConcertSpaceRoom of ConcertSpaceRoom
        | RehearsalSpaceRoom of RehearsalSpaceRoom
        | StudioRoom of StudioRoom

    /// Defines all the different terms that can be used to describe a street.
    type OutsideNodeDescriptor =
        | Beautiful
        | Boring
        | Central
        | Historical

    /// Defines all types of streets available in the game. This changes the
    /// way the street is described to the user.
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
    type CityNode =
        | InsideNode of InsideNode
        | OutsideNode of OutsideNode

    /// Unique identifier of a city.
    type CityId = Identity

    /// Defines a city in the world as a connection of nodes with one of them
    /// being the entrypoint. Nodes can be rooms, places or streets that
    /// connect with each other via a direction that the user will use to
    /// navigate the map.
    type City =
        { Id: CityId
          Name: string
          Graph: Graph<CityNode> }

    /// Defines the game world which contains all cities.
    type World = { Cities: Map<CityId, City> }

    /// Defines the coordinates to a point of the world map as the city and
    /// the node in which the character is in.
    type Coordinates = CityId * NodeId
