module Entities.World

open Aether
open Entities

/// Creates an empty world.
let empty = { Cities = Map.empty }

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
    let create (content: 'a) =
        { Id = Identity.create ()
          Content = content }

[<RequireQualifiedAccess>]
module City =
    /// Creates a city with only one initial starting node.
    let create name (startingNode: Node<CityNode>) =
        { Id = Identity.create ()
          Name = name
          Graph = Graph.from startingNode }

    /// Adds a node to the city.
    let addNode (node: Node<CityNode>) =
        Optic.map Lenses.World.City.graph_ (Graph.addNode node)

    /// Adds a connection between nodes of the city.
    let addConnection fromNodeId toNodeId direction =
        Optic.map
            Lenses.World.City.graph_
            (Graph.addConnection fromNodeId toNodeId direction)
