module Duets.Data.World.Cities.Seoul.Gangnam

open Duets.Data.World.Cities.Seoul
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let gangnamDaero (zone: Zone) =
    let street =
        World.Street.create Ids.Street.gangnamDaero (StreetType.Split(East, 3))

    let concertSpaces =
        [ ("COEX Artium", 3000, 92<quality>, Layouts.concertSpaceLayout4, zone.Id)
          ("SMTown Theatre", 1200, 90<quality>, Layouts.concertSpaceLayout3, zone.Id)
          ("Club Octagon", 500, 88<quality>, Layouts.concertSpaceLayout2, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Monkey Museum", 89<quality>, zone.Id)
          |> PlaceCreators.createBar street.Id
          ("Le Chamber", 92<quality>, zone.Id)
          |> PlaceCreators.createBar street.Id
          |> World.Place.tag "cocktail-bar" ]

    let hotels =
        [ ("Park Hyatt Seoul", 96<quality>, 500m<dd>, zone.Id)
          ("InterContinental COEX", 92<quality>, 400m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let metroStation =
        ("Gangnam Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces hotels
        |> World.Street.addPlace metroStation

    street, metroStation

let teheranRo (zone: Zone) =
    let street =
        World.Street.create Ids.Street.teheranRo (StreetType.Split(East, 2))

    let concertSpaces =
        [ ("Yes24 Live Hall", 2000, 88<quality>, Layouts.concertSpaceLayout3, zone.Id)
          ("Seoul Olympic Gymnastics Arena", 15000, 90<quality>, Layouts.concertSpaceLayout4, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let casinos =
        [ ("Seven Luck Casino Gangnam", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCasino street.Id)

    let carDealers =
        [ ("Hyundai Motorstudio Seoul",
           94<quality>,
           zone.Id,
           { Dealer =
               (Character.from
                   "Ji-ho Kim"
                   Male
                   (Shorthands.Autumn 15<days> 1980<years>))
             PriceRange = CarPriceRange.Premium }) ]
        |> List.map (PlaceCreators.createCarDealer street.Id)

    let restaurants =
        [ ("Mingles", 94<quality>, Korean, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Terarosa Coffee Gangnam", 91<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces casinos
    |> World.Street.addPlaces carDealers
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces cafes

let apgujeongRo (cityId: CityId) (zone: Zone) =
    let street =
        World.Street.create Ids.Street.apgujeongRo StreetType.OneWay

    let concertSpaces =
        [ ("Blue Square Mastercard Hall", 2800, 91<quality>, Layouts.concertSpaceLayout4, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let cinemas =
        [ ("CGV Cheongdam", 93<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCinema cityId street.Id)

    let bookstores =
        [ ("Kyobo Book Centre Gangnam", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces cinemas
    |> World.Street.addPlaces bookstores

let createZone (city: City) =
    let gangnamZone = World.Zone.create Ids.Zone.gangnam

    let gangnamDaero, gangnamMetroStation = gangnamDaero gangnamZone
    let teheranRo = teheranRo gangnamZone
    let apgujeongRo = apgujeongRo city.Id gangnamZone

    let station =
        { Lines = [ Red ]
          LeavesToStreet = gangnamDaero.Id
          PlaceId = gangnamMetroStation.Id }

    gangnamZone
    |> World.Zone.addStreet (World.Node.create gangnamDaero.Id gangnamDaero)
    |> World.Zone.addStreet (World.Node.create teheranRo.Id teheranRo)
    |> World.Zone.addStreet (World.Node.create apgujeongRo.Id apgujeongRo)
    |> World.Zone.connectStreets gangnamDaero.Id teheranRo.Id East
    |> World.Zone.connectStreets teheranRo.Id apgujeongRo.Id South
    |> World.Zone.addMetroStation station
