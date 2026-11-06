module rec Duets.Data.World.Cities.Prague.Libeň

open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let ocelářská (zone: Zone) =
    let street =
        World.Street.create "Ocelářská" (StreetType.Split(East, 2))

    let concertSpaces =
        [ ("O2 Arena", 18000, 95<quality>, Layouts.concertSpaceLayout2, zone.Id)
          ("O2 Universum",
           4500,
           92<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Pivovar U Korunky", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("Restaurace U Korunky", 84<quality>, Czech, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let metroStation =
        ("Českomoravská Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlace metroStation

    street, metroStation

let voctářova (zone: Zone) =
    let street =
        World.Street.create "Voctářova" (StreetType.Split(West, 1))

    let carDealers =
        [ ("Auto Libeň",
           77<quality>,
           zone.Id,
           { Dealer =
               (Character.from
                   "Jana Horáková"
                   Female
                   (Shorthands.Autumn 20<days> 1986<years>))
             PriceRange = CarPriceRange.MidRange }) ]
        |> List.map (PlaceCreators.createCarDealer street.Id)

    let bars =
        [ ("U Hubatků", 82<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("Restaurace U Hubatků", 81<quality>, Czech, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    street
    |> World.Street.addPlaces carDealers
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces restaurants

let zone =
    let libeňZone = World.Zone.create Ids.Zone.libeň

    let ocelářská, metroStation = ocelářská libeňZone
    let voctářova = voctářova libeňZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = ocelářská.Id
          PlaceId = metroStation.Id }

    libeňZone
    |> World.Zone.addStreet (World.Node.create ocelářská.Id ocelářská)
    |> World.Zone.addStreet (World.Node.create voctářova.Id voctářova)
    |> World.Zone.connectStreets ocelářská.Id voctářova.Id South
    |> World.Zone.addMetroStation station
