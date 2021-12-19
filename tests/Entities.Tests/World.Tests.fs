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

    let city =
        World.City.create "Test City" { Id = nodeId; Content = nodeContent }

    city.Nodes |> should haveCount 1

    city.Nodes
    |> Map.find nodeId
    |> should equal nodeContent

    city.Connections |> should haveCount 0

let cityWithStreet =
    let nodeId = Identity.create ()
    let nodeContent = Street { Name = "Test street" }

    World.City.create "Test City" { Id = nodeId; Content = nodeContent }

[<Test>]
let ``addNode adds a new node to the list of nodes and no connections`` () =
    let nodeId = Identity.create ()
    let nodeContent = Street { Name = "Second Test Street" }

    let city =
        World.City.addNode { Id = nodeId; Content = nodeContent } cityWithStreet

    city.Nodes |> should haveCount 2

    city.Nodes
    |> Map.find nodeId
    |> should equal nodeContent

    city.Connections |> should haveCount 0

let firstNode =
    World.Node.create (Street { Name = "Test street" })

let secondNode =
    World.Node.create (Street { Name = "Test street 2" })

let cityWithMultipleNodes =
    World.City.create "Test city" firstNode
    |> World.City.addNode secondNode

[<Test>]
let ``addConnection adds a connection between the two nodes in the given direction and the opposite``
    ()
    =
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
                |> World.City.addConnection firstNode.Id secondNode.Id input

            city.Connections |> should haveCount 2

            Map.find firstNode.Id city.Connections
            |> Map.find input
            |> should equal secondNode.Id

            Map.find secondNode.Id city.Connections
            |> Map.find opposite
            |> should equal firstNode.Id)

[<Test>]
let ``addConnection supports adding multiple directions per node`` () =
    let city =
        cityWithMultipleNodes
        |> World.City.addConnection firstNode.Id secondNode.Id North
        |> World.City.addConnection firstNode.Id secondNode.Id South
        |> World.City.addConnection secondNode.Id firstNode.Id NorthEast

    city.Connections |> should haveCount 2

    let firstNodeConnections = Map.find firstNode.Id city.Connections
    let secondNodeConnections = Map.find secondNode.Id city.Connections

    firstNodeConnections |> should haveCount 3
    secondNodeConnections |> should haveCount 3

    [ North; South; SouthWest ]
    |> List.iter
        (fun input ->
            firstNodeConnections
            |> Map.find input
            |> should equal secondNode.Id)

    [ South; North; NorthEast ]
    |> List.iter
        (fun input ->
            secondNodeConnections
            |> Map.find input
            |> should equal firstNode.Id)
