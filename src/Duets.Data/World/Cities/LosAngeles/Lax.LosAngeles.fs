module Duets.Data.World.Cities.LosAngeles.Lax

open Duets.Data.World.Cities.LosAngeles
open Duets.Data.World.Cities
open Duets.Entities

let centuryBoulevard (zone: Zone) =
    let street =
        World.Street.create Ids.Street.centuryBoulevard StreetType.OneWay
        |> World.Street.attachContext
            """
        Century Boulevard is a major, sprawling east-west thoroughfare in Los Angeles,
        famously dubbed "The Gateway to Los Angeles" as its western end leads directly
        to the passenger terminals of Los Angeles International Airport (LAX).
        As a primary artery, the street is dominated by a heavy concentration of tall,
        modern airport hotels (such as Marriott, Hyatt, and Hilton properties),
        rental car agencies, and other airport-support businesses. Further east,
        particularly as it enters Inglewood, the street is framed by major entertainment
        and sports landmarks like SoFi Stadium and the Intuit Dome, as well as
        the nearby Hollywood Park Casino and the site of the former Hollywood Park Racetrack.
        The street is a non-stop nexus of high-traffic transit, featuring wide lanes
        and the elevated tracks of the Metro Lines.
"""

    let airports =
        [ ("Los Angeles International Airport", 95<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createAirport street.Id)

    let hotels =
        [ ("Century Park Suites", 75<quality>, 200m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let metroStation =
        ("LAX/Metro Center", zone.Id) |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces airports
        |> World.Street.addPlaces hotels
        |> World.Street.addPlace metroStation

    street, metroStation

let zone =
    let laxZone = World.Zone.create Ids.Zone.lax

    let centuryBoulevard, centuryBoulevardStation = centuryBoulevard laxZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = centuryBoulevard.Id
          PlaceId = centuryBoulevardStation.Id }

    laxZone
    |> World.Zone.addStreet (
        World.Node.create centuryBoulevard.Id centuryBoulevard
    )
    |> World.Zone.addMetroStation station
