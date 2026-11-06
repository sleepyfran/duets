module rec Duets.Data.World.Cities.Prague.Ruzyně

open Duets.Data.World.Cities.Prague
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let kLetišti (zone: Zone) =
    let street =
        World.Street.create "K Letišti" (StreetType.Split(North, 1))

    let airport =
        PlaceCreators.createAirport
            street.Id
            ("Václav Havel Airport Prague", 85<quality>, zone.Id)

    street |> World.Street.addPlace airport

let evropská (zone: Zone) =
    let street =
        World.Street.create Ids.Street.evropská (StreetType.Split(East, 2))

    let carDealers =
        [ ("Auto Ruzyně",
           69<quality>,
           zone.Id,
           { Dealer =
               (Character.from
                   "Petr Novák"
                   Male
                   (Shorthands.Summer 16<days> 1981<years>))
             PriceRange = CarPriceRange.Budget }) ]
        |> List.map (PlaceCreators.createCarDealer street.Id)

    let metroStation =
        ("Nádraží Veleslavín Station", zone.Id)
        |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces carDealers
        |> World.Street.addPlace metroStation

    street, metroStation

let zone =
    let ruzyněZone = World.Zone.create Ids.Zone.ruzyně

    let kLetišti = kLetišti ruzyněZone
    let evropská, metroStation = evropská ruzyněZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = evropská.Id
          PlaceId = metroStation.Id }

    ruzyněZone
    |> World.Zone.addStreet (World.Node.create kLetišti.Id kLetišti)
    |> World.Zone.addStreet (World.Node.create evropská.Id evropská)
    |> World.Zone.connectStreets kLetišti.Id evropská.Id South

    |> World.Zone.addMetroStation station
