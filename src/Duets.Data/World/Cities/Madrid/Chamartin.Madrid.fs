module rec Duets.Data.World.Cities.Madrid.Chamartin

open Duets.Entities
open Duets.Data.World.Cities

let paseoCastellana city (zone: Zone) =
    let street =
        World.Street.create
            "Paseo de la Castellana"
            (StreetType.Split(North, 3))

    let hotels =
        [ ("Hotel Eurostars Madrid Tower", 93<quality>, 400m<dd>, zone.Id)
          ("Hotel Chamartín", 85<quality>, 260m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let restaurants =
        [ ("La Vaca Argentina", 87<quality>, French, zone.Id)
          ("Goiko Grill", 84<quality>, American, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let gyms =
        [ ("Basic-Fit Castellana", 81<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let concertSpaces =
        [ ("Moby Dick Club",
           400,
           85<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id)
          ("Sala Mon", 900, 87<quality>, Layouts.concertSpaceLayout1, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let metroStation =
        ("Chamartín Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces hotels
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces gyms
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlace metroStation

    street, metroStation

let avenidaAmerica (zone: Zone) =
    let street =
        World.Street.create "Avenida de América" (StreetType.Split(East, 2))

    let cafes =
        [ ("Café América", 80<quality>, zone.Id)
          ("Café Murillo", 82<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let bookstores =
        [ ("Librería América", 78<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    street |> World.Street.addPlaces cafes |> World.Street.addPlaces bookstores

// let aeropuerto (zone: Zone) =
//     let street =
//         World.Street.create "Aeropuerto" (StreetType.Split(NorthEast, 1))
//     let airport =
//         ("Aeropuerto Adolfo Suárez Madrid-Barajas", 90<quality>, zone.Id)
//         |> PlaceCreators.createAirport street.Id
//     street |> World.Street.addPlace airport

let zonaBernabeu (zone: Zone) =
    let street = World.Street.create "Zona Bernabéu" (StreetType.Split(West, 2))

    let restaurants =
        [ ("Asador Donostiarra", 88<quality>, Spanish, zone.Id)
          ("La Esquina", 84<quality>, Italian, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let bars =
        [ ("La Esquina Bar", 80<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let concertSpaces =
        [ ("Estadio Santiago Bernabéu",
           70000,
           98<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) // stadium
          ("Palacio de Deportes de la Comunidad",
           5000,
           92<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) // large
          ("Palacio de Vistalegre",
           15000,
           90<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ] // arena
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces concertSpaces

let createZone city =
    let chamartinZone = World.Zone.create "Chamartín"

    let paseoCastellana, metroStation = paseoCastellana city chamartinZone
    let avenidaAmerica = avenidaAmerica chamartinZone
    let zonaBernabeu = zonaBernabeu chamartinZone

    let station =
        { Lines = [ Blue; Red ]
          LeavesToStreet = paseoCastellana.Id
          PlaceId = metroStation.Id }

    chamartinZone
    |> World.Zone.addStreet (
        World.Node.create paseoCastellana.Id paseoCastellana
    )
    |> World.Zone.addStreet (World.Node.create avenidaAmerica.Id avenidaAmerica)
    |> World.Zone.addStreet (World.Node.create zonaBernabeu.Id zonaBernabeu)
    |> World.Zone.connectStreets paseoCastellana.Id avenidaAmerica.Id East
    |> World.Zone.connectStreets paseoCastellana.Id zonaBernabeu.Id West
    |> World.Zone.addMetroStation station
    |> World.Zone.addDescriptor BusinessDistrict
