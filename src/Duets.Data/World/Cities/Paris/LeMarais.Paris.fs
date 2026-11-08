module Duets.Data.World.Cities.Paris.LeMarais

open Duets.Data.World.Cities.Paris
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let rueDeRivoli (zone: Zone) (city: City) =
    let street =
        World.Street.create
            Ids.Street.rueDeRivoli
            (StreetType.Split(East, 3))
        |> World.Street.tag "rue-de-rivoli"

    let concertSpaces =
        [ ("Café de la Danse",
           400,
           87<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id)
          ("Le Bataclan",
           1500,
           89<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let casinos =
        [ ("Casino Barrière Paris", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCasino street.Id)

    let carDealers =
        [ ("Marais Auto",
           80<quality>,
           zone.Id,
           { Dealer =
               (Character.from
                   "Éric Fontaine"
                   Male
                   (Shorthands.Autumn 12<days> 1978<years>))
             PriceRange = CarPriceRange.MidRange }) ]
        |> List.map (PlaceCreators.createCarDealer street.Id)

    let metroStation =
        ("Hôtel de Ville Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces casinos
        |> World.Street.addPlaces carDealers
        |> World.Street.addPlace metroStation

    street, metroStation

let rueDesFrancsBourgeois (zone: Zone) (city: City) =
    let street =
        World.Street.create Ids.Street.rueDesFrancsBourgeois StreetType.OneWay

    let restaurants =
        [ ("L'As du Fallafel", 88<quality>, French, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Café Charlot", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let bookstores =
        [ ("La Belle Hortense", 87<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let gyms =
        [ ("Fitness Club Marais", 83<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    street
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces gyms

let rueVieilleDuTemple (zone: Zone) =
    let street =
        World.Street.create Ids.Street.rueVieilleDuTemple StreetType.OneWay

    let bars =
        [ ("Andy Wahloo", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let studios =
        [ ("Studio des Marais",
           85<quality>,
           480m<dd>,
           (Character.from
               "Sophie Laurent"
               Female
               (Shorthands.Spring 22<days> 1982<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let rehearsalSpaces =
        [ ("Le Studio Répétition", 84<quality>, 320m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let hotels =
        [ ("Hôtel du Petit Moulin", 91<quality>, 290m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    street
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces studios
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces hotels

let createZone (city: City) =
    let leMaraisZone = World.Zone.create Ids.Zone.leMarais

    let rueDeRivoli, rivoliStation = rueDeRivoli leMaraisZone city
    let rueDesFrancsBourgeois = rueDesFrancsBourgeois leMaraisZone city
    let rueVieilleDuTemple = rueVieilleDuTemple leMaraisZone

    let station =
        { Lines = [ Red ]
          LeavesToStreet = rueDeRivoli.Id
          PlaceId = rivoliStation.Id }

    leMaraisZone
    |> World.Zone.addStreet (World.Node.create rueDeRivoli.Id rueDeRivoli)
    |> World.Zone.addStreet (
        World.Node.create rueDesFrancsBourgeois.Id rueDesFrancsBourgeois
    )
    |> World.Zone.addStreet (
        World.Node.create rueVieilleDuTemple.Id rueVieilleDuTemple
    )
    |> World.Zone.connectStreets rueDeRivoli.Id rueDesFrancsBourgeois.Id North
    |> World.Zone.connectStreets rueDesFrancsBourgeois.Id rueVieilleDuTemple.Id East
    |> World.Zone.addMetroStation station
