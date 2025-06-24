module rec Duets.Data.World.Cities.Sydney.Mascot

open Duets.Data.World.Cities
open Duets.Entities

let private oRiordanStreet (zone: Zone) =
    let street =
        World.Street.create "O'Riordan Street" (StreetType.Split(South, 2))

    let airport =
        ("Sydney Airport", 90<quality>, zone.Id)
        |> (PlaceCreators.createAirport street.Id)

    let hotels =
        [ ("Stamford Plaza Sydney Airport", 86<quality>, 600m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let cafes =
        [ ("Café Two Black Sheep", 84<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let metroStation =
        ("Mascot Metro", zone.Id) |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlace airport
        |> World.Street.addPlaces hotels
        |> World.Street.addPlaces cafes
        |> World.Street.addPlace metroStation

    street, metroStation

let zone =
    let mascotZone = World.Zone.create "Mascot"
    let oRiordanStreet, oRiordanStreetMetro = oRiordanStreet mascotZone

    let oRiordanStreetMetroStation =
        { Lines = [ Blue ]
          LeavesToStreet = oRiordanStreet.Id
          PlaceId = oRiordanStreetMetro.Id }

    mascotZone
    |> World.Zone.addStreet (World.Node.create oRiordanStreet.Id oRiordanStreet)
    |> World.Zone.addDescriptor Industrial
    |> World.Zone.addDescriptor Nature
    |> World.Zone.addMetroStation oRiordanStreetMetroStation
