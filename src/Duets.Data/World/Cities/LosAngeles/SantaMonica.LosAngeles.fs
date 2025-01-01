module rec Duets.Data.World.Cities.LosAngeles.SantaMonica

open Duets.Data.World.Cities
open Duets.Entities
open Fugit.Months

let oceanAvenue city (zone: Zone) =
    let hotels =
        [ ("Shutters on the Beach", 86<quality>, 350m<dd>, zone.Id)
          ("Hotel Casa Del Mar", 84<quality>, 320m<dd>, zone.Id) ]
        |> List.map PlaceCreators.createHotel

    let restaurants =
        [ ("The Lobster", 87<quality>, Italian, zone.Id)
          ("Ivy at the Shore", 88<quality>, Vietnamese, zone.Id) ]
        |> List.map PlaceCreators.createRestaurant

    let gyms =
        [ ("Equinox Santa Monica", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city)

    let cafes =
        [ ("Urth Caffe", 78<quality>, zone.Id)
          ("Goodboybob Coffee", 72<quality>, zone.Id) ]
        |> List.map PlaceCreators.createCafe

    let concertSpaces =
        [ ("The Mint", 300, 80<quality>, Layouts.concertSpaceLayout1, zone.Id) ]
        |> List.map PlaceCreators.createConcertSpace

    let metroStation =
        ("Santa Monica Station", zone.Id) |> PlaceCreators.createMetro

    World.Street.create "Ocean Avenue" (StreetType.Split(North, 3))
    |> World.Street.addPlaces hotels
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces gyms
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlace metroStation

let pierWay (zone: Zone) =
    let restaurants =
        [ ("The Albright", 82<quality>, American, zone.Id)
          ("Pier Burger", 70<quality>, American, zone.Id)
          ("The Lobster Trap", 77<quality>, Italian, zone.Id) ]
        |> List.map PlaceCreators.createRestaurant

    let bars =
        [ ("The Brig", 74<quality>, zone.Id) ]
        |> List.map PlaceCreators.createBar

    let concertSpaces =
        [ ("McCabe's Guitar Shop",
           150,
           78<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id)
          ("Harvelle's", 200, 75<quality>, Layouts.concertSpaceLayout1, zone.Id) ]
        |> List.map PlaceCreators.createConcertSpace

    World.Street.create "Pier Way" StreetType.OneWay
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces concertSpaces

let promenadePath (zone: Zone) =
    let bookstores =
        [ ("Barnes & Noble", 75<quality>, zone.Id) ]
        |> List.map PlaceCreators.createBookstore

    let hotels =
        [ ("Fairmont Miramar Hotel & Bungalows", 92<quality>, 380m<dd>, zone.Id) ]
        |> List.map PlaceCreators.createHotel

    let restaurants =
        [ ("FIG Restaurant", 80<quality>, Vietnamese, zone.Id)
          ("The Water Grill", 85<quality>, American, zone.Id) ]
        |> List.map PlaceCreators.createRestaurant

    let recordingStudios =
        [ ("Sound City Studios",
           93<quality>,
           350m<dd>,
           (Character.from "Butch Vig" Male (August 2 1957)),
           zone.Id)
          ("Record Plant",
           90<quality>,
           320m<dd>,
           (Character.from "Jack Joseph Puig" Male (May 2 1967)),
           zone.Id) ]
        |> List.map PlaceCreators.createStudio

    World.Street.create "Promenade Path" (StreetType.Split(West, 2))
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces hotels
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces recordingStudios


let createZone city =
    let santaMonicaZone = World.Zone.create "Santa Monica"

    let oceanAvenue = oceanAvenue city santaMonicaZone
    let pierWay = pierWay santaMonicaZone
    let promenadePath = promenadePath santaMonicaZone

    let metroStation =
        { Line = Blue
          LeavesToStreet = oceanAvenue.Id }

    santaMonicaZone
    |> World.Zone.addStreet (World.Node.create oceanAvenue.Id oceanAvenue)
    |> World.Zone.addStreet (World.Node.create pierWay.Id pierWay)
    |> World.Zone.addStreet (World.Node.create promenadePath.Id promenadePath)
    |> World.Zone.connectStreets oceanAvenue.Id pierWay.Id East
    |> World.Zone.connectStreets pierWay.Id promenadePath.Id North
    |> World.Zone.addDescriptor Coastal
    |> World.Zone.addDescriptor Luxurious
    |> World.Zone.addMetroStation metroStation
