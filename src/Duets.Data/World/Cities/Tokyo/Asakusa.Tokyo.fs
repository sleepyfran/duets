module rec Duets.Data.World.Cities.Tokyo.Asakusa

open Duets.Data.World.Cities
open Duets.Entities

let private nakamiseStreet (zone: Zone) =
    let street =
        World.Street.create "Nakamise Street" (StreetType.Split(South, 3))

    let restaurants =
        [ ("Asakusa Menchi", 80<quality>, Japanese, zone.Id)
          ("Daikokuya Tempura", 85<quality>, Japanese, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Asakusa Coffee", 75<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let concertSpaces =
        [ ("Asakusa Gold Sounds",
           200,
           75<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bookstores =
        [ ("Asakusa Bookstore", 70<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let metroStation =
        ("Asakusa Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces cafes
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces bookstores
        |> World.Street.addPlace metroStation

    street, metroStation

let private sumidaRiver (zone: Zone) =
    let street = World.Street.create "Sumida Riverside" StreetType.OneWay

    let concertSpaces =
        [ ("Sumida Park Stage",
           2000,
           80<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let createZone city =
    let asakusaZone = World.Zone.create "Asakusa"

    let nakamiseStreet, metroStation = nakamiseStreet asakusaZone
    let sumidaRiver = sumidaRiver asakusaZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = nakamiseStreet.Id
          PlaceId = metroStation.Id }

    asakusaZone
    |> World.Zone.addStreet (World.Node.create nakamiseStreet.Id nakamiseStreet)
    |> World.Zone.addStreet (World.Node.create sumidaRiver.Id sumidaRiver)
    |> World.Zone.connectStreets nakamiseStreet.Id sumidaRiver.Id East
    |> World.Zone.addDescriptor Historic
    |> World.Zone.addDescriptor Cultural
    |> World.Zone.addMetroStation station
