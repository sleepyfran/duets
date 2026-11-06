module rec Duets.Data.World.Cities.Prague.Vršovice

open Duets.Data.World.Cities.Prague
open Duets.Data.World.Cities
open Duets.Entities

let krymská (zone: Zone) =
    let street =
        World.Street.create Ids.Street.krymská (StreetType.Split(East, 1))

    let bookstores =
        [ ("Shakespeare and Sons", 92<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let concertSpaces =
        [ ("Café v lese", 150, 85<quality>, Layouts.concertSpaceLayout1, zone.Id)
          ("Bad Flash Bar",
           100,
           82<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let restaurants =
        [ ("Café Sladkovský", 86<quality>, Czech, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Kafé Frída", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces cafes

let moskevská (zone: Zone) =
    let street =
        World.Street.create "Moskevská" (StreetType.Split(West, 2))

    let concertSpaces =
        [ ("Waldeska Pub", 80, 78<quality>, Layouts.concertSpaceLayout1, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let restaurants =
        [ ("U Veverky", 84<quality>, Czech, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces restaurants

let vyšehradská (zone: Zone) =
    let street =
        World.Street.create "Vyšehradská" (StreetType.Split(South, 2))

    let metroStation =
        ("Vyšehrad Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street = street |> World.Street.addPlace metroStation

    street, metroStation

let zone =
    let vršoviceZone = World.Zone.create Ids.Zone.vršovice
    let krymská = krymská vršoviceZone
    let moskevská = moskevská vršoviceZone
    let vyšehradská, metroStation = vyšehradská vršoviceZone

    let station =
        { Lines = [ Red ]
          LeavesToStreet = vyšehradská.Id
          PlaceId = metroStation.Id }

    vršoviceZone
    |> World.Zone.addStreet (World.Node.create krymská.Id krymská)
    |> World.Zone.addStreet (World.Node.create moskevská.Id moskevská)
    |> World.Zone.addStreet (World.Node.create vyšehradská.Id vyšehradská)
    |> World.Zone.connectStreets krymská.Id moskevská.Id North
    |> World.Zone.connectStreets moskevská.Id vyšehradská.Id South
    |> World.Zone.addMetroStation station
