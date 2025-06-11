module rec Duets.Data.World.Cities.NewYork.Soho

open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let private westBroadway (zone: Zone) =
    let street = World.Street.create "West Broadway" (StreetType.Split(North, 3))

    let home = PlaceCreators.createHome street.Id zone.Id

    let cafes =
        [ ("The Roasted Bean", 92<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let restaurants =
        [ ("Balthazar", 95<quality>, French, zone.Id)
          ("Fanelli Cafe", 85<quality>, American, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let bookstores =
        [ ("Housing Works Bookstore Cafe", 89<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let hotels =
        [ ("The Mercer", 96<quality>, 500m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let metroStation =
        ("SoHo Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlace home
        |> World.Street.addPlaces cafes
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces bookstores
        |> World.Street.addPlaces hotels
        |> World.Street.addPlace metroStation

    street, metroStation

let private princeStreet (zone: Zone) =
    let street = World.Street.create "Prince Street" (StreetType.Split(East, 2))

    let rehearsalSpaces =
        [ ("Music Cellar", 85<quality>, 100m<dd>, zone.Id)
          ("SoHo Sound", 87<quality>, 120m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let restaurants =
        [ ("Carbone", 90<quality>, Italian, zone.Id)
          ("Turkish Delight", 86<quality>, Turkish, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let studios =
        [ ("SoHo Records",
           85<quality>,
           200m<dd>,
           (Character.from
               "John Smith"
               Male
               (Shorthands.Winter 24<days> 1975<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    street
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces studios

let zone =
    let sohoZone = World.Zone.create "SoHo"

    let westBroadway, metroStation = westBroadway sohoZone
    let princeStreet = princeStreet sohoZone

    let metroStation =
        { Lines = [ Blue ]
          LeavesToStreet = westBroadway.Id
          PlaceId = metroStation.Id }

    sohoZone
    |> World.Zone.addStreet (World.Node.create westBroadway.Id westBroadway)
    |> World.Zone.addStreet (World.Node.create princeStreet.Id princeStreet)
    |> World.Zone.connectStreets westBroadway.Id princeStreet.Id East
    |> World.Zone.addDescriptor Creative
    |> World.Zone.addDescriptor BusinessDistrict
    |> World.Zone.addMetroStation metroStation
