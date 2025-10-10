module rec Duets.Data.World.Cities.Prague.Ruzyně

open Duets.Data.World.Cities.Prague
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let kLetišti (zone: Zone) =
    let street =
        World.Street.create "K Letišti" (StreetType.Split(North, 1))
        |> World.Street.attachContext
            """
        K Letišti is the main approach road to Václav Havel Airport Prague, characterized
        by modern infrastructure and aviation-related facilities. The street is wide
        and designed for efficient traffic flow, with clear signage directing travelers
        to different airport terminals. Commercial buildings, car rental agencies,
        and airport hotels line the route. The area has a distinctly functional character,
        dominated by the airport's presence. Aircraft can be seen taking off and
        landing in the distance, while the constant flow of taxis and airport shuttles
        creates a transient, international atmosphere.
"""

    let airport =
        PlaceCreators.createAirport
            street.Id
            ("Václav Havel Airport Prague", 85<quality>, zone.Id)

    street |> World.Street.addPlace airport

let evropská (zone: Zone) =
    let street =
        World.Street.create Ids.Street.evropská (StreetType.Split(East, 2))
        |> World.Street.attachContext
            """
        Evropská is a major arterial road connecting the airport area to central Prague,
        featuring a mix of commercial developments and residential areas. The street
        is characterized by modern office buildings, shopping centers, and the Nádraží Veleslavín
        metro station hub. Wide lanes accommodate heavy traffic, while pedestrian infrastructure
        includes modern sidewalks and crossings. The architecture is predominantly contemporary,
        with glass-fronted buildings and standardized commercial developments. The area serves
        as a gateway between the international airport zone and Prague's urban core.
"""

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
