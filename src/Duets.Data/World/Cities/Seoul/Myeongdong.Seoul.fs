module Duets.Data.World.Cities.Seoul.Myeongdong

open Duets.Data.World.Cities.Seoul
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let myeongdongGil (zone: Zone) =
    let street =
        World.Street.create Ids.Street.myeongdongGil (StreetType.Split(East, 2))
        |> World.Street.attachContext
            """
        Myeongdong-gil is Seoul's most famous shopping and tourism corridor,
        a pedestrianised street of wall-to-wall cosmetics stores, street food
        stalls, and international fashion chains. By day it draws shoppers from
        across Asia seeking Korean beauty products and fashion; by night the
        food carts take over, filling the air with the smell of tteokbokki and
        hotteok. Despite its commercial character the street retains a lively,
        festive energy that makes it one of the most visited spots in the country.
"""

    let concertSpaces =
        [ ("Chungmu Art Center", 1000, 89<quality>, Layouts.concertSpaceLayout3, zone.Id)
          ("Nanta Theatre", 500, 86<quality>, Layouts.concertSpaceLayout2, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let hotels =
        [ ("Lotte Hotel Seoul", 95<quality>, 450m<dd>, zone.Id)
          ("Shilla Stay Myeongdong", 88<quality>, 300m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let restaurants =
        [ ("Hadongkwan", 90<quality>, Korean, zone.Id)
          ("Myeongdong Kyoja", 88<quality>, Korean, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let metroStation =
        ("Myeongdong Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces hotels
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlace metroStation

    street, metroStation

let namdaemunRo (cityId: CityId) (zone: Zone) (city: City) =
    let street =
        World.Street.create Ids.Street.namdaemunRo StreetType.OneWay
        |> World.Street.attachContext
            """
        Namdaemun-ro passes through the historic heart of Seoul, running
        alongside the ancient Namdaemun Gate — one of the last surviving gates
        of the old city wall. The road anchors Seoul's traditional market
        district and connects the southern end of the city centre with the
        grand civic institutions that line its length: major concert halls,
        city offices, and national institutions. It is a street where old
        Seoul and modern Seoul coexist, sometimes uneasily, always vividly.
"""

    let concertSpaces =
        [ ("Seoul Arts Center", 2500, 93<quality>, Layouts.concertSpaceLayout4, zone.Id)
          ("Sejong Center for the Performing Arts", 3022, 94<quality>, Layouts.concertSpaceLayout4, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let hospitals =
        [ ("Severance Hospital", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createHospital street.Id)

    let bookstores =
        [ ("Kyobo Book Centre Main", 92<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let cafes =
        [ ("The Alley Myeongdong", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let radioStudios =
        [ ("KBS Radio Center", 88<quality>, "Pop", zone.Id) ]
        |> List.map (PlaceCreators.createRadioStudio city street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces hospitals
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces radioStudios

let createZone (city: City) =
    let myeongdongZone = World.Zone.create Ids.Zone.myeongdong

    let myeongdongGil, myeongdongMetroStation = myeongdongGil myeongdongZone
    let namdaemunRo = namdaemunRo city.Id myeongdongZone city

    let station =
        { Lines = [ Red; Blue ]
          LeavesToStreet = myeongdongGil.Id
          PlaceId = myeongdongMetroStation.Id }

    myeongdongZone
    |> World.Zone.addStreet (World.Node.create myeongdongGil.Id myeongdongGil)
    |> World.Zone.addStreet (World.Node.create namdaemunRo.Id namdaemunRo)
    |> World.Zone.connectStreets myeongdongGil.Id namdaemunRo.Id North
    |> World.Zone.addMetroStation station
