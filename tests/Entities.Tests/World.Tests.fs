module Entities.Tests.World

open FsUnit
open NUnit.Framework

open Common
open Entities

[<Test>]
let ``create returns a city with the given node in the list of nodes and no connections``
    ()
    =
    let nodeId = Identity.create ()
    let nodeContent = Street { Name = "Test street" }

    let city = World.City.create (nodeId, nodeContent)
    city.Nodes |> should haveCount 1

    city.Nodes
    |> Map.find nodeId
    |> should equal nodeContent

    city.Connections |> should haveCount 0

let cityWithStreet =
    let nodeId = Identity.create ()
    let nodeContent = Street { Name = "Test street" }

    World.City.create (nodeId, nodeContent)

[<Test>]
let ``addNode adds a new node to the list of nodes and no connections`` () =
    let nodeId = Identity.create ()
    let nodeContent = Street { Name = "Second Test Street" }

    let city =
        World.City.addNode (nodeId, nodeContent) cityWithStreet

    city.Nodes |> should haveCount 2

    city.Nodes
    |> Map.find nodeId
    |> should equal nodeContent

    city.Connections |> should haveCount 0

let firstNode =
    (Identity.create (), Street { Name = "Test street" })

let secondNode =
    (Identity.create (), Street { Name = "Second test street" })

let cityWithMultipleNodes =
    World.City.create firstNode
    |> World.City.addNode secondNode

[<Test>]
let ``addConnection adds a connection between the two nodes in the given direction and the opposite``
    ()
    =
    let (firstNodeId, _) = firstNode
    let (secondNodeId, _) = secondNode

    [ (North, South)
      (NorthEast, SouthWest)
      (East, West)
      (SouthEast, NorthWest)
      (South, North)
      (SouthWest, NorthEast)
      (West, East)
      (NorthWest, SouthEast) ]
    |> List.iter
        (fun (input, opposite) ->
            let city =
                cityWithMultipleNodes
                |> World.City.addConnection firstNodeId secondNodeId input

            city.Connections |> should haveCount 2

            Map.find firstNodeId city.Connections
            |> Map.find input
            |> should equal secondNodeId

            Map.find secondNodeId city.Connections
            |> Map.find opposite
            |> should equal firstNodeId)

[<Test>]
let ``addConnection supports adding multiple directions per node`` () =
    let (firstNodeId, _) = firstNode
    let (secondNodeId, _) = secondNode

    let city =
        cityWithMultipleNodes
        |> World.City.addConnection firstNodeId secondNodeId North
        |> World.City.addConnection firstNodeId secondNodeId South
        |> World.City.addConnection secondNodeId firstNodeId NorthEast

    city.Connections |> should haveCount 2

    let firstNodeConnections = Map.find firstNodeId city.Connections
    let secondNodeConnections = Map.find secondNodeId city.Connections

    firstNodeConnections |> should haveCount 3
    secondNodeConnections |> should haveCount 3

    [ North; South; SouthWest ]
    |> List.iter
        (fun input ->
            firstNodeConnections
            |> Map.find input
            |> should equal secondNodeId)

    [ South; North; NorthEast ]
    |> List.iter
        (fun input ->
            secondNodeConnections
            |> Map.find input
            |> should equal firstNodeId)
