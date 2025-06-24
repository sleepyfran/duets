module rec Duets.Data.World.Cities.MexicoCity.Polanco

open Duets.Data.World.Cities
open Duets.Entities

let private masaryk (zone: Zone) =
    let street = World.Street.create "Masaryk" (StreetType.Split(North, 2))

    let shops =
        [ ("Antara Fashion Hall", zone.Id); ("El Palacio de Hierro", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    let cafes =
        [ ("Maison Belén", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let metroStation =
        ("Polanco Station", zone.Id) |> PlaceCreators.createMetro street.Id

    street
    |> World.Street.addPlaces shops
    |> World.Street.addPlaces cafes
    |> World.Street.addPlace metroStation,
    metroStation

let private camposEliseos (zone: Zone) =
    let street =
        World.Street.create "Campos Elíseos" (StreetType.Split(West, 2))

    let hotels =
        [ ("Hyatt Regency", 90<quality>, 400m<dd>, zone.Id)
          ("InterContinental", 88<quality>, 350m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let bars =
        [ ("Jules Basement", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    street |> World.Street.addPlaces hotels |> World.Street.addPlaces bars

let private parqueLincoln (zone: Zone) =
    let street = World.Street.create "Parque Lincoln" StreetType.OneWay

    let concertSpaces =
        [ ("Auditorio Lunario",
           1000,
           85<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let zone =
    let zone = World.Zone.create "Polanco"
    let masaryk, metroStation = masaryk zone
    let camposEliseos = camposEliseos zone
    let parqueLincoln = parqueLincoln zone

    let zoneMetroStation =
        { Lines = [ Blue ]
          LeavesToStreet = masaryk.Id
          PlaceId = metroStation.Id }

    zone
    |> World.Zone.addStreet (World.Node.create masaryk.Id masaryk)
    |> World.Zone.addStreet (World.Node.create camposEliseos.Id camposEliseos)
    |> World.Zone.addStreet (World.Node.create parqueLincoln.Id parqueLincoln)
    |> World.Zone.addDescriptor Glitz
    |> World.Zone.addDescriptor BusinessDistrict
    |> World.Zone.addMetroStation zoneMetroStation
