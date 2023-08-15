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

/// Usual gym hours around the world.
let gymOpeningHours =
    PlaceOpeningHours.OpeningHours(
        Calendar.everyDay,
        [ EarlyMorning; Morning; Midday; Afternoon; Evening; Night ]
    )

/// Usual layout for a restaurant.
let restaurantRoomLayout menu =
    RoomType.Restaurant menu
    |> World.Room.create
    |> World.Node.create 0
    |> World.Graph.from

/// Usual layout for a bar.
let barRoomLayout =
    RoomType.Bar |> World.Room.create |> World.Node.create 0 |> World.Graph.from

/// Usual layout for a cafe.
let cafeRoomLayout =
    RoomType.Cafe
    |> World.Room.create
    |> World.Node.create 0
    |> World.Graph.from

let gymLayout =
    let lobby =
        RoomType.Lobby |> World.Room.create |> World.Node.create Ids.Gym.lobby

    let changingRoom =
        RoomType.ChangingRoom
        |> World.Room.create
        |> World.Node.create Ids.Gym.changingRoom

    let gym = RoomType.Gym |> World.Room.create |> World.Node.create Ids.Gym.gym

    World.Graph.fromMany [ lobby; changingRoom; gym ]
    |> World.Graph.connectMany
        [ lobby.Id, changingRoom.Id, East; changingRoom.Id, gym.Id, North ]

/// Usual layout for a home.
let homeLayout =
    let kitchen =
        RoomType.Kitchen
        |> World.Room.create
        |> World.Node.create Ids.Home.kitchen

    let livingRoom =
        RoomType.LivingRoom
        |> World.Room.create
        |> World.Node.create Ids.Home.livingRoom

    let bedroom =
        RoomType.Bedroom
        |> World.Room.create
        |> World.Node.create Ids.Home.bedroom

    World.Graph.fromMany [ kitchen; livingRoom; bedroom ]
    |> World.Graph.connectMany
        [ kitchen.Id, livingRoom.Id, South; livingRoom.Id, bedroom.Id, South ]

/// Usual layout for a hotel.
let hotelLayout =
    let lobby = RoomType.Lobby |> World.Room.create |> World.Node.create 0
    let bar = RoomType.Bar |> World.Room.create |> World.Node.create 1
    let bedroom = RoomType.Bedroom |> World.Room.create |> World.Node.create 2

    World.Graph.fromMany [ lobby; bar; bedroom ]
    |> World.Graph.connectMany
        [ lobby.Id, bar.Id, East; lobby.Id, bedroom.Id, North ]

/// First option of a layout for a concert space.
let concertSpaceLayout1 =
    let lobby =
        RoomType.Lobby
        |> World.Room.create
        |> World.Node.create Ids.ConcertSpace.lobby

    let bar =
        RoomType.Bar
        |> World.Room.create
        |> World.Node.create Ids.ConcertSpace.bar

    let stage =
        RoomType.Stage
        |> World.Room.create
        |> World.Node.create Ids.ConcertSpace.stage

    let backstage =
        RoomType.Backstage
        |> World.Room.create
        |> World.Node.create Ids.ConcertSpace.backstage

    World.Graph.fromMany [ lobby; bar; stage; backstage ]
    |> World.Graph.connectMany
        [ lobby.Id, bar.Id, East
          lobby.Id, stage.Id, North
          stage.Id, backstage.Id, West ]

/// Second option of a layout for a concert space.
let concertSpaceLayout2 =
    let lobby =
        RoomType.Lobby
        |> World.Room.create
        |> World.Node.create Ids.ConcertSpace.lobby

    let bar =
        RoomType.Bar
        |> World.Room.create
        |> World.Node.create Ids.ConcertSpace.bar

    let stage =
        RoomType.Stage
        |> World.Room.create
        |> World.Node.create Ids.ConcertSpace.stage

    let backstage =
        RoomType.Backstage
        |> World.Room.create
        |> World.Node.create Ids.ConcertSpace.backstage

    World.Graph.fromMany [ lobby; bar; stage; backstage ]
    |> World.Graph.connectMany
        [ lobby.Id, bar.Id, West
          bar.Id, stage.Id, North
          stage.Id, backstage.Id, North ]

/// Third option of a layout for a concert space.
let concertSpaceLayout3 =
    let lobby =
        RoomType.Lobby
        |> World.Room.create
        |> World.Node.create Ids.ConcertSpace.lobby

    let bar =
        RoomType.Bar
        |> World.Room.create
        |> World.Node.create Ids.ConcertSpace.bar

    let stage =
        RoomType.Stage
        |> World.Room.create
        |> World.Node.create Ids.ConcertSpace.stage

    let backstage =
        RoomType.Backstage
        |> World.Room.create
        |> World.Node.create Ids.ConcertSpace.backstage

    World.Graph.fromMany [ lobby; bar; stage; backstage ]
    |> World.Graph.connectMany
        [ lobby.Id, bar.Id, North
          bar.Id, stage.Id, West
          stage.Id, backstage.Id, West ]

/// Fourth option of a layout for a concert space.
let concertSpaceLayout4 =
    let lobby =
        RoomType.Lobby
        |> World.Room.create
        |> World.Node.create Ids.ConcertSpace.lobby

    let bar =
        RoomType.Bar
        |> World.Room.create
        |> World.Node.create Ids.ConcertSpace.bar

    let stage =
        RoomType.Stage
        |> World.Room.create
        |> World.Node.create Ids.ConcertSpace.stage

    let backstage =
        RoomType.Backstage
        |> World.Room.create
        |> World.Node.create Ids.ConcertSpace.backstage

    World.Graph.fromMany [ lobby; bar; stage; backstage ]
    |> World.Graph.connectMany
        [ lobby.Id, bar.Id, North
          bar.Id, stage.Id, East
          stage.Id, backstage.Id, East ]

/// Usual layout for an airport.
let airportLayout =
    let lobby =
        RoomType.Lobby
        |> World.Room.create
        |> World.Node.create Ids.Airport.lobby

    let securityControl =
        RoomType.SecurityControl
        |> World.Room.create
        |> World.Node.create Ids.Airport.securityControl

    World.Graph.fromMany [ lobby; securityControl ]
    |> World.Graph.connect lobby.Id securityControl.Id West

/// Usual layout for a rehearsal space.
let rehearsalSpaceLayout =
    let lobby = RoomType.Lobby |> World.Room.create |> World.Node.create 0
    let bar = RoomType.Bar |> World.Room.create |> World.Node.create 1

    let room1 =
        RoomType.RehearsalRoom |> World.Room.create |> World.Node.create 2

    let room2 =
        RoomType.RehearsalRoom |> World.Room.create |> World.Node.create 3

    let room3 =
        RoomType.RehearsalRoom |> World.Room.create |> World.Node.create 4

    World.Graph.fromMany [ lobby; bar; room1; room2; room3 ]
    |> World.Graph.connectMany
        [ lobby.Id, bar.Id, East
          lobby.Id, room1.Id, North
          lobby.Id, room2.Id, NorthWest
          lobby.Id, room3.Id, West ]

/// Usual layout for a studio.
let studioLayout =
    let masteringRoom =
        RoomType.MasteringRoom |> World.Room.create |> World.Node.create 0

    let recordingRoom =
        RoomType.RecordingRoom |> World.Room.create |> World.Node.create 1

    World.Graph.fromMany [ masteringRoom; recordingRoom ]
    |> World.Graph.connectMany [ masteringRoom.Id, recordingRoom.Id, North ]

/// Usual opening hours for service places like studios or rehearsal places.
let servicesOpeningHours =
    PlaceOpeningHours.OpeningHours(
        Calendar.everyDay,
        [ Morning; Midday; Afternoon; Evening ]
    )
