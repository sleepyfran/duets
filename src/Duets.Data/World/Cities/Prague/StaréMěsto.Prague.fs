module rec Duets.Data.World.Cities.Prague.StaréMěsto

open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let staroměstskéNáměstí city (zone: Zone) =
    let street =
        World.Street.create "Staroměstské náměstí" (StreetType.Split(South, 3))

    let concertSpaces =
        [ ("Roxy", 900, 90<quality>, Layouts.concertSpaceLayout3, zone.Id)
          ("La Fabrica", 800, 86<quality>, Layouts.concertSpaceLayout3, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bookstores =
        [ ("Knihkupectví Academia", 92<quality>, zone.Id)
          ("Shakespeare a synové", 95<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let hotels =
        [ ("Hotel Ambassador", 90<quality>, 120m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let restaurants =
        [ ("Istanbul Kebab", 86<quality>, Turkish, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let metroStation =
        ("Staroměstská Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces bookstores
        |> World.Street.addPlaces hotels
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlace metroStation

    street, metroStation

let karlova (zone: Zone) =
    let street = World.Street.create "Karlova" (StreetType.Split(West, 2))

    let merchandiseWorkshops =
        [ ("Prague Merch", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    let recordingStudios =
        [ ("Vyšehrad Nahrávání",
           92<quality>,
           340m<dd>,
           (Character.from
               "Tomáš Dvořák"
               Male
               (Shorthands.Summer 10<days> 1978<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let rehearsalSpaces =
        [ ("Rocková Nahrávka", 87<quality>, 120m<dd>, zone.Id)
          ("Staroměstská Zkušebna", 90<quality>, 150m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let concertSpaces =
        [ ("Forum Karlín",
           3000,
           91<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street
    |> World.Street.addPlaces merchandiseWorkshops
    |> World.Street.addPlaces recordingStudios
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces concertSpaces

let createZone city =
    let staréMěstoZone = World.Zone.create "Staré Město"

    let staroměstskéNáměstí, metroStation =
        staroměstskéNáměstí city staréMěstoZone

    let karlova = karlova staréMěstoZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = staroměstskéNáměstí.Id
          PlaceId = metroStation.Id }

    staréMěstoZone
    |> World.Zone.addStreet (
        World.Node.create staroměstskéNáměstí.Id staroměstskéNáměstí
    )
    |> World.Zone.addStreet (World.Node.create karlova.Id karlova)
    |> World.Zone.connectStreets staroměstskéNáměstí.Id karlova.Id East
    |> World.Zone.addDescriptor Historic
    |> World.Zone.addMetroStation station
