module Duets.Data.World.Everywhere.Common

open Duets.Entities
open Duets.Data.World

/// Usual bar opening hours around the world.
let barOpeningHours =
    PlaceOpeningHours.OpeningHours(
        Calendar.everyDay,
        [ Midday; Afternoon; Evening; Night; Midnight ]
    )

/// Usual cafe opening hours around the world.
let cafeOpeningHours =
    PlaceOpeningHours.OpeningHours(
        Calendar.everyDay,
        [ EarlyMorning; Morning; Midday; Afternoon; Evening ]
    )

/// Usual concert space hours around the world.
let concertSpaceOpeningHours =
    PlaceOpeningHours.OpeningHours(
        Calendar.everyDay,
        [ Afternoon; Evening; Night ]
    )

/// Usual restaurant hours around the world.
let restaurantOpeningHours =
    PlaceOpeningHours.OpeningHours(
        Calendar.everyDay,
        [ Midday; Afternoon; Evening; Night ]
    )

/// Usual layout for a restaurant.
let restaurantRoomLayout menu =
    RoomType.Restaurant menu |> World.Node.create 0 |> World.Graph.from

/// Usual layout for a bar.
let barRoomLayout = RoomType.Bar |> World.Node.create 0 |> World.Graph.from

/// Usual layout for a cafe.
let cafeRoomLayout = RoomType.Cafe |> World.Node.create 0 |> World.Graph.from

/// Usual layout for a home.
let homeLayout =
    let kitchen = World.Node.create Ids.Home.kitchen RoomType.Kitchen
    let livingRoom = World.Node.create Ids.Home.livingRoom RoomType.LivingRoom
    let bedroom = World.Node.create Ids.Home.bedroom RoomType.Bedroom

    World.Graph.fromMany [ kitchen; livingRoom; bedroom ]
    |> World.Graph.connectMany
        [ kitchen.Id, livingRoom.Id, South; livingRoom.Id, bedroom.Id, South ]

/// Usual layout for a hotel.
let hotelLayout =
    let lobby = World.Node.create 0 RoomType.Lobby
    let bar = World.Node.create 1 RoomType.Bar
    let bedroom = World.Node.create 2 RoomType.Bedroom

    World.Graph.fromMany [ lobby; bar; bedroom ]
    |> World.Graph.connectMany
        [ lobby.Id, bar.Id, East; lobby.Id, bedroom.Id, North ]

/// First option of a layout for a concert space.
let concertSpaceLayout1 =
    let lobby = World.Node.create Ids.ConcertSpace.lobby RoomType.Lobby
    let bar = RoomType.Bar |> World.Node.create Ids.ConcertSpace.bar
    let stage = World.Node.create Ids.ConcertSpace.stage RoomType.Stage

    let backstage =
        World.Node.create Ids.ConcertSpace.backstage RoomType.Backstage

    World.Graph.fromMany [ lobby; bar; stage; backstage ]
    |> World.Graph.connectMany
        [ lobby.Id, bar.Id, East
          lobby.Id, stage.Id, North
          stage.Id, backstage.Id, West ]

/// Second option of a layout for a concert space.
let concertSpaceLayout2 =
    let lobby = World.Node.create 0 RoomType.Lobby
    let bar = RoomType.Bar |> World.Node.create Ids.ConcertSpace.bar
    let stage = World.Node.create Ids.ConcertSpace.stage RoomType.Stage

    let backstage =
        World.Node.create Ids.ConcertSpace.backstage RoomType.Backstage

    World.Graph.fromMany [ lobby; bar; stage; backstage ]
    |> World.Graph.connectMany
        [ lobby.Id, bar.Id, West
          bar.Id, stage.Id, North
          stage.Id, backstage.Id, North ]

/// Third option of a layout for a concert space.
let concertSpaceLayout3 =
    let lobby = World.Node.create 0 RoomType.Lobby
    let bar = RoomType.Bar |> World.Node.create Ids.ConcertSpace.bar
    let stage = World.Node.create Ids.ConcertSpace.stage RoomType.Stage

    let backstage =
        World.Node.create Ids.ConcertSpace.backstage RoomType.Backstage

    World.Graph.fromMany [ lobby; bar; stage; backstage ]
    |> World.Graph.connectMany
        [ lobby.Id, bar.Id, North
          bar.Id, stage.Id, West
          stage.Id, backstage.Id, West ]

/// Fourth option of a layout for a concert space.
let concertSpaceLayout4 =
    let lobby = World.Node.create 0 RoomType.Lobby
    let bar = RoomType.Bar |> World.Node.create Ids.ConcertSpace.bar
    let stage = World.Node.create Ids.ConcertSpace.stage RoomType.Stage

    let backstage =
        World.Node.create Ids.ConcertSpace.backstage RoomType.Backstage

    World.Graph.fromMany [ lobby; bar; stage; backstage ]
    |> World.Graph.connectMany
        [ lobby.Id, bar.Id, North
          bar.Id, stage.Id, East
          stage.Id, backstage.Id, East ]

/// Usual layout for an airport.
let airportLayout =
    let lobby = World.Node.create Ids.Airport.lobby RoomType.Lobby

    let securityControl =
        World.Node.create Ids.Airport.securityControl RoomType.SecurityControl

    World.Graph.fromMany [ lobby; securityControl ]
    |> World.Graph.connect lobby.Id securityControl.Id West

/// Usual layout for a rehearsal space.
let rehearsalSpaceLayout =
    let lobby = World.Node.create 0 RoomType.Lobby
    let bar = RoomType.Bar |> World.Node.create 1
    let room1 = World.Node.create 2 RoomType.RehearsalRoom
    let room2 = World.Node.create 3 RoomType.RehearsalRoom
    let room3 = World.Node.create 4 RoomType.RehearsalRoom

    World.Graph.fromMany [ lobby; bar; room1; room2; room3 ]
    |> World.Graph.connectMany
        [ lobby.Id, bar.Id, East
          lobby.Id, room1.Id, North
          lobby.Id, room2.Id, NorthWest
          lobby.Id, room3.Id, West ]

/// Usual layout for a studio.
let studioLayout =
    let masteringRoom = World.Node.create 0 RoomType.MasteringRoom
    let recordingRoom = World.Node.create 1 RoomType.RecordingRoom

    World.Graph.fromMany [ masteringRoom; recordingRoom ]
    |> World.Graph.connectMany [ masteringRoom.Id, recordingRoom.Id, North ]

/// Usual opening hours for service places like studios or rehearsal places.
let servicesOpeningHours =
    PlaceOpeningHours.OpeningHours(
        Calendar.everyDay,
        [ Morning; Midday; Afternoon; Evening ]
    )
