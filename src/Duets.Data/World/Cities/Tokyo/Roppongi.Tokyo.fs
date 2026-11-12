module Duets.Data.World.Cities.Tokyo.Roppongi

open Duets.Data.World.Cities.Tokyo
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let roppongiDori (zone: Zone) =
    let street =
        World.Street.create Ids.Street.roppongiDori (StreetType.Split(East, 3))
        |> World.Street.tag "roppongi-hills"

    let concertSpaces =
        [ ("Billboard Live Tokyo", 300, 95<quality>, Layouts.concertSpaceLayout2, zone.Id)
          |> PlaceCreators.createConcertSpace street.Id
          |> World.Place.tag "jazz-club"
          ("EX Theater Roppongi", 2000, 92<quality>, Layouts.concertSpaceLayout3, zone.Id)
          |> PlaceCreators.createConcertSpace street.Id
          ("Roppongi Hills Arena", 3000, 90<quality>, Layouts.concertSpaceLayout4, zone.Id)
          |> PlaceCreators.createConcertSpace street.Id ]

    let bars =
        [ ("Mado Lounge", 93<quality>, zone.Id)
          |> PlaceCreators.createBar street.Id
          |> World.Place.tag "cocktail-bar"
          ("SuperDeluxe", 87<quality>, zone.Id)
          |> PlaceCreators.createBar street.Id ]

    let restaurants =
        [ ("Brasserie Paul Bocuse le Musee", 96<quality>, French, zone.Id)
          ("L'Atelier de Joel Robuchon", 98<quality>, French, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let hotels =
        [ ("The Ritz-Carlton Tokyo", 99<quality>, 900m<dd>, zone.Id)
          ("Grand Hyatt Tokyo", 96<quality>, 600m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let metroStation =
        ("Roppongi Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces hotels
        |> World.Street.addPlace metroStation

    street, metroStation

let azabuJuban (zone: Zone) (city: City) =
    let street =
        World.Street.create Ids.Street.azabuJuban StreetType.OneWay

    let studios =
        [ ("Azabu Sound Studio",
           93<quality>,
           800m<dd>,
           (Character.from
               "Rie Fujiwara"
               Female
               (Shorthands.Winter 3<days> 1971<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let rehearsalSpaces =
        [ ("Azabu Practice Rooms", 88<quality>, 450m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let cafes =
        [ ("Bear Pond Espresso", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let carDealers =
        [ ("Lexus Azabu",
           97<quality>,
           zone.Id,
           { Dealer =
               (Character.from
                   "Takeshi Mori"
                   Male
                   (Shorthands.Spring 18<days> 1968<years>))
             PriceRange = CarPriceRange.Premium }) ]
        |> List.map (PlaceCreators.createCarDealer street.Id)

    street
    |> World.Street.addPlaces studios
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces carDealers

let createZone (city: City) =
    let roppongiZone = World.Zone.create Ids.Zone.roppongi

    let roppongiDori, roppongiMetroStation = roppongiDori roppongiZone
    let azabuJuban = azabuJuban roppongiZone city

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = roppongiDori.Id
          PlaceId = roppongiMetroStation.Id }

    roppongiZone
    |> World.Zone.addStreet (World.Node.create roppongiDori.Id roppongiDori)
    |> World.Zone.addStreet (World.Node.create azabuJuban.Id azabuJuban)
    |> World.Zone.connectStreets roppongiDori.Id azabuJuban.Id South
    |> World.Zone.addMetroStation station
