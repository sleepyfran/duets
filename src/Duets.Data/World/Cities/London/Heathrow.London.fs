module rec Duets.Data.World.Cities.London.Heathrow

open Duets.Data.World.Cities
open Duets.Entities

let private heathrowAirport (zone: Zone) =
    let street = World.Street.create "Heathrow" StreetType.OneWay

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
    let street = World.Street.create "Bath Road" (StreetType.Split(West, 2))

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
