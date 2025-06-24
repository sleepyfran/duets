module rec Duets.Data.World.Cities.MexicoCity.Aeropuerto

open Duets.Data.World.Cities
open Duets.Entities

let private aeropuerto (zone: Zone) =
    let street = World.Street.create "Aeropuerto" StreetType.OneWay

    let airport =
        ("Aeropuerto Internacional Benito Juárez", 95<quality>, zone.Id)
        |> PlaceCreators.createAirport street.Id

    let metroStation =
        ("Aeropuerto Station", zone.Id) |> PlaceCreators.createMetro street.Id

    street
    |> World.Street.addPlace airport
    |> World.Street.addPlace metroStation,
    metroStation

let private terminalHotel (zone: Zone) =
    let street =
        World.Street.create "Terminal Hotelera" (StreetType.Split(North, 2))

    let hotels =
        [ ("NH Collection Airport", 85<quality>, 300m<dd>, zone.Id)
          ("Hilton Mexico City Airport", 88<quality>, 350m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    street |> World.Street.addPlaces hotels

let private avenidaOceanía (zone: Zone) =
    let street =
        World.Street.create "Avenida Oceanía" (StreetType.Split(East, 2))

    let cafes =
        [ ("Café Avión", 70<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street |> World.Street.addPlaces cafes

let zone =
    let zone = World.Zone.create "Aeropuerto"
    let aeropuerto, metroStation = aeropuerto zone
    let terminalHotel = terminalHotel zone
    let avenidaOceanía = avenidaOceanía zone

    let zoneMetroStation =
        { Lines = [ Red ]
          LeavesToStreet = aeropuerto.Id
          PlaceId = metroStation.Id }

    zone
    |> World.Zone.addStreet (World.Node.create aeropuerto.Id aeropuerto)
    |> World.Zone.addStreet (World.Node.create terminalHotel.Id terminalHotel)
    |> World.Zone.addStreet (World.Node.create avenidaOceanía.Id avenidaOceanía)
    |> World.Zone.addDescriptor BusinessDistrict
    |> World.Zone.addDescriptor Industrial
    |> World.Zone.addMetroStation zoneMetroStation
