module rec Duets.Data.World.Cities.LosAngeles.Venice

open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let private veniceBeachBoardwalk city (zone: Zone) =
    let street =
        World.Street.create
            "Venice Beach Boardwalk"
            (StreetType.Split(North, 3))

    let cafes =
        [ ("The Boardwalk Cafe", 75<quality>, zone.Id)
          ("Beachside Coffee Co.", 70<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let bars =
        [ ("Sidewalk Cafe", 72<quality>, zone.Id)
          ("The Venice Whaler", 78<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let gyms =
        [ ("Venice Gym", 80<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let restaurants =
        [ ("The Fig Tree", 74<quality>, Mexican, zone.Id)
          ("Poke Poke", 72<quality>, Japanese, zone.Id)
          ("The Waterfront", 76<quality>, American, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let concertSpaces =
        [ ("Venice Beach Music Stage",
           250,
           75<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let metroStation =
        ("Venice Beach Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces cafes
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces gyms
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlace metroStation

    street, metroStation

let private abbotKinney (zone: Zone) =
    let street =
        World.Street.create "Abbot Kinney Boulevard" (StreetType.Split(East, 2))

    let bookstores =
        [ ("Small World Books", 80<quality>, zone.Id)
          ("Beyond Baroque Literary Arts Center", 82<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let restaurants =
        [ ("Gjelina", 84<quality>, Italian, zone.Id)
          ("The Butcher's Daughter", 82<quality>, American, zone.Id)
          ("Salt & Straw", 79<quality>, American, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Intelligensia Coffee", 85<quality>, zone.Id)
          ("Blue Bottle Coffee", 83<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let bars =
        [ ("The Otheroom", 81<quality>, zone.Id)
          ("The Brig", 79<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let concertSpaces =
        [ ("The Venice Art Bar",
           175,
           80<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces concertSpaces

let private pacificAvenue (zone: Zone) =
    let street = World.Street.create "Pacific Avenue" StreetType.OneWay

    let studios =
        [ ("Venice Sound Studio",
           82<quality>,
           300m<dd>,
           (Character.from
               "Sarah McLachlan"
               Female
               (Shorthands.Winter 28<days> 1968<years>)),
           zone.Id)
          ("Beach Beats Recording",
           80<quality>,
           280m<dd>,
           (Character.from
               "John Doe"
               Male
               (Shorthands.Winter 25<days> 1977<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let rehearsalSpaces =
        [ ("The Canal Club", 78<quality>, 200m<dd>, zone.Id)
          ("Venice Underground", 75<quality>, 180m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)


    let concertSpaces =
        [ ("The Townhouse & Del Monte Speakeasy",
           200,
           77<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let hotels =
        [ ("Venice Beach Suites", 74<quality>, 200m<dd>, zone.Id)
          ("Hotel Erwin", 79<quality>, 240m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    street
    |> World.Street.addPlaces studios
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces hotels

let createZone city =
    let veniceZone = World.Zone.create "Venice"

    let veniceBeachBoardwalk, metroStation =
        veniceBeachBoardwalk city veniceZone

    let abbotKinney = abbotKinney veniceZone
    let pacificAvenue = pacificAvenue veniceZone

    let metroStation =
        { Lines = [ Blue ]
          LeavesToStreet = veniceBeachBoardwalk.Id
          PlaceId = metroStation.Id }

    veniceZone
    |> World.Zone.addStreet (
        World.Node.create veniceBeachBoardwalk.Id veniceBeachBoardwalk
    )
    |> World.Zone.addStreet (World.Node.create abbotKinney.Id abbotKinney)
    |> World.Zone.addStreet (World.Node.create pacificAvenue.Id pacificAvenue)
    |> World.Zone.connectStreets veniceBeachBoardwalk.Id abbotKinney.Id East
    |> World.Zone.connectStreets abbotKinney.Id pacificAvenue.Id South
    |> World.Zone.addDescriptor Bohemian
    |> World.Zone.addDescriptor Creative
    |> World.Zone.addDescriptor Coastal
    |> World.Zone.addMetroStation metroStation
