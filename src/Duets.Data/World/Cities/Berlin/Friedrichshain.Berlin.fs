module Duets.Data.World.Cities.Berlin.Friedrichshain

open Duets.Data.World.Cities.Berlin
open Duets.Data.World.Cities
open Duets.Entities

let warschauerStrasse (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.warschauerStrasse
            (StreetType.Split(East, 2))
        |> World.Street.tag "east-side-gallery"

    let concertSpaces =
        [ ("Berghain",
           1500,
           97<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id)
          ("Mercedes-Benz Arena",
           17000,
           95<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id)
          ("Astra Kulturhaus",
           1400,
           88<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Hops & Barley", 87<quality>, zone.Id)
          |> PlaceCreators.createBar street.Id
          |> World.Place.tag "pub"
          ("BarCom", 83<quality>, zone.Id)
          |> PlaceCreators.createBar street.Id ]

    let hotels =
        [ ("nhow Berlin", 88<quality>, 250m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let metroStation =
        ("S+U Warschauer Straße Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces hotels
        |> World.Street.addPlace metroStation

    street, metroStation

let simonDachStrasse (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.simonDachStrasse
            StreetType.OneWay

    let concertSpaces =
        [ ("Cassiopeia",
           150,
           85<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let cafes =
        [ ("Café Lois", 84<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let restaurants =
        [ ("Schnitzelei Friedrichshain", 87<quality>, German, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces restaurants

let createZone =
    let friedrichshainZone = World.Zone.create Ids.Zone.friedrichshain

    let warschauerStrasse, warschauerStation = warschauerStrasse friedrichshainZone
    let simonDachStrasse = simonDachStrasse friedrichshainZone

    let station =
        { Lines = [ Red ]
          LeavesToStreet = warschauerStrasse.Id
          PlaceId = warschauerStation.Id }

    friedrichshainZone
    |> World.Zone.addStreet (World.Node.create warschauerStrasse.Id warschauerStrasse)
    |> World.Zone.addStreet (World.Node.create simonDachStrasse.Id simonDachStrasse)
    |> World.Zone.connectStreets warschauerStrasse.Id simonDachStrasse.Id North
    |> World.Zone.addMetroStation station
