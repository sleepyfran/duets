module Duets.Data.World.Cities.Santiago.LasCondes

open Duets.Data.World.Cities.Santiago
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let avenidaApoquindo (city: City) (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.avenidaApoquindo
            (StreetType.Split(East, 3))

    let concertSpaces =
        [ ("Teatro Municipal Las Condes", 800, 88<quality>, Layouts.concertSpaceLayout2, zone.Id)
          ("Teatro Oriente", 1200, 86<quality>, Layouts.concertSpaceLayout3, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id >> World.Place.tag "seated")

    let hotels =
        [ ("W Santiago", 96<quality>, 450m<dd>, zone.Id)
          ("The Ritz-Carlton Santiago", 98<quality>, 520m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let radioStudios =
        [ ("Radio Concierto", 88<quality>, "Rock", zone.Id) ]
        |> List.map (PlaceCreators.createRadioStudio city street.Id)

    let metroStation =
        ("El Golf Metro Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces hotels
        |> World.Street.addPlaces radioStudios
        |> World.Street.addPlace metroStation

    street, metroStation

let avenidaIsidoraGoyenechea (cityId: CityId) (zone: Zone) =
    let street =
        World.Street.create Ids.Street.avenidaIsidoraGoyenechea StreetType.OneWay

    let restaurants =
        [ ("Osaka Santiago", 92<quality>, Japanese, zone.Id)
          ("Boragó", 96<quality>, Chilean, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let bars =
        [ ("Bar de Cócteles 40/40", 90<quality>, zone.Id)
          ("Whisky Bar Las Condes", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id >> World.Place.tag "cocktail-bar")

    let cinemas =
        [ ("Cine Hoyts El Golf", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCinema cityId street.Id)

    street
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces cinemas

let avenidaManquehue (city: City) (zone: Zone) =
    let street =
        World.Street.create Ids.Street.avenidaManquehue StreetType.OneWay

    let carDealers =
        [ ("Salón BMW Las Condes",
           94<quality>,
           zone.Id,
           { Dealer =
               (Character.from
                   "Andrés Mora"
                   Male
                   (Shorthands.Winter 3<days> 1975<years>))
             PriceRange = CarPriceRange.Premium }) ]
        |> List.map (PlaceCreators.createCarDealer street.Id)

    let gyms =
        [ ("Gold's Gym Las Condes", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let home = PlaceCreators.createHome street.Id zone.Id

    let cafes =
        [ ("Starbucks Manquehue", 78<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlaces carDealers
    |> World.Street.addPlaces gyms
    |> World.Street.addPlace home
    |> World.Street.addPlaces cafes

let createZone (city: City) =
    let lasCondesZone = World.Zone.create Ids.Zone.lasCondes

    let avenidaApoquindo, lasCondesMetroStation =
        avenidaApoquindo city lasCondesZone

    let avenidaIsidoraGoyenechea = avenidaIsidoraGoyenechea city.Id lasCondesZone
    let avenidaManquehue = avenidaManquehue city lasCondesZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = avenidaApoquindo.Id
          PlaceId = lasCondesMetroStation.Id }

    lasCondesZone
    |> World.Zone.addStreet (World.Node.create avenidaApoquindo.Id avenidaApoquindo)
    |> World.Zone.addStreet (World.Node.create avenidaIsidoraGoyenechea.Id avenidaIsidoraGoyenechea)
    |> World.Zone.addStreet (World.Node.create avenidaManquehue.Id avenidaManquehue)
    |> World.Zone.connectStreets avenidaApoquindo.Id avenidaIsidoraGoyenechea.Id South
    |> World.Zone.connectStreets avenidaIsidoraGoyenechea.Id avenidaManquehue.Id East
    |> World.Zone.addMetroStation station
