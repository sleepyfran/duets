module Simulation.World.Generation

open Aether
open Entities

/// Generates the game world. Right now only creates a hard-coded city with
/// a bunch of places interconnected, in the future this should procedurally
/// generate the world and all the cities in it.
let rec generate () =
    let mainStreet =
        Street { Name = "Calle de Atocha" }
        |> World.Node.create

    let city = World.City.create "Madrid" mainStreet

    city
    |> addBeginnersRehearsalRoom mainStreet
    |> fun (city: City) -> { Cities = [ (city.Id, city) ] |> Map.ofList }

and addBeginnersRehearsalRoom street city =
    let lobby =
        RehearsalSpaceRoom.Lobby |> World.Node.create

    let bar =
        RehearsalSpaceRoom.Bar |> World.Node.create

    let rehearsalRoom =
        RehearsalSpaceRoom.RehearsalRoom
        |> World.Node.create

    let roomGraph =
        World.Graph.from lobby
        |> World.Graph.addNode bar
        |> World.Graph.addNode rehearsalRoom
        |> World.Graph.addConnection lobby.Id bar.Id NorthEast
        |> World.Graph.addConnection lobby.Id rehearsalRoom.Id North

    let rehearsalSpace =
        RehearsalSpace(
            { Name = "Good ol' Rehearsal Space"
              Quality = 20<quality>
              Price = 150<dd> },
            roomGraph
        )
        |> Place
        |> World.Node.create

    city
    |> World.City.addNode rehearsalSpace
    |> World.City.addConnection street.Id rehearsalSpace.Id West
    |> Optic.set Lenses.World.City.startingNode_ rehearsalSpace.Id
