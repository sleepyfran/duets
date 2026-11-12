module Duets.Data.World.Cities.Paris.SaintGermain

open Duets.Data.World.Cities.Paris
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let boulevardSaintGermain (zone: Zone) (cityId: CityId) (city: City) =
    let street =
        World.Street.create
            Ids.Street.boulevardSaintGermain
            (StreetType.Split(East, 2))

    let concertSpaces =
        [ ("Le Caveau de la Huchette",
           200,
           88<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (
            PlaceCreators.createConcertSpace street.Id
            >> World.Place.tag "jazz-club"
        )

    let cafes =
        [ ("Café de Flore", 92<quality>, zone.Id)
          ("Les Deux Magots", 91<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let radioStudios =
        [ ("Radio France Studios", 92<quality>, "Jazz", zone.Id) ]
        |> List.map (PlaceCreators.createRadioStudio city street.Id)

    let restaurants =
        [ ("Brasserie Lipp", 91<quality>, French, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let metroStation =
        ("Saint-Germain-des-Prés Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces radioStudios
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces cafes
        |> World.Street.addPlace metroStation

    street, metroStation

let rueDeSeine (zone: Zone) =
    let street =
        World.Street.create Ids.Street.rueDeSeine StreetType.OneWay

    let cafes =
        [ ("Café de la Mairie", 84<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let bars =
        [ ("Prescription Cocktail Club", 89<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id >> World.Place.tag "cocktail-bar")

    let merchandiseWorkshops =
        [ ("FNAC Saint-Germain", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    let hotels =
        [ ("Hôtel d'Aubusson", 93<quality>, 380m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    street
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces merchandiseWorkshops
    |> World.Street.addPlaces hotels

let rueDuBac (zone: Zone) (city: City) =
    let street =
        World.Street.create Ids.Street.rueDuBac StreetType.OneWay

    let hospitals =
        [ ("Hôpital Laennec", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createHospital street.Id)

    let rehearsalSpaces =
        [ ("La Fabrique Sonore", 86<quality>, 340m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let carDealers =
        [ ("Véhicules du Bac",
           70<quality>,
           zone.Id,
           { Dealer =
               (Character.from
                   "Pierre Morel"
                   Male
                   (Shorthands.Summer 5<days> 1975<years>))
             PriceRange = CarPriceRange.Budget }) ]
        |> List.map (PlaceCreators.createCarDealer street.Id)

    let gyms =
        [ ("Club Med Gym Rive Gauche", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    street
    |> World.Street.addPlaces hospitals
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces carDealers
    |> World.Street.addPlaces gyms

let createZone (cityId: CityId) (city: City) =
    let saintGermainZone = World.Zone.create Ids.Zone.saintGermain

    let boulevardSaintGermain, saintGermainStation =
        boulevardSaintGermain saintGermainZone cityId city

    let rueDeSeine = rueDeSeine saintGermainZone
    let rueDuBac = rueDuBac saintGermainZone city

    let station =
        { Lines = [ Red; Blue ]
          LeavesToStreet = boulevardSaintGermain.Id
          PlaceId = saintGermainStation.Id }

    saintGermainZone
    |> World.Zone.addStreet (
        World.Node.create boulevardSaintGermain.Id boulevardSaintGermain
    )
    |> World.Zone.addStreet (World.Node.create rueDeSeine.Id rueDeSeine)
    |> World.Zone.addStreet (World.Node.create rueDuBac.Id rueDuBac)
    |> World.Zone.connectStreets boulevardSaintGermain.Id rueDeSeine.Id North
    |> World.Zone.connectStreets rueDeSeine.Id rueDuBac.Id East
    |> World.Zone.addMetroStation station
