module Simulation.World.Generation.Cities.Prague

open Entities

let rec generate () =
    let prague = World.City.create "Prague"

    let wenceslasSquare =
        CityNode.OutsideNode
            { Name = "Václavské náměstí"
              Descriptors = [ Beautiful; Central ]
              Type = OutsideNodeType.Boulevard }
        |> World.Node.create

    let jzpSquare =
        CityNode.OutsideNode
            { Name = "Náměstí Jiřího z Poděbrad"
              Descriptors = [ Beautiful ]
              Type = OutsideNodeType.Square }
        |> World.Node.create

    let oldTownSquare =
        CityNode.OutsideNode
            { Name = "Staroměstské náměstí"
              Descriptors = [ Beautiful; Historical; Central ]
              Type = OutsideNodeType.Square }
        |> World.Node.create

    prague wenceslasSquare
    |> World.City.addNode wenceslasSquare
    |> World.City.addNode jzpSquare
    |> World.City.addNode oldTownSquare
    |> World.City.addConnection wenceslasSquare.Id jzpSquare.Id East
    |> World.City.addConnection wenceslasSquare.Id oldTownSquare.Id West
    |> addDuetsRehearsalSpace jzpSquare
    |> addDuetsStudio jzpSquare
    |> addPalacAkropolis jzpSquare
    |> addRedutaJazzClub oldTownSquare

and addDuetsRehearsalSpace street city =
    let rehearsalSpace = { Price = 300<dd> }

    let lobby = Room.Lobby |> World.Node.create

    let bar = Room.Bar |> World.Node.create

    let rehearsalRoom = Room.RehearsalRoom |> World.Node.create

    let node =
        World.Place.create
            "Duets Rehearsal Place"
            20<quality>
            (RehearsalSpace rehearsalSpace)
            lobby
        |> World.Place.addRoom bar
        |> World.Place.addConnection lobby bar NorthEast
        |> World.Place.addRoom rehearsalRoom
        |> World.Place.addConnection lobby rehearsalRoom North
        |> World.Place.addExit lobby street
        |> CityNode.Place
        |> World.Node.create

    city
    |> World.City.addNode node
    |> World.City.addConnection street.Id node.Id NorthWest

and addDuetsStudio street city =
    let (studioName, quality, studio) = List.head (Database.studios ())

    let masteringRoom = Room.MasteringRoom |> World.Node.create

    let recordingRoom = Room.RecordingRoom |> World.Node.create

    let node =
        World.Place.create studioName quality (Studio studio) masteringRoom
        |> World.Place.addRoom recordingRoom
        |> World.Place.addConnection masteringRoom recordingRoom North
        |> World.Place.addExit masteringRoom street
        |> CityNode.Place
        |> World.Node.create

    city
    |> World.City.addNode node
    |> World.City.addConnection street.Id node.Id East

and addPalacAkropolis street city =
    let concertSpace = { Capacity = 1000 }

    let lobby = Room.Lobby |> World.Node.create

    let bar = Room.Bar |> World.Node.create

    let stage = Room.Stage |> World.Node.create

    let backstage = Room.Backstage |> World.Node.create

    let node =
        World.Place.create
            "Palác Akropolis"
            75<quality>
            (ConcertSpace concertSpace)
            lobby
        |> World.Place.addRoom bar
        |> World.Place.addConnection lobby bar East
        |> World.Place.addRoom stage
        |> World.Place.addConnection lobby stage North
        |> World.Place.addRoom backstage
        |> World.Place.addConnection lobby backstage NorthEast
        |> World.Place.addConnection stage backstage East
        |> World.Place.addExit lobby street
        |> CityNode.Place
        |> World.Node.create

    city
    |> World.City.addNode node
    |> World.City.addConnection street.Id node.Id North

and addRedutaJazzClub street city =
    let concertSpace = { Capacity = 250 }

    let lobby = Room.Lobby |> World.Node.create

    let bar = Room.Bar |> World.Node.create

    let stage = Room.Stage |> World.Node.create

    let backstage = Room.Backstage |> World.Node.create

    let node =
        World.Place.create
            "Reduta Jazz Club"
            95<quality>
            (ConcertSpace concertSpace)
            lobby
        |> World.Place.addRoom bar
        |> World.Place.addConnection lobby bar West
        |> World.Place.addRoom stage
        |> World.Place.addConnection bar stage North
        |> World.Place.addRoom backstage
        |> World.Place.addConnection bar backstage NorthEast
        |> World.Place.addConnection stage backstage East
        |> World.Place.addExit lobby street
        |> CityNode.Place
        |> World.Node.create

    city
    |> World.City.addNode node
    |> World.City.addConnection street.Id node.Id West
