module rec Duets.Data.World.Cities.London.Camden

open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let private camdenHighStreet (city: City) (zone: Zone) =
    let street =
        World.Street.create "Camden High Street" (StreetType.Split(North, 2))

    let carDealers =
        [ ("Camden Motor Co",
           78<quality>,
           zone.Id,
           { Dealer =
               (Character.from
                   "Sophie Clarke"
                   Female
                   (Shorthands.Autumn 14<days> 1985<years>))
             PriceRange = CarPriceRange.MidRange }) ]
        |> List.map (PlaceCreators.createCarDealer street.Id)

    let gyms =
        [ ("Gymbox Camden", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let recordingStudios =
        [ ("The Pool Studio",
           89<quality>,
           320m<dd>,
           (Character.from
               "Ian Caple"
               Male
               (Shorthands.Spring 20<days> 1962<years>)),
           zone.Id)
          ("Sarm Studios",
           92<quality>,
           380m<dd>,
           (Character.from
               "Jill Sinclair"
               Female
               (Shorthands.Summer 5<days> 1952<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let bars =
        [ ("The World's End", 80<quality>, zone.Id)
          ("Dublin Castle", 78<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let concertSpaces =
        [ ("Roundhouse", 1700, 88<quality>, Layouts.concertSpaceLayout2, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let rehearsalSpaces =
        [ ("Camden Practice Rooms", 75<quality>, 160m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let radioStudios =
        [ ("Absolute Radio", 90<quality>, "Rock", zone.Id) ]
        |> List.map (PlaceCreators.createRadioStudio city street.Id)

    let metroStation =
        ("Camden Town Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces carDealers
        |> World.Street.addPlaces gyms
        |> World.Street.addPlaces recordingStudios
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces rehearsalSpaces
        |> World.Street.addPlaces radioStudios
        |> World.Street.addPlace metroStation

    street, metroStation

let private chalkFarm (city: City) (zone: Zone) =
    let street =
        World.Street.create "Chalk Farm" (StreetType.Split(West, 2))

    let recordingStudios =
        [ ("Abbey Road Studios",
           98<quality>,
           500m<dd>,
           (Character.from
               "Giles Martin"
               Male
               (Shorthands.Autumn 9<days> 1969<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let cafes =
        [ ("The Coffee Jar", 75<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let bookstores =
        [ ("Owl Bookshop", 80<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    street 
    |> World.Street.addPlaces recordingStudios
    |> World.Street.addPlaces cafes 
    |> World.Street.addPlaces bookstores

let private regentsPark (zone: Zone) =
    let street =
        World.Street.create "Regent's Park" StreetType.OneWay

    let concertSpaces =
        [ ("Open Air Theatre",
           1250,
           85<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let cafes =
        [ ("Regent's Park Cafe", 70<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces cafes

let createZone (city: City) =
    let zone = World.Zone.create "Camden"
    let camdenHighStreet, metroStation = camdenHighStreet city zone
    let chalkFarm = chalkFarm city zone
    let regentsPark = regentsPark zone

    let metroStation =
        { Lines = [ Red; Blue ]
          LeavesToStreet = camdenHighStreet.Id
          PlaceId = metroStation.Id }

    zone
    |> World.Zone.addStreet (
        World.Node.create camdenHighStreet.Id camdenHighStreet
    )
    |> World.Zone.addStreet (World.Node.create chalkFarm.Id chalkFarm)
    |> World.Zone.addStreet (World.Node.create regentsPark.Id regentsPark)
    |> World.Zone.connectStreets camdenHighStreet.Id chalkFarm.Id West
    |> World.Zone.connectStreets camdenHighStreet.Id regentsPark.Id North
    |> World.Zone.addMetroStation metroStation

let zone = createZone
