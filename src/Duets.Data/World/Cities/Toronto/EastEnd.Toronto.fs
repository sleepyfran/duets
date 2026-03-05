module rec Duets.Data.World.Cities.Toronto.EastEnd

open Duets.Data.World.Cities.Toronto
open Duets.Data.World.Cities
open Duets.Entities

let private danforthAvenue (city: City) (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.danforthAvenue
            (StreetType.Split(East, 2))

    let concerts =
        [ ("Danforth Music Hall",
           1500,
           89<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let restaurants =
        [ ("Asteria", 88<quality>, Italian, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Dark Horse Espresso", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let gyms =
        [ ("GoodLife Fitness", 82<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let metroStation =
        ("Broadview Station", zone.Id)
        |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces concerts
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces cafes
        |> World.Street.addPlaces gyms
        |> World.Street.addPlace metroStation
        |> World.Street.attachContext
            """
        Danforth Avenue, known as Greektown, is a lively east-end strip of
        restaurants, cafes, and the iconic Danforth Music Hall. Blue and white
        awnings mark Greek tavernas alongside newer additions from Toronto's
        multicultural food scene. The Bloor-Danforth subway rumbles beneath the
        street while families, joggers, and musicians share the wide sidewalks.
        The annual Taste of the Danforth festival transforms the avenue into one
        of the city's largest street parties.
"""

    street, metroStation

let private queenStreetEast (city: City) (zone: Zone) =
    let street =
        World.Street.create Ids.Street.queenStreetEast StreetType.OneWay

    let concerts =
        [ ("The Opera House",
           950,
           86<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("The Only Cafe", 83<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let radioStudios =
        [ ("Indie Radio Toronto", 86<quality>, "Indie", zone.Id) ]
        |> List.map (PlaceCreators.createRadioStudio city street.Id)

    let restaurants =
        [ ("Tabule", 85<quality>, Turkish, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    street
    |> World.Street.addPlaces concerts
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces radioStudios
    |> World.Street.addPlaces restaurants
    |> World.Street.attachContext
        """
    Queen Street East in Leslieville has transformed from a working-class strip
    into a hub of indie culture. The Opera House, a converted vaudeville theatre,
    hosts mid-size touring acts, while the surrounding blocks offer craft breweries,
    vintage furniture shops, and independent radio studios broadcasting from
    converted storefronts. The neighbourhood retains a gritty charm beneath its
    gentrifying surface.
"""

let createZone (city: City) =
    let eastEndZone = World.Zone.create Ids.Zone.eastEnd

    let danforthAvenue, broadviewMetro = danforthAvenue city eastEndZone
    let queenStreetEast = queenStreetEast city eastEndZone

    let broadviewMetroStation =
        { Lines = [ Blue ]
          LeavesToStreet = danforthAvenue.Id
          PlaceId = broadviewMetro.Id }

    eastEndZone
    |> World.Zone.addStreet (
        World.Node.create danforthAvenue.Id danforthAvenue
    )
    |> World.Zone.addStreet (
        World.Node.create queenStreetEast.Id queenStreetEast
    )
    |> World.Zone.connectStreets danforthAvenue.Id queenStreetEast.Id South
    |> World.Zone.addMetroStation broadviewMetroStation
