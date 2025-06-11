module rec Duets.Data.World.Cities.NewYork.Astoria

open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let private ditmarsBlvd (zone: Zone) =
    let street =
        World.Street.create "Ditmars Boulevard" (StreetType.Split(East, 3))

    let airport =
        ("John F. Kennedy International Airport", 72<quality>, zone.Id)
        |> PlaceCreators.createAirport street.Id

    let bars =
        [ ("Bohemian Hall and Beer Garden", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let cafes =
        [ ("Java Palace", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let metroStation =
        ("Astoria-Ditmars Boulevard Station", zone.Id)
        |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlace airport
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces cafes
        |> World.Street.addPlace metroStation

    street, metroStation

let private steinwayStreet (zone: Zone) =
    let street =
        World.Street.create "Steinway Street" (StreetType.Split(North, 4))

    let casinos =
        [ ("Statue of Luck Casino", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCasino street.Id)

    let concertSpaces =
        [ ("Forest Hills Stadium",
           14000,
           83<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let rehearsalSpaces =
        [ ("Queens Quavers", 92<quality>, 170m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let restaurants =
        [ ("Casa Enrique", 89<quality>, Mexican, zone.Id)
          ("Istanbul Kebab House", 67<quality>, Turkish, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let studios =
        [ ("Astoria Sound",
           92<quality>,
           340m<dd>,
           (Character.from
               "Tom Davis"
               Male
               (Shorthands.Summer 10<days> 1978<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    street
    |> World.Street.addPlaces casinos
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces studios

let zone =
    let astoriaZone = World.Zone.create "Astoria"

    let ditmarsBlvd, metroStation = ditmarsBlvd astoriaZone
    let steinwayStreet = steinwayStreet astoriaZone

    let metroStation =
        { Lines = [ Blue ]
          LeavesToStreet = ditmarsBlvd.Id
          PlaceId = metroStation.Id }

    astoriaZone
    |> World.Zone.addStreet (World.Node.create ditmarsBlvd.Id ditmarsBlvd)
    |> World.Zone.addStreet (World.Node.create steinwayStreet.Id steinwayStreet)
    |> World.Zone.connectStreets ditmarsBlvd.Id steinwayStreet.Id South
    |> World.Zone.addDescriptor Cultural
    |> World.Zone.addMetroStation metroStation
