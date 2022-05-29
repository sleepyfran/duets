module Simulation.WorldGeneration.Cities.Prague

open Entities

let rec generate () =
    let prague =
        World.City.create
            (Identity.from "cea284b4-7714-45cc-a101-c1d69e347671")
            "Prague"

    let wenceslasSquare =
        CityNode.OutsideNode
            { Name = "Václavské náměstí"
              Descriptors = [ Beautiful; Central ]
              Type = OutsideNodeType.Boulevard }
        |> World.Node.create (
            Identity.from "1b6f9892-def3-4e6c-aee8-3414e8301a11"
        )

    let jzpSquare =
        CityNode.OutsideNode
            { Name = "Náměstí Jiřího z Poděbrad"
              Descriptors = [ Beautiful ]
              Type = OutsideNodeType.Square }
        |> World.Node.create (
            Identity.from "3efc7f12-a3d4-4a30-a3c5-00ce3b02c3a0"
        )

    let oldTownSquare =
        CityNode.OutsideNode
            { Name = "Staroměstské náměstí"
              Descriptors = [ Beautiful; Historical; Central ]
              Type = OutsideNodeType.Square }
        |> World.Node.create (
            Identity.from "d36c0522-665e-4552-bc2f-95031151ffa1"
        )

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

    let lobby =
        Room.Lobby
        |> World.Node.create (
            Identity.from "6b31ecb9-0df6-4b3c-b2af-77d75eb25d31"
        )

    let bar =
        Room.Bar
        |> World.Node.create (
            Identity.from "c082e6e2-42e9-4bbc-bea2-9690d54ad36d"
        )

    let rehearsalRoom =
        Room.RehearsalRoom
        |> World.Node.create (
            Identity.from "89537bd2-9902-47ab-bd72-a4f5b6056e6b"
        )

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
        |> World.Node.create (
            Identity.from "fec78931-1447-472c-b37f-d093235447c3"
        )

    city
    |> World.City.addNode node
    |> World.City.addConnection street.Id node.Id NorthWest

and addDuetsStudio street city =
    let studioName, quality, studio =
        List.head (Database.studios ())

    let masteringRoom =
        Room.MasteringRoom
        |> World.Node.create (
            Identity.from "d050db29-0e04-46dd-a917-c41f2225931d"
        )

    let recordingRoom =
        Room.RecordingRoom
        |> World.Node.create (
            Identity.from "69d7a19e-2a08-4cb1-9f7f-269d4b0d0d9f"
        )

    let node =
        World.Place.create studioName quality (Studio studio) masteringRoom
        |> World.Place.addRoom recordingRoom
        |> World.Place.addConnection masteringRoom recordingRoom North
        |> World.Place.addExit masteringRoom street
        |> CityNode.Place
        |> World.Node.create (
            Identity.from "0857ab74-186b-48dd-b6dc-e1922f49ea6a"
        )

    city
    |> World.City.addNode node
    |> World.City.addConnection street.Id node.Id East

and addPalacAkropolis street city =
    let concertSpace = { Capacity = 1000 }

    let lobby =
        Room.Lobby
        |> World.Node.create (
            Identity.from "6c9bf67e-76fa-4c3a-a693-42fcc3a39a39"
        )

    let bar =
        Room.Bar
        |> World.Node.create (
            Identity.from "30d888e5-70d1-4ee3-a661-4595276d7f98"
        )

    let stage =
        Room.Stage
        |> World.Node.create (
            Identity.from "8bca0085-48e0-407b-a7b1-3649dc33945e"
        )

    let backstage =
        Room.Backstage
        |> World.Node.create (
            Identity.from "a0d49337-9eb2-4090-b935-48b1267bed24"
        )

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
        |> World.Node.create (
            Identity.from "1a08f39b-714e-4e26-bc8c-f07744af1777"
        )

    city
    |> World.City.addNode node
    |> World.City.addConnection street.Id node.Id North

and addRedutaJazzClub street city =
    let concertSpace = { Capacity = 250 }

    let lobby =
        Room.Lobby
        |> World.Node.create (
            Identity.from "51f9de6d-c8f4-47ff-be27-203e9b3718fc"
        )

    let bar =
        Room.Bar
        |> World.Node.create (
            Identity.from "2bf83a32-ba46-4489-97d0-c8d6c05ec79e"
        )

    let stage =
        Room.Stage
        |> World.Node.create (
            Identity.from "7dfdc1d4-d453-44fc-933d-148c26e752bd"
        )

    let backstage =
        Room.Backstage
        |> World.Node.create (
            Identity.from "9ff3840f-d955-4221-b655-e661edd34dda"
        )

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
        |> World.Node.create (
            Identity.from "3076affc-80cb-426b-870b-48718adb1a9d"
        )

    city
    |> World.City.addNode node
    |> World.City.addConnection street.Id node.Id West
