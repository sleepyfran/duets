module Duets.Data.World.Cities.Tokyo.Shinjuku

open Duets.Data.World.Cities.Tokyo
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let kabukicho (zone: Zone) =
    let street =
        World.Street.create Ids.Street.kabukicho (StreetType.Split(East, 3))
        |> World.Street.tag "kabukicho"

    let concertSpaces =
        [ ("Shinjuku BLAZE", 1500, 89<quality>, Layouts.concertSpaceLayout3, zone.Id)
          |> PlaceCreators.createConcertSpace street.Id
          ("Shinjuku Loft", 500, 87<quality>, Layouts.concertSpaceLayout2, zone.Id)
          |> PlaceCreators.createConcertSpace street.Id
          ("Pit Inn Shinjuku", 150, 91<quality>, Layouts.concertSpaceLayout1, zone.Id)
          |> PlaceCreators.createConcertSpace street.Id
          |> World.Place.tag "jazz-club"
          ("Shinjuku ReNY", 800, 86<quality>, Layouts.concertSpaceLayout3, zone.Id)
          |> PlaceCreators.createConcertSpace street.Id
          ("Shinjuku Naked Loft", 250, 84<quality>, Layouts.concertSpaceLayout1, zone.Id)
          |> PlaceCreators.createConcertSpace street.Id ]

    let bars =
        [ ("Zoetrope Whisky Bar", 91<quality>, zone.Id)
          |> PlaceCreators.createBar street.Id
          |> World.Place.tag "cocktail-bar"
          ("Albatross G", 83<quality>, zone.Id)
          |> PlaceCreators.createBar street.Id ]

    let casinos =
        [ ("Shinjuku Pachinko Palace", 78<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCasino street.Id)

    let hotels =
        [ ("Park Hyatt Tokyo", 98<quality>, 700m<dd>, zone.Id)
          ("Shinjuku Granbell Hotel", 84<quality>, 300m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let metroStation =
        ("Shinjuku Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces casinos
        |> World.Street.addPlaces hotels
        |> World.Street.addPlace metroStation

    street, metroStation

let nishiShinjuku (zone: Zone) (city: City) =
    let street =
        World.Street.create Ids.Street.nishiShinjuku StreetType.OneWay
        |> World.Street.tag "omoide-yokocho"

    let gyms =
        [ ("Gold's Gym Shinjuku", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let hospitals =
        [ ("Keio University Hospital", 94<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createHospital street.Id)

    let radioStudios =
        [ ("Tokyo FM Broadcast Center", 92<quality>, "J-Pop", zone.Id) ]
        |> List.map (PlaceCreators.createRadioStudio city street.Id)

    let restaurants =
        [ ("Tsunahachi Shinjuku", 90<quality>, Japanese, zone.Id)
          ("Gyukatsu Motomura", 87<quality>, Japanese, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    street
    |> World.Street.addPlaces gyms
    |> World.Street.addPlaces hospitals
    |> World.Street.addPlaces radioStudios
    |> World.Street.addPlaces restaurants

let shinjukuSanchome (zone: Zone) =
    let street =
        World.Street.create Ids.Street.shinjukuSanchome StreetType.OneWay

    let bookstores =
        [ ("Kinokuniya Shinjuku", 95<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let cafes =
        [ ("Cafe Aaliya", 88<quality>, zone.Id)
          ("Doutor Coffee Shinjuku", 80<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let merchandiseWorkshops =
        [ ("Disk Union Shinjuku", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    let carDealers =
        [ ("Honda Cars Shinjuku",
           80<quality>,
           zone.Id,
           { Dealer =
               (Character.from
                   "Hiroshi Nakamura"
                   Male
                   (Shorthands.Autumn 5<days> 1978<years>))
             PriceRange = CarPriceRange.Budget }) ]
        |> List.map (PlaceCreators.createCarDealer street.Id)

    street
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces merchandiseWorkshops
    |> World.Street.addPlaces carDealers

let createZone (city: City) =
    let shinjukuZone = World.Zone.create Ids.Zone.shinjuku

    let kabukicho, shinjukuMetroStation = kabukicho shinjukuZone
    let nishiShinjuku = nishiShinjuku shinjukuZone city
    let shinjukuSanchome = shinjukuSanchome shinjukuZone

    let station =
        { Lines = [ Red ]
          LeavesToStreet = kabukicho.Id
          PlaceId = shinjukuMetroStation.Id }

    shinjukuZone
    |> World.Zone.addStreet (World.Node.create kabukicho.Id kabukicho)
    |> World.Zone.addStreet (World.Node.create nishiShinjuku.Id nishiShinjuku)
    |> World.Zone.addStreet (World.Node.create shinjukuSanchome.Id shinjukuSanchome)
    |> World.Zone.connectStreets kabukicho.Id nishiShinjuku.Id West
    |> World.Zone.connectStreets kabukicho.Id shinjukuSanchome.Id East
    |> World.Zone.addMetroStation station
