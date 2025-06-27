module rec Duets.Data.World.Cities.Prague.Holešovice

open Duets.Data.World.Cities
open Duets.Entities

let plynární (zone: Zone) =
    let street = World.Street.create "Plynární" (StreetType.Split(West, 2))

    let concertSpaces =
        [ ("Cross Club", 400, 86<quality>, Layouts.concertSpaceLayout3, zone.Id)
          ("La Fabrika", 800, 85<quality>, Layouts.concertSpaceLayout2, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Láhev Sud", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("SaSaZu", 90<quality>, Japanese, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let metroStation =
        ("Nádraží Holešovice Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlace metroStation

    street, metroStation

let komuniní (zone: Zone) =
    let street = World.Street.create "Komunardů" (StreetType.Split(North, 2))

    let concertSpaces =
        [ ("DOX Centre for Contemporary Art",
           600,
           84<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Bar Cobra", 87<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("Home Kitchen", 85<quality>, French, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Vnitroblock Café", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces cafes

let zaElektrarnou (zone: Zone) =
    let street =
        World.Street.create "Za Elektrárnou" (StreetType.Split(East, 2))

    let concertSpaces =
        [ ("Tipsport Arena",
           13000,
           88<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Restaurace U Výstaviště", 82<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("Bistro Stromovka", 85<quality>, Czech, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces restaurants

let zone =
    let holešoviceZone = World.Zone.create "Holešovice"

    let plynární, metroStation = plynární holešoviceZone
    let komunardů = komuniní holešoviceZone
    let zaElektrarnou = zaElektrarnou holešoviceZone

    let station =
        { Lines = [ Red ]
          LeavesToStreet = plynární.Id
          PlaceId = metroStation.Id }

    holešoviceZone
    |> World.Zone.addStreet (World.Node.create plynární.Id plynární)
    |> World.Zone.addStreet (World.Node.create komunardů.Id komunardů)
    |> World.Zone.addStreet (World.Node.create zaElektrarnou.Id zaElektrarnou)
    |> World.Zone.connectStreets plynární.Id komunardů.Id South
    |> World.Zone.connectStreets komunardů.Id zaElektrarnou.Id East
    |> World.Zone.addMetroStation station
