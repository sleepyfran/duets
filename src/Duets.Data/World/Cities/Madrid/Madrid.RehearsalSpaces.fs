module Duets.Data.World.Cities.Madrid.RehearsalSpaces

open Duets.Entities
open Duets.Data.World

let addJackOnTheRocks zone =
    let rehearsalSpace = { Price = 20m<dd> }

    let lobby = World.Node.create 0 RoomType.Lobby
    let room1 = World.Node.create 1 RoomType.RehearsalRoom
    let room2 = World.Node.create 2 RoomType.RehearsalRoom
    let room3 = World.Node.create 3 RoomType.RehearsalRoom

    let roomGraph =
        World.Graph.fromMany [ lobby; room1; room2; room3 ]
        |> World.Graph.connectMany
            [ lobby.Id, room1.Id, North
              lobby.Id, room2.Id, NorthWest
              lobby.Id, room3.Id, West ]

    World.Place.create
        ("479ec3de-10ef-41a5-a158-882ef031c125" |> Identity.from)
        "Jack on the Rocks"
        50<quality>
        (RehearsalSpace rehearsalSpace)
        roomGraph
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.servicesOpeningHours
    |> World.City.addPlace

let addPandorasVox zone =
    let rehearsalSpace = { Price = 65m<dd> }

    let shop =
        { AvailableItems = CityCommonItems.commonBar
          PriceModifier = 2<multiplier> }

    let lobby = World.Node.create 0 RoomType.Lobby
    let bar = RoomType.Bar shop |> World.Node.create 1
    let room1 = World.Node.create 2 RoomType.RehearsalRoom
    let room2 = World.Node.create 3 RoomType.RehearsalRoom
    let room3 = World.Node.create 4 RoomType.RehearsalRoom

    let roomGraph =
        World.Graph.fromMany [ lobby; bar; room1; room2; room3 ]
        |> World.Graph.connectMany
            [ lobby.Id, bar.Id, North
              lobby.Id, room1.Id, North
              lobby.Id, room2.Id, West
              lobby.Id, room3.Id, East ]

    World.Place.create
        ("85ebaab3-3e1c-4c3b-afb2-d6ba2944ab9c" |> Identity.from)
        "Pandora's Vox"
        80<quality>
        (RehearsalSpace rehearsalSpace)
        roomGraph
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.servicesOpeningHours
    |> World.City.addPlace
