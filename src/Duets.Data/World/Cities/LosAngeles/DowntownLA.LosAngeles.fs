module rec Duets.Data.World.Cities.LosAngeles.DowntownLA

open Duets.Data.World.Cities
open Duets.Entities
open Fugit.Months

let financialCorridor city (zone: Zone) =
    let hotels =
        [ ("The Biltmore Hotel", 88<quality>, 400m<dd>, zone.Id) ]
        |> List.map PlaceCreators.createHotel

    let restaurants =
        [ ("Grand Central Market", 85<quality>, American, zone.Id)
          ("Perch", 90<quality>, Japanese, zone.Id)
          ("Patina", 92<quality>, Italian, zone.Id) ]
        |> List.map PlaceCreators.createRestaurant

    let gyms =
        [ ("Crunch Fitness", 80<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city)

    let recordingStudios =
        [ ("United Recording",
           85<quality>,
           280m<dd>,
           (Character.from "David Foster" Male (November 1 1949)),
           zone.Id)
          ("Capitol Studios",
           90<quality>,
           300m<dd>,
           (Character.from "Al Schmitt" Male (April 17 1930)),
           zone.Id) ]
        |> List.map PlaceCreators.createStudio

    let cafes =
        [ ("Starbucks", 75<quality>, zone.Id) ]
        |> List.map PlaceCreators.createCafe

    let metroStation =
        ("Downtown LA Station", zone.Id) |> PlaceCreators.createMetro

    World.Street.create "Financial Corridor" (StreetType.Split(North, 3))
    |> World.Street.addPlaces hotels
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces gyms
    |> World.Street.addPlaces recordingStudios
    |> World.Street.addPlaces cafes
    |> World.Street.addPlace metroStation

let grandStreet (zone: Zone) =
    let concertSpaces =
        [ ("The Novo", 2300, 88<quality>, Layouts.concertSpaceLayout3, zone.Id) ]
        |> List.map PlaceCreators.createConcertSpace

    let restaurants =
        [ ("Broken Spanish", 89<quality>, Mexican, zone.Id)
          ("Bestia", 91<quality>, Mexican, zone.Id) ]
        |> List.map PlaceCreators.createRestaurant

    let casinos =
        [ ("The Bicycle Casino", 80<quality>, zone.Id)
          ("Hollywood Park Casino", 78<quality>, zone.Id) ]
        |> List.map PlaceCreators.createCasino

    let recordingStudios =
        [ ("EastWest Studios",
           92<quality>,
           350m<dd>,
           (Character.from "Greg Kurstin" Male (May 14 1969)),
           zone.Id)
          ("Village Studios",
           95<quality>,
           380m<dd>,
           (Character.from "Rick Rubin" Male (March 10 1963)),
           zone.Id) ]
        |> List.map PlaceCreators.createStudio

    World.Street.create "Grand Street" (StreetType.Split(East, 2))
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces casinos
    |> World.Street.addPlaces recordingStudios

let backstreets (zone: Zone) =
    let recordingStudios =
        [ ("Conway Recording Studios",
           83<quality>,
           290m<dd>,
           (Character.from "Bruce Swedien" Male (April 19 1934)),
           zone.Id)
          ("Paramount Recording Studio",
           79<quality>,
           250m<dd>,
           (Character.from "Rodney Jenkins" Male (October 24 1969)),
           zone.Id) ]
        |> List.map PlaceCreators.createStudio

    let bars =
        [ ("The Redwood Bar", 72<quality>, zone.Id)
          ("Seven Grand", 76<quality>, zone.Id) ]
        |> List.map PlaceCreators.createBar

    let rehearsalSpaces =
        [ ("The Beat Lab", 62<quality>, 140m<dd>, zone.Id) ]
        |> List.map PlaceCreators.createRehearsalSpace

    let bookstores =
        [ ("The Last Bookstore", 88<quality>, zone.Id) ]
        |> List.map PlaceCreators.createBookstore

    let cafes =
        [ ("Groundwork Coffee", 74<quality>, zone.Id) ]
        |> List.map PlaceCreators.createCafe

    World.Street.create "Backstreets" (StreetType.Split(West, 2))
    |> World.Street.addPlaces recordingStudios
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces cafes

let createZone city =
    let downtownLAZone = World.Zone.create "Downtown LA"

    let financialCorridor = financialCorridor city downtownLAZone
    let grandStreet = grandStreet downtownLAZone
    let backstreets = backstreets downtownLAZone

    let metroStation =
        { Line = Blue
          LeavesToStreet = financialCorridor.Id }

    downtownLAZone
    |> World.Zone.addStreet (
        World.Node.create financialCorridor.Id financialCorridor
    )
    |> World.Zone.addStreet (World.Node.create grandStreet.Id grandStreet)
    |> World.Zone.addStreet (World.Node.create backstreets.Id backstreets)
    |> World.Zone.connectStreets financialCorridor.Id grandStreet.Id East
    |> World.Zone.connectStreets grandStreet.Id backstreets.Id North
    |> World.Zone.addDescriptor BusinessDistrict
    |> World.Zone.addMetroStation metroStation
