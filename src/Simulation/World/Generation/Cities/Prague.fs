module Simulation.World.Generation.Cities.Prague

open Entities

let rec generate () =
    let prague = World.City.create "Prague"

    let wenceslasSquare =
        OutsideNode
            { Name = "Václavské náměstí"
              Descriptors = [ Beautiful ]
              Type = Boulevard }
        |> World.Node.create

    let jzpSquare =
        OutsideNode
            { Name = "Náměstí Jiřího z Poděbrad"
              Descriptors = [ Beautiful ]
              Type = Square }
        |> World.Node.create

    // TODO: Add Reduta Jazz Club

    prague wenceslasSquare
    |> World.City.addNode wenceslasSquare
    |> World.City.addNode jzpSquare
    |> World.City.addConnection wenceslasSquare.Id jzpSquare.Id South
    |> addDuetsRehearsalSpace jzpSquare
    |> addDuetsStudio jzpSquare
    |> addPalacAkropolis jzpSquare

and addDuetsRehearsalSpace street city =
    let rehearsalSpace =
        { Name = "Duets Rehearsal Space"
          Quality = 20<quality>
          Price = 300<dd> }

    let lobby =
        RehearsalSpaceRoom.Lobby rehearsalSpace
        |> RehearsalSpaceRoom
        |> InsideNode
        |> World.Node.create

    let bar =
        RehearsalSpaceRoom.Bar rehearsalSpace
        |> RehearsalSpaceRoom
        |> InsideNode
        |> World.Node.create

    let rehearsalRoom =
        RehearsalSpaceRoom.RehearsalRoom rehearsalSpace
        |> RehearsalSpaceRoom
        |> InsideNode
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
        |> InsideNode
        |> World.Node.create

    let recordingRoom =
        StudioRoom.RecordingRoom studio
        |> StudioRoom
        |> InsideNode
        |> World.Node.create

    city
    |> World.City.addNode masteringRoom
    |> World.City.addNode recordingRoom
    |> World.City.addConnection masteringRoom.Id recordingRoom.Id North
    |> World.City.addConnection street.Id masteringRoom.Id East

and addPalacAkropolis street city =
    let concertSpace =
        { Name = "Palác Akropolis"
          Quality = 75<quality>
          Capacity = 1000 }

    let lobby =
        ConcertSpaceRoom.Lobby concertSpace
        |> ConcertSpaceRoom
        |> InsideNode
        |> World.Node.create

    let bar =
        ConcertSpaceRoom.Bar concertSpace
        |> ConcertSpaceRoom
        |> InsideNode
        |> World.Node.create

    let stage =
        ConcertSpaceRoom.Stage concertSpace
        |> ConcertSpaceRoom
        |> InsideNode
        |> World.Node.create

    city
    |> World.City.addNode lobby
    |> World.City.addNode bar
    |> World.City.addNode stage
    |> World.City.addConnection street.Id lobby.Id North
    |> World.City.addConnection lobby.Id bar.Id East
    |> World.City.addConnection lobby.Id stage.Id North
