module rec Duets.Data.World.Cities.Sydney.Bondi

open Duets.Data.World.Cities
open Duets.Entities

let private campbellParade (zone: Zone) =
    let street =
        World.Street.create "Campbell Parade" (StreetType.Split(East, 2))

    let hotels =
        [ ("QT Bondi", 89<quality>, 600m<dd>, zone.Id)
          ("Hotel Bondi", 87<quality>, 550m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let restaurants =
        [ ("Icebergs Dining Room", 93<quality>, Italian, zone.Id)
          ("Bondi Trattoria", 90<quality>, Spanish, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Speedos Cafe", 88<quality>, zone.Id)
          ("Bondi Wholefoods", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let bookstores =
        [ ("Gertrude & Alice", 87<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let metroStation =
        ("Bondi Junction Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces hotels
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces cafes
        |> World.Street.addPlaces bookstores
        |> World.Street.addPlace metroStation

    street, metroStation

let private hallStreet (zone: Zone) =
    let street = World.Street.create "Hall Street" StreetType.OneWay

    let bars =
        [ ("Bondi Hardware", 85<quality>, zone.Id)
          ("The Anchor", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let metroStation =
        ("Hall Street Metro", zone.Id) |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces bars
        |> World.Street.addPlace metroStation

    street, metroStation

let zone =
    let bondiZone = World.Zone.create "Bondi"
    let campbellParade, campbellParadeMetro = campbellParade bondiZone
    let hallStreet, _ = hallStreet bondiZone

    let campbellParadeMetroStation =
        { Lines = [ Blue ]
          LeavesToStreet = campbellParade.Id
          PlaceId = campbellParadeMetro.Id }

    bondiZone
    |> World.Zone.addStreet (World.Node.create campbellParade.Id campbellParade)
    |> World.Zone.addStreet (World.Node.create hallStreet.Id hallStreet)
    |> World.Zone.connectStreets campbellParade.Id hallStreet.Id North
    |> World.Zone.addDescriptor Coastal
    |> World.Zone.addDescriptor EntertainmentHeart
    |> World.Zone.addMetroStation campbellParadeMetroStation
