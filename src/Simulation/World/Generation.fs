module Simulation.World.Generation

open Aether
open Entities

/// Generates the game world. Right now only creates a hard-coded city with
/// a bunch of places interconnected, in the future this should procedurally
/// generate the world and all the cities in it.
let rec generate () =
    let mainStreet =
        Street
            { Name = "Calle de Atocha"
              Descriptor = Boring }
        |> World.Node.create

    let city = World.City.create "Madrid" mainStreet

    city
    |> addBeginnersRehearsalRoom mainStreet
    |> addDuetsStudio mainStreet
    |> Optic.set Lenses.World.City.startingNode_ mainStreet.Id
    |> fun (city: City) -> { Cities = [ (city.Id, city) ] |> Map.ofList }

and addBeginnersRehearsalRoom street city =
    let rehearsalSpace =
        { Name = "Good ol' Rehearsal Space"
          Quality = 10<quality>
          Price = 150<dd> }

    let lobby =
        RehearsalSpaceRoom.Lobby rehearsalSpace
        |> RehearsalSpaceRoom
        |> Room
        |> World.Node.create

    let bar =
        RehearsalSpaceRoom.Bar rehearsalSpace
        |> RehearsalSpaceRoom
        |> Room
        |> World.Node.create

    let rehearsalRoom =
        RehearsalSpaceRoom.RehearsalRoom rehearsalSpace
        |> RehearsalSpaceRoom
        |> Room
        |> World.Node.create

    city
    |> World.City.addNode lobby
    |> World.City.addNode bar
    |> World.City.addNode rehearsalRoom
    |> World.City.addConnection lobby.Id bar.Id NorthEast
    |> World.City.addConnection lobby.Id rehearsalRoom.Id North
    |> World.City.addConnection street.Id lobby.Id West

and addDuetsStudio street city =
    let studio = List.head (Database.studios ())

    let masteringRoom =
        StudioRoom.MasteringRoom studio
        |> StudioRoom
        |> Room
        |> World.Node.create

    let recordingRoom =
        StudioRoom.RecordingRoom studio
        |> StudioRoom
        |> Room
        |> World.Node.create

    city
    |> World.City.addNode masteringRoom
    |> World.City.addNode recordingRoom
    |> World.City.addConnection masteringRoom.Id recordingRoom.Id North
    |> World.City.addConnection street.Id masteringRoom.Id East
