module rec Duets.Data.World.Cities.Tokyo.Shibuya

open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let private centerGai (zone: Zone) =
    let street = World.Street.create "Center Gai" (StreetType.Split(North, 3))

    let restaurants =
        [ ("Ichiran Ramen", 88<quality>, Japanese, zone.Id)
          ("Katsuya", 80<quality>, Japanese, zone.Id)
          ("Burger Mania", 75<quality>, American, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Streamer Coffee Company", 82<quality>, zone.Id)
          ("Shibuya Starbucks", 78<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let bars =
        [ ("Nonbei Yokocho", 84<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let concertSpaces =
        [ ("Shibuya O-East",
           1300,
           85<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let metroStation =
        ("Shibuya Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces cafes
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlace metroStation

    street, metroStation

let private catStreet (zone: Zone) =
    let street = World.Street.create "Cat Street" (StreetType.Split(East, 2))

    let studios =
        [ ("Cat Street Studio",
           75<quality>,
           200m<dd>,
           (Character.from
               "Yuki Tanaka"
               Female
               (Shorthands.Spring 12<days> 1990<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let rehearsalSpaces =
        [ ("Harajuku Rehearsal", 70<quality>, 150m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let concertSpaces =
        [ ("Shibuya Milkyway",
           250,
           78<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id)
          ("Shibuya WWW X",
           500,
           82<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street
    |> World.Street.addPlaces studios
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces concertSpaces

let zone =
    let shibuyaZone = World.Zone.create "Shibuya"

    let centerGai, metroStation = centerGai shibuyaZone
    let catStreet = catStreet shibuyaZone

    let metroStation =
        { Lines = [ Blue; Red ]
          LeavesToStreet = centerGai.Id
          PlaceId = metroStation.Id }

    shibuyaZone
    |> World.Zone.addStreet (World.Node.create centerGai.Id centerGai)
    |> World.Zone.addStreet (World.Node.create catStreet.Id catStreet)
    |> World.Zone.connectStreets centerGai.Id catStreet.Id East
    |> World.Zone.addDescriptor EntertainmentHeart
    |> World.Zone.addDescriptor Creative
    |> World.Zone.addMetroStation metroStation
