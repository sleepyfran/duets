module rec Duets.Data.World.Cities.London.Greenwich

open Duets.Data.World.Cities
open Duets.Entities

let private greenwichMarket (zone: Zone) =
    let street =
        World.Street.create "Greenwich Market" (StreetType.Split(South, 2))
        |> World.Street.tag "market"

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

    let restaurants =
        [ ("Bianco43", 80<quality>, Italian, zone.Id)
          ("Goddards at Greenwich", 82<quality>, British, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let metroStation =
        ("Cutty Sark Station", zone.Id) |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces shops
        |> World.Street.addPlaces cafes
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlace metroStation

    street, metroStation

let private royalObservatory (zone: Zone) =
    let street =
        World.Street.create "Royal Observatory" StreetType.OneWay
        |> World.Street.tag "prime-meridian"

    let cafes =
        [ ("Astronomy Cafe", 68<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let bookstores =
        [ ("Greenwich Books", 75<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    street |> World.Street.addPlaces cafes |> World.Street.addPlaces bookstores

let private greenwichPark (zone: Zone) =
    let street =
        World.Street.create "Greenwich Park" StreetType.OneWay
        |> World.Street.tag "park"

    let concertSpaces =
        [ ("Bandstand", 300, 70<quality>, Layouts.concertSpaceLayout1, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Trafalgar Tavern", 84<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id >> World.Place.tag "pub")

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces bars

let private peninsulaSquare (zone: Zone) =
    let street =
        World.Street.create "Peninsula Square" (StreetType.Split(North, 2))
        |> World.Street.tag "waterfront"

    let concertSpaces =
        [ ("The O2 Arena",
           20000,
           95<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id)
          ("indigo at The O2",
           2700,
           86<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let hotels =
        [ ("InterContinental London - The O2", 88<quality>, 320m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces hotels

let zone =
    let zone = World.Zone.create "Greenwich"
    let greenwichMarket, metroStation = greenwichMarket zone
    let royalObservatory = royalObservatory zone
    let greenwichPark = greenwichPark zone
    let peninsulaSquare = peninsulaSquare zone

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
    |> World.Zone.addStreet (
        World.Node.create peninsulaSquare.Id peninsulaSquare
    )
    |> World.Zone.connectStreets greenwichMarket.Id royalObservatory.Id North
    |> World.Zone.connectStreets greenwichMarket.Id greenwichPark.Id East
    |> World.Zone.connectStreets greenwichMarket.Id peninsulaSquare.Id NorthEast
    |> World.Zone.addMetroStation metroStation
