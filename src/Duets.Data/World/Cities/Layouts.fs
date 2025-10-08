module Duets.Data.World.Cities.Layouts

open Duets.Entities
open Duets.Data.World

/// Usual layout for a restaurant.
let restaurantRoomLayout menu =
    let kitchen =
        RoomType.Kitchen
        |> World.Room.create
        |> World.Node.create Ids.Restaurant.kitchen

    let diningRoom =
        RoomType.Restaurant menu
        |> World.Room.create
        |> World.Node.create Ids.Common.restaurant

    World.Graph.fromMany [ diningRoom; kitchen ]
    |> World.Graph.connect kitchen.Id diningRoom.Id North

/// Usual layout for a bar.
let barRoomLayout =
    RoomType.Bar
    |> World.Room.create
    |> World.Node.create Ids.Common.bar
    |> World.Graph.from

/// Usual layout for a bookstore.
let bookstoreLayout =
    RoomType.ReadingRoom
    |> World.Room.create
    |> World.Node.create Ids.Bookstore.readingRoom
    |> World.Graph.from

/// Usual layout for a cafe.
let cafeRoomLayout =
    RoomType.Cafe
    |> World.Room.create
    |> World.Node.create Ids.Common.cafe
    |> World.Graph.from

/// Usual layout for a casino.
let casinoLayout =
    let lobby =
        RoomType.Lobby
        |> World.Room.create
        |> World.Node.create Ids.Common.lobby

    let bar =
        RoomType.Bar |> World.Room.create |> World.Node.create Ids.Common.bar

    let casinoFloor =
        RoomType.CasinoFloor
        |> World.Room.create
        |> World.Node.create Ids.Casino.casinoFloor

    World.Graph.fromMany [ lobby; bar; casinoFloor ]
    |> World.Graph.connectMany
        [ lobby.Id, bar.Id, East; lobby.Id, casinoFloor.Id, North ]

/// Usual layout for a gym.
let gymLayout =
    let lobby =
        RoomType.Lobby
        |> World.Room.create
        |> World.Node.create Ids.Common.lobby

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
    let lobby =
        RoomType.Lobby
        |> World.Room.create
        |> World.Node.create Ids.Common.lobby

    let bar =
        RoomType.Bar |> World.Room.create |> World.Node.create Ids.Common.bar

    let bedroom =
        RoomType.Bedroom
        |> World.Room.create
        |> World.Node.create Ids.Home.bedroom

    World.Graph.fromMany [ lobby; bar; bedroom ]
    |> World.Graph.connectMany
        [ lobby.Id, bar.Id, East; lobby.Id, bedroom.Id, North ]

/// Usual layout for a merchandise workshop.
let merchandiseWorkshopLayout =
    let workshop =
        RoomType.Workshop
        |> World.Room.create
        |> World.Node.create Ids.Workshop.workshop

    World.Graph.from workshop

/// Usual layout for a radio studio.
let radioStudioLayout =
    let lobby =
        RoomType.Lobby
        |> World.Room.create
        |> World.Node.create Ids.Common.lobby

    let recordingRoom =
        RoomType.RecordingRoom
        |> World.Room.create
        |> World.Node.create Ids.Studio.recordingRoom

    World.Graph.fromMany [ lobby; recordingRoom ]
    |> World.Graph.connect lobby.Id recordingRoom.Id North

/// First option of a layout for a concert space.
let concertSpaceLayout1 =
    let lobby =
        RoomType.Lobby
        |> World.Room.create
        |> World.Node.create Ids.Common.lobby

    let bar =
        RoomType.Bar |> World.Room.create |> World.Node.create Ids.Common.bar

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
          lobby.Id, backstage.Id, West
          backstage.Id, stage.Id, NorthEast ]

/// Second option of a layout for a concert space.
let concertSpaceLayout2 =
    let lobby =
        RoomType.Lobby
        |> World.Room.create
        |> World.Node.create Ids.Common.lobby

    let bar =
        RoomType.Bar |> World.Room.create |> World.Node.create Ids.Common.bar

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
          bar.Id, backstage.Id, NorthEast
          backstage.Id, stage.Id, West ]

/// Third option of a layout for a concert space.
let concertSpaceLayout3 =
    let lobby =
        RoomType.Lobby
        |> World.Room.create
        |> World.Node.create Ids.Common.lobby

    let bar =
        RoomType.Bar |> World.Room.create |> World.Node.create Ids.Common.bar

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
          lobby.Id, backstage.Id, West
          backstage.Id, stage.Id, North ]

/// Fourth option of a layout for a concert space.
let concertSpaceLayout4 =
    let lobby =
        RoomType.Lobby
        |> World.Room.create
        |> World.Node.create Ids.Common.lobby

    let bar =
        RoomType.Bar |> World.Room.create |> World.Node.create Ids.Common.bar

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
          lobby.Id, backstage.Id, East
          backstage.Id, stage.Id, North ]

/// Usual layout for an airport.
let airportLayout =
    let lobby =
        RoomType.Lobby
        |> World.Room.create
        |> World.Node.create Ids.Common.lobby

    let securityControl =
        RoomType.SecurityControl
        |> World.Room.create
        |> World.Node.create Ids.Airport.securityControl

    let boardingGate =
        RoomType.BoardingGate
        |> World.Room.create
        |> World.Node.create Ids.Airport.boardingGate

    let cafe =
        RoomType.Cafe |> World.Room.create |> World.Node.create Ids.Common.cafe

    let restaurant =
        RoomType.Restaurant RestaurantCuisine.American
        |> World.Room.create
        |> World.Node.create Ids.Common.restaurant

    World.Graph.fromMany
        [ lobby; securityControl; boardingGate; cafe; restaurant ]
    |> World.Graph.connectMany
        [ lobby.Id, securityControl.Id, West
          boardingGate.Id, cafe.Id, North
          boardingGate.Id, restaurant.Id, South ]

/// Usual layout for a metro station.
let metroLayout =
    let platform =
        RoomType.Platform
        |> World.Room.create
        |> World.Node.create Ids.Metro.platform

    World.Graph.from platform

/// Usual layout for a rehearsal space.
let rehearsalSpaceLayout =
    let lobby =
        RoomType.Lobby
        |> World.Room.create
        |> World.Node.create Ids.Common.lobby

    let bar =
        RoomType.Bar |> World.Room.create |> World.Node.create Ids.Common.bar

    let room1 =
        RoomType.RehearsalRoom
        |> World.Room.create
        |> World.Node.create (Ids.RehearsalRoom.room 1)

    let room2 =
        RoomType.RehearsalRoom
        |> World.Room.create
        |> World.Node.create (Ids.RehearsalRoom.room 2)

    let room3 =
        RoomType.RehearsalRoom
        |> World.Room.create
        |> World.Node.create (Ids.RehearsalRoom.room 3)

    World.Graph.fromMany [ lobby; bar; room1; room2; room3 ]
    |> World.Graph.connectMany
        [ lobby.Id, bar.Id, East
          lobby.Id, room1.Id, North
          lobby.Id, room2.Id, NorthWest
          lobby.Id, room3.Id, West ]

/// Usual layout for a studio.
let studioLayout =
    let masteringRoom =
        RoomType.MasteringRoom
        |> World.Room.create
        |> World.Node.create Ids.Studio.masteringRoom

    let recordingRoom =
        RoomType.RecordingRoom
        |> World.Room.create
        |> World.Node.create Ids.Studio.recordingRoom

    World.Graph.fromMany [ masteringRoom; recordingRoom ]
    |> World.Graph.connectMany [ masteringRoom.Id, recordingRoom.Id, North ]

/// Usual layout for a hospital.
let hospitalLayout =
    let lobby =
        RoomType.Lobby
        |> World.Room.create
        |> World.Node.create Ids.Common.lobby

    World.Graph.from lobby
