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

    /// Defines a rehearsal space in which the band can go to practice their
    /// songs and compose new ones.
    type RehearsalSpace =
        { Name: string
          Quality: Quality
          Price: Amount }

    /// Defines all types of rooms that are available in the game.
    type Room = RehearsalSpace

    /// Defines a street in the game.
    type Street = { Name: string }

    /// Defines a node in the game, which represents one space inside of the
    /// map that the player can be in.
    type NodeContent =
        | Room of Room
        | Street of Street

    /// Unique ID of a node inside of the world.
    type NodeId = Identity

    /// Defines all connections that a node has around it.
    type NodeConnections = Map<Direction, NodeId>

    /// Defines a city in the world as a connection of nodes with one of them
    /// being the entrypoint.
    type City =
        { Id: NodeId
          StartingNode: NodeId
          Nodes: Map<NodeId, NodeContent>
          Connections: Map<NodeId, NodeConnections> }

    /// Defines the game world which contains all cities.
    type World = { Cities: City list }
