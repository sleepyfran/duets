module Duets.Data.World.Cities.Tokyo.Akihabara

open Duets.Data.World.Cities.Tokyo
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let chuoDori (cityId: CityId) (zone: Zone) =
    let street =
        World.Street.create Ids.Street.chuoDori (StreetType.Split(East, 2))
        |> World.Street.tag "elevated-train"

    let concertSpaces =
        [ ("Akihabara Goodman", 200, 85<quality>, Layouts.concertSpaceLayout1, zone.Id)
          ("Zepp DiverCity Tokyo", 2500, 91<quality>, Layouts.concertSpaceLayout4, zone.Id)
          ("AKB48 Theater", 260, 88<quality>, Layouts.concertSpaceLayout1, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let cinemas =
        [ ("Akihabara UDX Cinema", 84<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCinema cityId street.Id)

    let bookstores =
        [ ("Yodobashi Camera Media", 90<quality>, zone.Id)
          ("Mandarake Complex", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let cafes =
        [ ("Akihabara Maid Cafe", 82<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let metroStation =
        ("Akihabara Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces cinemas
        |> World.Street.addPlaces bookstores
        |> World.Street.addPlaces cafes
        |> World.Street.addPlace metroStation

    street, metroStation

let kandaDori (zone: Zone) (city: City) =
    let street =
        World.Street.create Ids.Street.kandaDori (StreetType.Split(West, 2))
        |> World.Street.tag "kanda-myojin"

    let restaurants =
        [ ("Kanda Yabu Soba", 91<quality>, Japanese, zone.Id)
          ("Ramen Jiro Akihabara", 83<quality>, Japanese, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let bars =
        [ ("Bar Ishinohana", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id >> World.Place.tag "cocktail-bar")

    let gyms =
        [ ("Anytime Fitness Akihabara", 82<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let merchandiseWorkshops =
        [ ("Ishibashi Music Akihabara", zone.Id)
          ("Guitar Center Akihabara", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    let concertSpaces =
        [ ("Tokyo Dome", 55000, 99<quality>, Layouts.concertSpaceLayout4, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces gyms
    |> World.Street.addPlaces merchandiseWorkshops

let createZone (city: City) =
    let akihabaraZone = World.Zone.create Ids.Zone.akihabara

    let chuoDori, akihabaraMetroStation = chuoDori city.Id akihabaraZone
    let kandaDori = kandaDori akihabaraZone city

    let station =
        { Lines = [ Red; Blue ]
          LeavesToStreet = chuoDori.Id
          PlaceId = akihabaraMetroStation.Id }

    akihabaraZone
    |> World.Zone.addStreet (World.Node.create chuoDori.Id chuoDori)
    |> World.Zone.addStreet (World.Node.create kandaDori.Id kandaDori)
    |> World.Zone.connectStreets chuoDori.Id kandaDori.Id North
    |> World.Zone.addMetroStation station
