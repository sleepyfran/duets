module rec Duets.Data.World.Cities.Sydney.Newtown

open Duets.Data.World.Cities
open Duets.Entities

let private kingStreet (zone: Zone) =
    let street = World.Street.create "King Street" (StreetType.Split(West, 2))

    let hotels =
        [ ("The Urban Newtown", 85<quality>, 500m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let restaurants =
        [ ("Lentil As Anything", 88<quality>, Mexican, zone.Id)
          ("Thai Pothong", 90<quality>, Vietnamese, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Brewtown Newtown", 87<quality>, zone.Id)
          ("Cuckoo Callay", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let bars =
        [ ("The Courthouse Hotel", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let bookstores =
        [ ("Better Read Than Dead", 89<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let metroStation =
        ("Newtown Station", zone.Id) |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces hotels
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces cafes
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces bookstores
        |> World.Street.addPlace metroStation

    street, metroStation

let private enmoreRoad (zone: Zone) =
    let street = World.Street.create "Enmore Road" StreetType.OneWay

    let concertSpaces =
        [ ("Enmore Theatre",
           1600,
           91<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id)
          ("The Vanguard",
           250,
           88<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id)
          ("Camelot Lounge",
           200,
           87<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id)
          ("Leadbelly", 350, 86<quality>, Layouts.concertSpaceLayout1, zone.Id)
          ("Lazybones Lounge",
           400,
           85<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let metroStation =
        ("Enmore Road Metro", zone.Id) |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlace metroStation

    street

let zone =
    let newtownZone = World.Zone.create "Newtown"
    let kingStreet, kingStreetMetro = kingStreet newtownZone
    let enmoreRoad = enmoreRoad newtownZone

    let kingStreetMetroStation =
        { Lines = [ Red ]
          LeavesToStreet = kingStreet.Id
          PlaceId = kingStreetMetro.Id }

    newtownZone
    |> World.Zone.addStreet (World.Node.create kingStreet.Id kingStreet)
    |> World.Zone.addStreet (World.Node.create enmoreRoad.Id enmoreRoad)
    |> World.Zone.connectStreets kingStreet.Id enmoreRoad.Id South
    |> World.Zone.addDescriptor Cultural
    |> World.Zone.addDescriptor EntertainmentHeart
    |> World.Zone.addMetroStation kingStreetMetroStation
