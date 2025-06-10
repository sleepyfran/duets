module rec Duets.Data.World.Cities.Prague.Holešovice

open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let plynární (zone: Zone) =
    let street = World.Street.create "Plynární" (StreetType.Split(West, 3))

    let concertSpaces =
        [ ("Cross Club", 400, 86<quality>, Layouts.concertSpaceLayout3, zone.Id)
          ("Tipsport Arena",
           13000,
           88<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Láhev Sud", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let cafes =
        [ ("Cup of Joy", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let metroStation =
        ("Nádraží Holešovice Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces cafes
        |> World.Street.addPlace metroStation

    street, metroStation

let dělnická (zone: Zone) =
    let street = World.Street.create "Dělnická" (StreetType.Split(North, 2))

    let casinos =
        [ ("Golden Dice Casino", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCasino street.Id)

    let restaurants =
        [ ("Trattoria Da Antonio", 92<quality>, Italian, zone.Id)
          ("Big Burger", 84<quality>, American, zone.Id)
          ("Taco Fiesta", 88<quality>, Mexican, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let recordingStudios =
        [ ("Holešovické Tóny",
           89<quality>,
           280m<dd>,
           (Character.from
               "Ondřej Soukup"
               Male
               (Shorthands.Winter 2<days> 1960<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let rehearsalSpaces =
        [ ("Místnost Prove", 88<quality>, 120m<dd>, zone.Id)
          ("Zkušebna Křižík", 86<quality>, 110m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let bars =
        [ ("Duchy Spodky", 94<quality>, zone.Id)
          ("Mug Mountain", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let cafes =
        [ ("Java Palace", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlaces casinos
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces recordingStudios
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces cafes

let zone =
    let holešoviceZone = World.Zone.create "Holešovice"

    let plynární, metroStation = plynární holešoviceZone
    let dělnická = dělnická holešoviceZone

    let station =
        { Lines = [ Red ]
          LeavesToStreet = plynární.Id
          PlaceId = metroStation.Id }

    holešoviceZone
    |> World.Zone.addStreet (World.Node.create plynární.Id plynární)
    |> World.Zone.addStreet (World.Node.create dělnická.Id dělnická)
    |> World.Zone.connectStreets plynární.Id dělnická.Id South
    |> World.Zone.addDescriptor EntertainmentHeart
    |> World.Zone.addMetroStation station
