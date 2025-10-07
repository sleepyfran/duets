module rec Duets.Data.World.Cities.NewYork.Jamaica

open Duets.Data.World.Cities
open Duets.Entities

let private vanWyckExpressway (zone: Zone) =
    let street =
        World.Street.create Ids.Street.vanWyckExpressway StreetType.OneWay

    let casinos =
        [ ("Resorts World Casino", 89<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCasino street.Id)

    let airports =
        [ ("John F. Kennedy International Airport", 95<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createAirport street.Id)

    let metroStation =
        ("JFK Airport AirTrain", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces casinos
        |> World.Street.addPlaces airports
        |> World.Street.addPlace metroStation
        |> World.Street.attachContext
            """
        The Van Wyck Expressway is a major highway corridor leading to JFK
        International Airport, dominated by the roar of traffic and jet engines overhead.
        Aircraft follow flight paths directly above, their landing gear visible
        as they descend toward the runways. The expressway cuts through Queens with
        elevated sections offering views of surrounding neighborhoods and distant Manhattan skyline.
        Concrete barriers, overhead signs directing to terminals, and the AirTrain's elevated
        tracks characterize the infrastructure-heavy landscape. Airport hotels, gas stations,
        and car rental facilities line the service roads. The area pulses with the
        constant movement of taxis, buses, and travelers with luggage, creating
        a transitional zone between city and sky.
"""

    street, metroStation

let zone =
    let jamaicaZone = World.Zone.create Ids.Zone.jamaica

    let vanWyckExpressway, jfkMetro = vanWyckExpressway jamaicaZone

    let jfkMetroStation =
        { Lines = [ Blue ]
          LeavesToStreet = vanWyckExpressway.Id
          PlaceId = jfkMetro.Id }

    jamaicaZone
    |> World.Zone.addStreet (
        World.Node.create vanWyckExpressway.Id vanWyckExpressway
    )
    |> World.Zone.addMetroStation jfkMetroStation
