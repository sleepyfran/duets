module Duets.Data.World.Cities.Paris.LaDefense

open Duets.Data.World.Cities.Paris
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let esplanadeDeLaDefense (zone: Zone) (city: City) =
    let street =
        World.Street.create
            Ids.Street.esplanadeDeLaDefense
            (StreetType.Split(North, 2))
        |> World.Street.tag "pedestrian"

    let concertSpaces =
        [ ("Paris La Défense Arena",
           40000,
           97<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id)
          ("La Seine Musicale",
           6000,
           95<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let carDealers =
        [ ("Prestige Auto La Défense",
           95<quality>,
           zone.Id,
           { Dealer =
               (Character.from
                   "Alexandre Bertrand"
                   Male
                   (Shorthands.Winter 18<days> 1972<years>))
             PriceRange = CarPriceRange.Premium }) ]
        |> List.map (PlaceCreators.createCarDealer street.Id)

    let hotels =
        [ ("Pullman Paris La Défense", 91<quality>, 320m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let metroStation =
        ("La Défense/Grande Arche Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces carDealers
        |> World.Street.addPlaces hotels
        |> World.Street.addPlace metroStation

    street, metroStation

let rueBellini (zone: Zone) =
    let street =
        World.Street.create Ids.Street.rueBellini StreetType.OneWay

    let studios =
        [ ("Studio La Défense",
           88<quality>,
           560m<dd>,
           (Character.from
               "Julien Marchand"
               Male
               (Shorthands.Autumn 7<days> 1980<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let restaurants =
        [ ("Le Saigon de La Défense", 84<quality>, Vietnamese, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Café Coeur Défense", 82<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let bars =
        [ ("Le Piano Vache", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    street
    |> World.Street.addPlaces studios
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces bars

let createZone (city: City) =
    let laDefenseZone = World.Zone.create Ids.Zone.laDefense

    let esplanadeDeLaDefense, esplanadeStation =
        esplanadeDeLaDefense laDefenseZone city

    let rueBellini = rueBellini laDefenseZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = esplanadeDeLaDefense.Id
          PlaceId = esplanadeStation.Id }

    laDefenseZone
    |> World.Zone.addStreet (
        World.Node.create esplanadeDeLaDefense.Id esplanadeDeLaDefense
    )
    |> World.Zone.addStreet (World.Node.create rueBellini.Id rueBellini)
    |> World.Zone.connectStreets esplanadeDeLaDefense.Id rueBellini.Id East
    |> World.Zone.addMetroStation station
