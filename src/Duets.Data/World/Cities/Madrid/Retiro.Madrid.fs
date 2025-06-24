module rec Duets.Data.World.Cities.Madrid.Retiro

open Duets.Entities
open Duets.Data.World.Cities

let parqueRetiro (zone: Zone) =
    let street =
        World.Street.create "Parque del Retiro" (StreetType.Split(East, 2))

    let cafes =
        [ ("Florida Retiro", 86<quality>, zone.Id)
          ("Café del Palacio de Cristal", 83<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let bookstores =
        [ ("Librería del Retiro", 80<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let concertSpaces =
        [ ("Teatro Monumental",
           500,
           84<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id)
          ("La Riviera", 2500, 90<quality>, Layouts.concertSpaceLayout2, zone.Id)
          ("Sala But", 900, 88<quality>, Layouts.concertSpaceLayout1, zone.Id)
          ("Sala Caracol",
           500,
           85<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let metroStation =
        ("Retiro Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let home =
        World.Place.create "Home" 100<quality> Home Layouts.homeLayout zone.Id

    let street =
        street
        |> World.Street.addPlaces cafes
        |> World.Street.addPlaces bookstores
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlace metroStation
        |> World.Street.addPlace home

    street, metroStation

let doctorEsquerdo city (zone: Zone) =
    let street =
        World.Street.create "Doctor Esquerdo" (StreetType.Split(South, 2))

    let hospital =
        ("Hospital General Universitario Gregorio Marañón", 88<quality>, zone.Id)
        |> PlaceCreators.createHospital street.Id

    let gyms =
        [ ("Retiro Fitness", 80<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let cafes =
        [ ("Café Esquerdo", 81<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlace hospital
    |> World.Street.addPlaces gyms
    |> World.Street.addPlaces cafes

let avenidaMenendezPelayo (zone: Zone) =
    let street =
        World.Street.create
            "Avenida de Menéndez Pelayo"
            (StreetType.Split(North, 2))

    let restaurants =
        [ ("Taberna La Dolores", 84<quality>, Spanish, zone.Id)
          ("La Castela", 87<quality>, French, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let bars =
        [ ("Bar Menéndez", 79<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    street |> World.Street.addPlaces restaurants |> World.Street.addPlaces bars

let createZone city =
    let retiroZone = World.Zone.create "Retiro"

    let parqueRetiro, metroStation = parqueRetiro retiroZone
    let doctorEsquerdo = doctorEsquerdo city retiroZone
    let avenidaMenendezPelayo = avenidaMenendezPelayo retiroZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = parqueRetiro.Id
          PlaceId = metroStation.Id }

    retiroZone
    |> World.Zone.addStreet (World.Node.create parqueRetiro.Id parqueRetiro)
    |> World.Zone.addStreet (World.Node.create doctorEsquerdo.Id doctorEsquerdo)
    |> World.Zone.addStreet (
        World.Node.create avenidaMenendezPelayo.Id avenidaMenendezPelayo
    )
    |> World.Zone.connectStreets parqueRetiro.Id doctorEsquerdo.Id South
    |> World.Zone.connectStreets
        doctorEsquerdo.Id
        avenidaMenendezPelayo.Id
        North
    |> World.Zone.addMetroStation station
    |> World.Zone.addDescriptor Nature
    |> World.Zone.addDescriptor Historic
