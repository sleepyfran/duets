module rec Duets.Data.World.Cities.NewYork.Brooklyn

open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let private lafayetteAtlantic (city: City) (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.lafayetteAtlantic
            (StreetType.Split(East, 3))

    let gyms =
        [ ("Blink Fitness Brooklyn", 84<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let concerts =
        [ ("Barclays Center",
           19000,
           96<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id)
          |> PlaceCreators.createConcertSpace street.Id
          ("BAM (Brooklyn Academy of Music)",
           2100,
           92<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id)
          |> PlaceCreators.createConcertSpace street.Id
          |> World.Place.tag "seated" ]

    let bars =
        [ ("baba cool", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("Bacchus", 88<quality>, French, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Hungry Ghost Coffee", 84<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let cinemas =
        [ ("Regal UA Court Street", 89<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCinema city.Id street.Id)

    let metroStation =
        ("Atlantic Avenue-Barclays Center", zone.Id)
        |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces gyms
        |> World.Street.addPlaces concerts
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces cafes
        |> World.Street.addPlaces cinemas
        |> World.Street.addPlace metroStation

    street, metroStation

let private frostStreet (zone: Zone) =
    let street = World.Street.create Ids.Street.frostStreet StreetType.OneWay

    let home = PlaceCreators.createHome street.Id zone.Id

    // TODO: Add Frost Playground once we support parks
    // TODO: Add Cooper Park once we support parks

    let concerts =
        [ ("Brooklyn Steel",
           1800,
           90<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concerts

let private bedfordAvenue (zone: Zone) =
    let street = World.Street.create Ids.Street.bedfordAvenue StreetType.OneWay

    let carDealers =
        [ ("Williamsburg Motors",
           80<quality>,
           zone.Id,
           { Dealer =
               (Character.from
                   "Alex Rivera"
                   Male
                   (Shorthands.Summer 10<days> 1983<years>))
             PriceRange = CarPriceRange.MidRange }) ]
        |> List.map (PlaceCreators.createCarDealer street.Id)

    let concerts =
        [ ("The Knitting Factory",
           150,
           87<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let rehearsalSpaces =
        [ ("Music Building", 88<quality>, 175m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let cafes =
        [ ("Toby's Estate Coffee", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlaces carDealers
    |> World.Street.addPlaces concerts
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces cafes

let zone city =
    let brooklynZone = World.Zone.create Ids.Zone.brooklyn

    let lafayetteAtlantic, atlanticMetro = lafayetteAtlantic city brooklynZone
    let frostStreet = frostStreet brooklynZone
    let bedfordAvenue = bedfordAvenue brooklynZone

    let atlanticMetroStation =
        { Lines = [ Blue ]
          LeavesToStreet = lafayetteAtlantic.Id
          PlaceId = atlanticMetro.Id }

    brooklynZone
    |> World.Zone.addStreet (
        World.Node.create lafayetteAtlantic.Id lafayetteAtlantic
    )
    |> World.Zone.addStreet (World.Node.create frostStreet.Id frostStreet)
    |> World.Zone.addStreet (World.Node.create bedfordAvenue.Id bedfordAvenue)
    |> World.Zone.connectStreets lafayetteAtlantic.Id frostStreet.Id North
    |> World.Zone.connectStreets lafayetteAtlantic.Id bedfordAvenue.Id East
    |> World.Zone.addMetroStation atlanticMetroStation
