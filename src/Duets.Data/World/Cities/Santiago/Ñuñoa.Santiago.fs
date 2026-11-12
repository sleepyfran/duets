module Duets.Data.World.Cities.Santiago.Ñuñoa

open Duets.Data.World.Cities.Santiago
open Duets.Data.World.Cities
open Duets.Entities

let avenidaIrarrázaval (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.avenidaIrarrázaval
            (StreetType.Split(East, 2))

    let restaurants =
        [ ("Fuente Suiza", 84<quality>, Chilean, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Juan Valdez Café Ñuñoa", 78<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let rehearsalSpaces =
        [ ("Sala de Ensayo Irarrázaval", 80<quality>, 90m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let metroStation =
        ("Ñuñoa Metro Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces cafes
        |> World.Street.addPlaces rehearsalSpaces
        |> World.Street.addPlace metroStation

    street, metroStation

let plazaÑuñoa (zone: Zone) =
    let street =
        World.Street.create Ids.Street.plazaÑuñoa (StreetType.Split(North, 2))
        |> World.Street.tag "pedestrian"

    let concertSpaces =
        [ ("La Batuta", 400, 82<quality>, Layouts.concertSpaceLayout2, zone.Id)
          |> PlaceCreators.createConcertSpace street.Id
          ("Teatro UC", 550, 88<quality>, Layouts.concertSpaceLayout3, zone.Id)
          |> PlaceCreators.createConcertSpace street.Id
          |> World.Place.tag "seated" ]

    let bars =
        [ ("Las Lanzas", 82<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces bars

let avenidaGrecia (city: City) (zone: Zone) =
    let street =
        World.Street.create Ids.Street.avenidaGrecia StreetType.OneWay

    let concertSpaces =
        [ ("Estadio Nacional Julio Martínez Prádanos",
           50000,
           82<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let gyms =
        [ ("Smart Fit Ñuñoa", 76<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces gyms

let createZone (city: City) =
    let ñuñoaZone = World.Zone.create Ids.Zone.ñuñoa

    let avenidaIrarrázaval, metroStation = avenidaIrarrázaval ñuñoaZone
    let plazaÑuñoa = plazaÑuñoa ñuñoaZone
    let avenidaGrecia = avenidaGrecia city ñuñoaZone

    let station =
        { Lines = [ Red ]
          LeavesToStreet = avenidaIrarrázaval.Id
          PlaceId = metroStation.Id }

    ñuñoaZone
    |> World.Zone.addStreet (
        World.Node.create avenidaIrarrázaval.Id avenidaIrarrázaval
    )
    |> World.Zone.addStreet (World.Node.create plazaÑuñoa.Id plazaÑuñoa)
    |> World.Zone.addStreet (World.Node.create avenidaGrecia.Id avenidaGrecia)
    |> World.Zone.connectStreets avenidaIrarrázaval.Id plazaÑuñoa.Id North
    |> World.Zone.connectStreets avenidaIrarrázaval.Id avenidaGrecia.Id South
    |> World.Zone.addMetroStation station
