module rec Duets.Data.World.Cities.LosAngeles.BeverlyHills

open Duets.Data.World.Cities
open Duets.Entities
open Fugit.Months

let private rodeoDrive (zone: Zone) =
    let street = World.Street.create "Rodeo Drive" (StreetType.Split(North, 3))

    let hotels =
        [ ("The Beverly Wilshire", 95<quality>, 900m<dd>, zone.Id)
          ("The Peninsula Beverly Hills", 97<quality>, 950m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let restaurants =
        [ ("Spago Beverly Hills", 95<quality>, Italian, zone.Id)
          ("The Palm", 93<quality>, American, zone.Id)
          ("Matsuhisa", 94<quality>, Japanese, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Urth Caffé", 88<quality>, zone.Id)
          ("Alfred Coffee", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let bookstores =
        [ ("Book Soup", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let metroStation =
        ("Beverly Hills Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces hotels
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces cafes
        |> World.Street.addPlaces bookstores
        |> World.Street.addPlace metroStation

    street, metroStation

let private wilshireBoulevard (zone: Zone) =
    let street =
        World.Street.create "Wilshire Boulevard" (StreetType.Split(East, 2))

    let hotels =
        [ ("The Beverly Hilton", 94<quality>, 850m<dd>, zone.Id)
          ("Waldorf Astoria Beverly Hills", 96<quality>, 980m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let restaurants =
        [ ("The Belvedere", 92<quality>, American, zone.Id)
          ("The Polo Lounge", 96<quality>, Italian, zone.Id)
          ("CUT by Wolfgang Puck", 95<quality>, American, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let bars =
        [ ("The Blvd", 91<quality>, zone.Id)
          ("The Rooftop by JG", 93<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let concertSpaces =
        [ ("The Wallis Annenberg Center",
           1800,
           92<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street
    |> World.Street.addPlaces hotels
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces concertSpaces

let private canyonDrive (zone: Zone) =
    let street = World.Street.create "Canyon Drive" StreetType.OneWay

    let studios =
        [ ("Beverly Hills Recording",
           90<quality>,
           380m<dd>,
           (Character.from "David Foster" Male (November 1 1949)),
           zone.Id)
          ("Platinum Sound",
           92<quality>,
           400m<dd>,
           (Character.from "Quincy Jones" Male (March 14 1933)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let rehearsalSpaces =
        [ ("Elite Rehearsals", 88<quality>, 250m<dd>, zone.Id)
          ("The Hills Practice Room", 85<quality>, 220m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    street
    |> World.Street.addPlaces studios
    |> World.Street.addPlaces rehearsalSpaces

let createZone _city =
    let beverlyHillsZone = World.Zone.create "Beverly Hills"

    let rodeoDrive, metroStation = rodeoDrive beverlyHillsZone
    let wilshireBoulevard = wilshireBoulevard beverlyHillsZone
    let canyonDrive = canyonDrive beverlyHillsZone

    let metroStation =
        { Lines = [ Red ]
          LeavesToStreet = rodeoDrive.Id
          PlaceId = metroStation.Id }

    beverlyHillsZone
    |> World.Zone.addStreet (World.Node.create rodeoDrive.Id rodeoDrive)
    |> World.Zone.addStreet (
        World.Node.create wilshireBoulevard.Id wilshireBoulevard
    )
    |> World.Zone.addStreet (World.Node.create canyonDrive.Id canyonDrive)
    |> World.Zone.connectStreets rodeoDrive.Id wilshireBoulevard.Id East
    |> World.Zone.connectStreets wilshireBoulevard.Id canyonDrive.Id North
    |> World.Zone.addDescriptor Glitz
    |> World.Zone.addDescriptor Luxurious
    |> World.Zone.addMetroStation metroStation
