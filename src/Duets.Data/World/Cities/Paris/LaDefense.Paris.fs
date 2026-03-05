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
        |> World.Street.attachContext
            """
        The Esplanade de La Défense is the vast pedestrian axis at the heart of
        Europe's largest purpose-built business district. Stretching over a kilometre,
        it is an open-air museum of monumental contemporary sculpture—works by Calder,
        Miró, Torricini and dozens of other artists are installed permanently along
        its length. The Esplanade terminates at the Grande Arche de La Défense, a
        hollow marble cube aligned precisely with the Arc de Triomphe and the Louvre
        along the historic Grand Axe of Paris. By day it is one of the most animated
        public spaces in the region, with hundreds of thousands of office workers,
        tourists and Parisians crossing it; by night the illuminated towers create an
        extraordinary skyline that rivals any in the world.
"""

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
        |> World.Street.attachContext
            """
        Rue Bellini is a quieter side street in La Défense, running parallel to the
        Esplanade and threading between the bases of the district's glass towers.
        Unlike the monumental public spaces nearby, it has a more human scale,
        with street-level restaurants catering to office workers at lunch and a
        handful of specialist music and electronics shops. The street connects
        directly to the RER A and metro lines, making it one of the busiest
        pedestrian thoroughfares during rush hour despite its modest width.
        A small garden square at its northern end provides a rare patch of green
        within this otherwise entirely built environment.
"""

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
