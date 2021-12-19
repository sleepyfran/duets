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

    /// Defines all types of rooms that are available inside a place.
    type Room = RehearsalSpaceRoom of RehearsalSpaceRoom

    /// Defines all types of places that are available inside a city.
    type Place =
        | RehearsalSpace of space: RehearsalSpace * initialRoom: NodeId
        | Studio of studio: Studio * initialRoom: NodeId

    /// Defines a street in the game, which communicates different places
    /// in the world.
    type Street = { Name: string }

    /// Defines a node in the game, which represents one space inside of the
    /// map that the player can be in.
    type NodeContent =
        | Room of Room
        | Place of Place
        | Street of Street

    /// Defines all connections that a node has in each of its directions.
    type NodeConnections = Map<Direction, NodeId>

    /// Defines the relationship between a node ID and its content. Not stored
    /// anywhere, only used to pass information into the functions to create
    /// worlds and nodes.
    type NodeWithContent = { Id: NodeId; Content: NodeContent }

    /// Defines a city in the world as a connection of nodes with one of them
    /// being the entrypoint. Nodes can be rooms, places or streets that
    /// connect with each other via a direction that the user will use to
    /// navigate the map.
    type City =
        { Id: NodeId
          Name: string
          StartingNode: NodeId
          Nodes: Map<NodeId, NodeContent>
          Connections: Map<NodeId, NodeConnections> }

    /// Defines the game world which contains all cities.
    type World = { Cities: City list }
