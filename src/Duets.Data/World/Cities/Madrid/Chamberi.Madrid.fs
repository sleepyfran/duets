module rec Duets.Data.World.Cities.Madrid.Chamberi

open Duets.Entities
open Duets.Entities.Calendar
open Duets.Data.World.Cities

let callePonzano (city: City) (zone: Zone) =
    let street =
        World.Street.create "Calle de Ponzano" (StreetType.Split(West, 2))

    let bars =
        [ ("Fide", 80<quality>, zone.Id); ("El Doble", 82<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("Sala de Despiece", 86<quality>, French, zone.Id)
          ("Arima", 84<quality>, Japanese, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Toma Café", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let concertSpaces =
        [ ("Galileo Galilei",
           600,
           87<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id)
          ("Sala Clamores",
           500,
           84<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let radioStudios =
        [ ("Radio Clásica", 88<quality>, "Jazz", zone.Id) ]
        |> List.map (PlaceCreators.createRadioStudio city)

    let metroStation =
        ("Chamberí Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces cafes
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces radioStudios
        |> World.Street.addPlace metroStation

    street, metroStation

let calleAlmagro (zone: Zone) =
    let street =
        World.Street.create "Calle de Almagro" (StreetType.Split(North, 2))

    let studios =
        [ ("Estudio Almagro",
           83<quality>,
           210m<dd>,
           (Character.from
               "Rosalía"
               Female
               (Shorthands.Winter 25<days> 1992<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let rehearsalSpaces =
        [ ("Espacio Abierto", 78<quality>, 100m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let cafes =
        [ ("Café Comercial", 84<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let concertSpaces =
        [ ("Teatro Luchana",
           250,
           79<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ] // small
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street
    |> World.Street.addPlaces studios
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces concertSpaces

let calleFuencarral city (zone: Zone) =
    let street =
        World.Street.create "Calle de Fuencarral" (StreetType.Split(South, 2))

    let gyms =
        [ ("Basic-Fit", 80<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let bookstores =
        [ ("Librería Mujeres", 81<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let cafes =
        [ ("HanSo Café", 83<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlaces gyms
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces cafes

let createZone city =
    let chamberiZone = World.Zone.create "Chamberí"

    let callePonzano, metroStation = callePonzano city chamberiZone
    let calleAlmagro = calleAlmagro chamberiZone
    let calleFuencarral = calleFuencarral city chamberiZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = callePonzano.Id
          PlaceId = metroStation.Id }

    chamberiZone
    |> World.Zone.addStreet (World.Node.create callePonzano.Id callePonzano)
    |> World.Zone.addStreet (World.Node.create calleAlmagro.Id calleAlmagro)
    |> World.Zone.addStreet (
        World.Node.create calleFuencarral.Id calleFuencarral
    )
    |> World.Zone.connectStreets callePonzano.Id calleAlmagro.Id North
    |> World.Zone.connectStreets calleAlmagro.Id calleFuencarral.Id South
    |> World.Zone.addMetroStation station
    |> World.Zone.addDescriptor Bohemian
    |> World.Zone.addDescriptor Creative
