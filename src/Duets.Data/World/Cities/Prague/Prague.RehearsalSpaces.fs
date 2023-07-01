module Duets.Data.World.Cities.Prague.RehearsalSpaces

open Duets.Entities
open Duets.Data.World

let addCheapAndFast zone =
    let rehearsalSpace = { Price = 15m<dd> }

    let lobby = World.Node.create 0 RoomType.Lobby
    let room1 = World.Node.create 1 RoomType.RehearsalRoom

    let roomGraph =
        World.Graph.fromMany [ lobby; room1 ]
        |> World.Graph.connect lobby.Id room1.Id North

    World.Place.create
        ("e2352c71-f18d-4594-816e-e1780506aa33" |> Identity.from)
        "Cheap&Fast Rehearsal Space"
        20<quality>
        (RehearsalSpace rehearsalSpace)
        roomGraph
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.servicesOpeningHours
    |> World.City.addPlace

let addZkusebnyTovarna zone =
    let rehearsalSpace = { Price = 55m<dd> }

    let shop =
        { AvailableItems = CityCommonItems.commonBar
          PriceModifier = 1<multiplier> }

    let lobby = World.Node.create 0 RoomType.Lobby
    let bar = RoomType.Bar shop |> World.Node.create 1
    let room1 = World.Node.create 2 RoomType.RehearsalRoom
    let room2 = World.Node.create 3 RoomType.RehearsalRoom
    let room3 = World.Node.create 4 RoomType.RehearsalRoom

    let roomGraph =
        World.Graph.fromMany [ lobby; bar; room1; room2; room3 ]
        |> World.Graph.connectMany
            [ lobby.Id, bar.Id, East
              lobby.Id, room1.Id, North
              lobby.Id, room2.Id, NorthWest
              lobby.Id, room3.Id, West ]

    World.Place.create
        ("1aaa53e5-f949-40c8-a2e2-7a2db64c8e95" |> Identity.from)
        "Zkušebny Praha Továrna"
        78<quality>
        (RehearsalSpace rehearsalSpace)
        roomGraph
        zone
    |> World.Place.changeOpeningHours Everywhere.Common.servicesOpeningHours
    |> World.City.addPlace
