module Duets.Data.World.Cities.LosAngeles.Hollywood

open Duets.Data.World.Cities.LosAngeles
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let sunsetBoulevard (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.sunsetBoulevardHollywood
            (StreetType.Split(East, 3))

    let concertSpaces =
        [ ("The Roxy Theatre",
           500,
           88<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id)
          ("Whisky a Go Go",
           500,
           86<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id)
          ("The Viper Room",
           250,
           84<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id)
          ("The Troubadour",
           400,
           87<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("The Rainbow Bar & Grill", 89<quality>, zone.Id)
          ("SkyBar at Mondrian Hotel", 92<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let studios =
        [ ("Nightbird Studios",
           90<quality>,
           700m<dd>,
           (Character.from
               "David Bianco"
               Male
               (Shorthands.Spring 15<days> 1965<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let hotels =
        [ ("The Dreamcatcher Inn", 85<quality>, 350m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let metroStation =
        ("Hollywood/Vine Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces studios
        |> World.Street.addPlaces hotels
        |> World.Street.addPlace metroStation

    street, metroStation

let hollywoodBoulevard (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.hollywoodBoulevard
            (StreetType.Split(East, 2))

    let concertSpaces =
        [ ("Hollywood Palladium",
           3700,
           91<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id)
          ("The Fonda Theatre",
           1300,
           88<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id)
          ("Avalon Hollywood",
           1500,
           87<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id)
          ("Dolby Theatre",
           3400,
           94<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id)
          ("The Hotel Café",
           100,
           85<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let merchandiseWorkshops =
        [ ("Amoeba Music", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    let hotels =
        [ ("The Gold Standard", 95<quality>, 500m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces merchandiseWorkshops
    |> World.Street.addPlaces hotels

let zone =
    let hollywoodZone = World.Zone.create Ids.Zone.hollywood

    let sunsetBoulevard, sunsetBoulevardStation = sunsetBoulevard hollywoodZone
    let hollywoodBoulevard = hollywoodBoulevard hollywoodZone

    let station =
        { Lines = [ Red ]
          LeavesToStreet = sunsetBoulevard.Id
          PlaceId = sunsetBoulevardStation.Id }

    hollywoodZone
    |> World.Zone.addStreet (
        World.Node.create sunsetBoulevard.Id sunsetBoulevard
    )
    |> World.Zone.addStreet (
        World.Node.create hollywoodBoulevard.Id hollywoodBoulevard
    )
    |> World.Zone.connectStreets sunsetBoulevard.Id hollywoodBoulevard.Id North
    |> World.Zone.addMetroStation station
