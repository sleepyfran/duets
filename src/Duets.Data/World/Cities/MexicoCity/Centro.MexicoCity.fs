module rec Duets.Data.World.Cities.MexicoCity.Centro

open Duets.Data.World.Cities
open Duets.Entities

let private madero (zone: Zone) =
    let street = World.Street.create "Madero" (StreetType.Split(East, 2))

    let cafes =
        [ ("Café de Tacuba", 85<quality>, zone.Id)
          ("El Cardenal", 80<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let bookstores =
        [ ("Gandhi Centro", 80<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let concertSpaces =
        [ ("Teatro de la Ciudad",
           1300,
           90<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let metroStation =
        ("Centro Station", zone.Id) |> PlaceCreators.createMetro street.Id

    street
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlace metroStation,
    metroStation

let private zocalo (zone: Zone) =
    let street = World.Street.create "Zócalo" (StreetType.Split(South, 2))

    let bars =
        [ ("Bar La Ópera", 80<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let rehearsalSpaces =
        [ ("Casa de la Música", 75<quality>, 180m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    street
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces rehearsalSpaces

let private hospital (zone: Zone) =
    let street = World.Street.create "Hospital General" StreetType.OneWay

    let hospital =
        ("Hospital General de México", 95<quality>, zone.Id)
        |> PlaceCreators.createHospital street.Id

    street |> World.Street.addPlace hospital

let zone =
    let zone = World.Zone.create "Centro"
    let madero, metroStation = madero zone
    let zocalo = zocalo zone
    let hospital = hospital zone

    let zoneMetroStation =
        { Lines = [ Blue; Red ]
          LeavesToStreet = madero.Id
          PlaceId = metroStation.Id }

    zone
    |> World.Zone.addStreet (World.Node.create madero.Id madero)
    |> World.Zone.addStreet (World.Node.create zocalo.Id zocalo)
    |> World.Zone.addStreet (World.Node.create hospital.Id hospital)
    |> World.Zone.addDescriptor Historic
    |> World.Zone.addDescriptor EntertainmentHeart
    |> World.Zone.addMetroStation zoneMetroStation
