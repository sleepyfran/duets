module Simulation.World.Generation

open Aether
open Entities

/// Generates the game world. Right now only creates a hard-coded city with
/// a bunch of places interconnected, in the future this should procedurally
/// generate the world and all the cities in it.
let rec generate () =
    let city = World.City.empty "Madrid"

    let mainStreet =
        Street { Name = "Calle de Atocha" }
        |> World.Node.create

    city
    |> Optic.set Lenses.World.City.startingNode_ mainStreet.Id
    |> addBeginnersRehearsalRoom mainStreet
    |> fun city -> { Cities = [ city ] }

and addBeginnersRehearsalRoom street city =
    let lobby =
        RehearsalSpaceRoom RehearsalSpaceRoom.Lobby
        |> Room
        |> World.Node.create

    let bar =
        RehearsalSpaceRoom RehearsalSpaceRoom.Bar
        |> Room
        |> World.Node.create

    let rehearsalRoom =
        RehearsalSpaceRoom RehearsalSpaceRoom.RehearsalRoom
        |> Room
        |> World.Node.create

    let rehearsalSpace =
        RehearsalSpace(
            { Name = "Good ol' Rehearsal Space"
              Quality = 20<quality>
              Price = 150<dd> },
            lobby.Id
        )
        |> Place
        |> World.Node.create

    city
    |> World.City.addNode lobby
    |> World.City.addNode bar
    |> World.City.addNode rehearsalRoom
    |> World.City.addNode rehearsalSpace
    |> World.City.addConnection lobby.Id bar.Id NorthEast
    |> World.City.addConnection lobby.Id rehearsalRoom.Id North
    |> World.City.addConnection lobby.Id street.Id East
