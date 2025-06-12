module rec Duets.Data.World.Cities.Tokyo.Ginza

open Duets.Data.World.Cities
open Duets.Entities

let private chuoDori (zone: Zone) =
    let street = World.Street.create "Chuo Dori" (StreetType.Split(East, 3))

    let restaurants =
        [ ("Sukiyabashi Jiro", 98<quality>, Japanese, zone.Id)
          ("Ginza Kyubey", 95<quality>, Japanese, zone.Id)
          ("Ginza Steak", 90<quality>, Japanese, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Ginza Coffee", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let bookstores =
        [ ("Ginza Tsutaya", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let concertSpaces =
        [ ("Ginza Swing",
           280,
           87<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let metroStation =
        ("Ginza Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces cafes
        |> World.Street.addPlaces bookstores
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlace metroStation

    street, metroStation

let private luxuryRow (zone: Zone) =
    let street = World.Street.create "Luxury Row" (StreetType.Split(North, 2))

    let hotels =
        [ ("The Peninsula Tokyo", 95<quality>, 650m<dd>, zone.Id)
          ("Mandarin Oriental", 93<quality>, 620m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    street |> World.Street.addPlaces hotels

let createZone city =
    let ginzaZone = World.Zone.create "Ginza"

    let chuoDori, metroStation = chuoDori ginzaZone
    let luxuryRow = luxuryRow ginzaZone

    let station =
        { Lines = [ Red ]
          LeavesToStreet = chuoDori.Id
          PlaceId = metroStation.Id }

    ginzaZone
    |> World.Zone.addStreet (World.Node.create chuoDori.Id chuoDori)
    |> World.Zone.addStreet (World.Node.create luxuryRow.Id luxuryRow)
    |> World.Zone.connectStreets chuoDori.Id luxuryRow.Id South
    |> World.Zone.addDescriptor Luxurious
    |> World.Zone.addDescriptor Glitz
    |> World.Zone.addMetroStation station
