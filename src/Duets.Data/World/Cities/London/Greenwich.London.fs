module rec Duets.Data.World.Cities.London.Greenwich

open Duets.Data.World.Cities
open Duets.Entities

let private greenwichMarket (zone: Zone) =
    let street =
        World.Street.create "Greenwich Market" (StreetType.Split(South, 2))

    let shops =
        [ ("Greenwich Vintage", zone.Id); ("Market Antiques", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    let cafes =
        [ ("Heap's Sausage Cafe", 72<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let concertSpaces =
        [ ("Trinity Laban Hall",
           400,
           80<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let metroStation =
        ("Cutty Sark Station", zone.Id) |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces shops
        |> World.Street.addPlaces cafes
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlace metroStation

    street, metroStation

let private royalObservatory (zone: Zone) =
    let street = World.Street.create "Royal Observatory" StreetType.OneWay

    let cafes =
        [ ("Astronomy Cafe", 68<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let bookstores =
        [ ("Greenwich Books", 75<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    street |> World.Street.addPlaces cafes |> World.Street.addPlaces bookstores

let private greenwichPark (zone: Zone) =
    let street = World.Street.create "Greenwich Park" StreetType.OneWay

    let concertSpaces =
        [ ("Bandstand", 300, 70<quality>, Layouts.concertSpaceLayout1, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let zone =
    let zone = World.Zone.create "Greenwich"
    let greenwichMarket, metroStation = greenwichMarket zone
    let royalObservatory = royalObservatory zone
    let greenwichPark = greenwichPark zone

    let metroStation =
        { Lines = [ Blue ]
          LeavesToStreet = greenwichMarket.Id
          PlaceId = metroStation.Id }

    zone
    |> World.Zone.addStreet (
        World.Node.create greenwichMarket.Id greenwichMarket
    )
    |> World.Zone.addStreet (
        World.Node.create royalObservatory.Id royalObservatory
    )
    |> World.Zone.addStreet (World.Node.create greenwichPark.Id greenwichPark)
    |> World.Zone.connectStreets greenwichMarket.Id royalObservatory.Id North
    |> World.Zone.connectStreets greenwichMarket.Id greenwichPark.Id East
    |> World.Zone.addDescriptor Nature
    |> World.Zone.addDescriptor Cultural
    |> World.Zone.addMetroStation metroStation
