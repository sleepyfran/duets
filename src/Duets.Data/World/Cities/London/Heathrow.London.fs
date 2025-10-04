module rec Duets.Data.World.Cities.London.Heathrow

open Duets.Data.World.Cities
open Duets.Entities

let private heathrowAirport (zone: Zone) =
    let street =
        World.Street.create "Heathrow" StreetType.OneWay
        |> World.Street.attachContext
            """
        Heathrow Airport sprawls across 12.27 square kilometers on London's western edge,
        its five terminals handling over 80 million passengers annually. The constant roar
        of aircraft taking off and landing creates a distinctive soundscape. Terminal buildings
        blend brutalist concrete with modern glass and steel expansions. Signs in dozens of
        languages guide international travelers through the complex. The airport tunnel road
        system channels vehicles between terminals beneath the runways. Despite the functional
        architecture, the area pulses with the energy of global movement, as business travelers,
        tourists, and airline crews flow through 24 hours a day.
"""

    let airport =
        ("Heathrow Airport", 100<quality>, zone.Id)
        |> PlaceCreators.createAirport street.Id

    let metroStation =
        ("Heathrow Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlace airport
        |> World.Street.addPlace metroStation

    street, metroStation

let private bathRoad (zone: Zone) =
    let street =
        World.Street.create "Bath Road" (StreetType.Split(West, 2))
        |> World.Street.attachContext
            """
        Bath Road runs parallel to Heathrow's northern perimeter, a functional corridor
        dominated by airport hotels, car rental offices, and logistics facilities. The A4
        road carries constant traffic between central London and the airport terminals.
        Aircraft pass low overhead every few minutes on their final approach, their landing
        gear visible from the street. Modern hotel blocks in corporate architectural styles
        line the route, interspersed with older buildings that predate the airport's expansion.
        The atmosphere is transient and utilitarian, serving the needs of travelers in transit
        rather than fostering community life.
"""

    let hotels =
        [ ("Sofitel London Heathrow", 85<quality>, 350m<dd>, zone.Id)
          ("Hilton Garden Inn", 80<quality>, 300m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let cafes =
        [ ("Costa Coffee", 70<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street |> World.Street.addPlaces hotels |> World.Street.addPlaces cafes

let zone =
    let zone = World.Zone.create "Heathrow"
    let heathrow, metroStation = heathrowAirport zone
    let bathRoad = bathRoad zone

    let metroStation =
        { Lines = [ Blue ]
          LeavesToStreet = heathrow.Id
          PlaceId = metroStation.Id }

    zone
    |> World.Zone.addStreet (World.Node.create heathrow.Id heathrow)
    |> World.Zone.addStreet (World.Node.create bathRoad.Id bathRoad)
    |> World.Zone.addDescriptor BusinessDistrict
    |> World.Zone.addDescriptor Industrial
    |> World.Zone.addMetroStation metroStation
