module Entities.Tests.World

open FsUnit
open NUnit.Framework

open Common
open Entities

let createOutsideNode name =
    CityNode.OutsideNode
        { Name = name
          Descriptors = [ Boring ]
          Type = OutsideNodeType.Street }

[<Test>]
let ``from returns a graph with the given node in the list of nodes and no connections``
    ()
    =
    let nodeId = Identity.create ()

    let nodeContent =
        createOutsideNode "Test street"

    let graph =
        World.Graph.from { Id = nodeId; Content = nodeContent }

    graph.Nodes |> should haveCount 1

    graph.Nodes
    |> Map.find nodeId
    |> should equal nodeContent

    graph.Connections |> should haveCount 0

let cityWithStreet =
    let nodeId = Identity.create ()

    let nodeContent =
        createOutsideNode "Test street"

    World.Graph.from { Id = nodeId; Content = nodeContent }

[<Test>]
let ``addNode adds a new node to the list of nodes and no connections`` () =
    let nodeId = Identity.create ()

    let nodeContent =
        createOutsideNode "Second test street"

    let graph =
        World.Graph.addNode
            { Id = nodeId; Content = nodeContent }
            cityWithStreet

    graph.Nodes |> should haveCount 2

    graph.Nodes
    |> Map.find nodeId
    |> should equal nodeContent

    graph.Connections |> should haveCount 0

let firstNode =
    World.Node.create (Identity.create ()) (createOutsideNode "Test street")

let secondNode =
    World.Node.create (Identity.create ()) (createOutsideNode "Test street 2")

let cityWithMultipleNodes =
    World.Graph.from firstNode
    |> World.Graph.addNode secondNode

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
    |> List.iter (fun (input, opposite) ->
        let graph =
            cityWithMultipleNodes
            |> World.Graph.addConnection firstNode.Id secondNode.Id input

        graph.Connections |> should haveCount 2

        Map.find firstNode.Id graph.Connections
        |> Map.find input
        |> should equal secondNode.Id

        Map.find secondNode.Id graph.Connections
        |> Map.find opposite
        |> should equal firstNode.Id)

[<Test>]
let ``addConnection supports adding multiple directions per node`` () =
    let graph =
        cityWithMultipleNodes
        |> World.Graph.addConnection firstNode.Id secondNode.Id North
        |> World.Graph.addConnection firstNode.Id secondNode.Id South
        |> World.Graph.addConnection secondNode.Id firstNode.Id NorthEast

    graph.Connections |> should haveCount 2

    let firstNodeConnections =
        Map.find firstNode.Id graph.Connections

    let secondNodeConnections =
        Map.find secondNode.Id graph.Connections

    firstNodeConnections |> should haveCount 3
    secondNodeConnections |> should haveCount 3

    [ North; South; SouthWest ]
    |> List.iter (fun input ->
        firstNodeConnections
        |> Map.find input
        |> should equal secondNode.Id)

    [ South; North; NorthEast ]
    |> List.iter (fun input ->
        secondNodeConnections
        |> Map.find input
        |> should equal firstNode.Id)
