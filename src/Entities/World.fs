module Entities.World

open Aether
open Entities

/// Creates an empty world.
let empty = { Cities = [] }

[<RequireQualifiedAccess>]
module Node =
    /// Creates a new node with an auto-generated ID and the given content.
    let create (content: NodeContent) =
        { Id = Identity.create ()
          Content = content }

[<RequireQualifiedAccess>]
module City =
    /// Creates an empty INVALID city as a starting point. This should NEVER be
    /// used without properly initializing the nodes and starting point.
    let empty name =
        { Id = Identity.create ()
          Name = name
          StartingNode = Identity.create ()
          Nodes = Map.empty
          Connections = Map.empty }

    /// Creates a city with only one initial starting node.
    let create name (startingNode: NodeWithContent) =
        { Id = Identity.create ()
          Name = name
          StartingNode = startingNode.Id
          Nodes =
              [ (startingNode.Id, startingNode.Content) ]
              |> Map.ofList
          Connections = Map.empty }

    /// Adds a new node with no connections to the given city.
    let addNode (node: NodeWithContent) city =
        { city with
              Nodes = city.Nodes |> Map.add node.Id node.Content }

    /// Adds a connection from the first node ID to the second in the given
    /// direction. Also adds a second connection from the second node ID to
    /// the first as the opposite direction of the given one, so that if the
    /// given direction is north then the second connection will be generated
    /// in the south.
    let addConnection fromNodeId toNodeId direction city =
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

        city
        |> Optic.map
            (Lenses.World.City.nodeConnections_ fromNodeId)
            (Map.add direction toNodeId)
        |> Optic.map
            (Lenses.World.City.nodeConnections_ toNodeId)
            (Map.add oppositeDirection fromNodeId)
