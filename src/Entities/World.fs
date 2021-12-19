module Entities.World

open Aether

/// Creates an empty world.
let empty = { Cities = [] }

[<RequireQualifiedAccess>]
module City =
    /// Creates a city with only one initial starting node.
    let create (startingNodeId, startingNodeContent) =
        { Id = Identity.create ()
          StartingNode = startingNodeId
          Nodes =
              [ (startingNodeId, startingNodeContent) ]
              |> Map.ofList
          Connections = Map.empty }

    /// Adds a new node with no connections to the given city.
    let addNode (nodeId, nodeContent) city =
        { city with
              Nodes = city.Nodes |> Map.add nodeId nodeContent }

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
