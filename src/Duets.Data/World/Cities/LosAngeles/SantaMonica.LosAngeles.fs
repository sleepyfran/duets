module rec Duets.Data.World.Cities.LosAngeles.SantaMonica

open Duets.Data.World.Cities
open Duets.Entities
open Fugit.Months

let oceanAvenue city =
    let hotels =
        [ ("Shutters on the Beach", 86<quality>, 350m<dd>)
          ("Hotel Casa Del Mar", 84<quality>, 320m<dd>) ]
        |> List.map PlaceCreators.createHotel

    let restaurants =
        [ ("The Lobster", 87<quality>, Italian)
          ("Ivy at the Shore", 88<quality>, Vietnamese) ]
        |> List.map PlaceCreators.createRestaurant

    let gyms =
        [ ("Equinox Santa Monica", 85<quality>) ]
        |> List.map (PlaceCreators.createGym city)

    let cafes =
        [ ("Urth Caffe", 78<quality>); ("Goodboybob Coffee", 72<quality>) ]
        |> List.map PlaceCreators.createCafe

    let concertSpaces =
        [ ("The Mint", 300, 80<quality>, Layouts.concertSpaceLayout1) ]
        |> List.map PlaceCreators.createConcertSpace

    let metroStation = "Santa Monica Station" |> PlaceCreators.createMetro

    World.Street.create "Ocean Avenue" (StreetType.Split(North, 3))
    |> World.Street.addPlaces hotels
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces gyms
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlace metroStation

let pierWay =
    let restaurants =
        [ ("The Albright", 82<quality>, American)
          ("Pier Burger", 70<quality>, American)
          ("The Lobster Trap", 77<quality>, Italian) ]
        |> List.map PlaceCreators.createRestaurant

    let bars = [ ("The Brig", 74<quality>) ] |> List.map PlaceCreators.createBar

    let concertSpaces =
        [ ("McCabe's Guitar Shop", 150, 78<quality>, Layouts.concertSpaceLayout1)
          ("Harvelle's", 200, 75<quality>, Layouts.concertSpaceLayout1) ]
        |> List.map PlaceCreators.createConcertSpace

    World.Street.create "Pier Way" (StreetType.Split(East, 3))
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces concertSpaces

let promenadePath =
    let bookstores =
        [ ("Barnes & Noble", 75<quality>) ]
        |> List.map PlaceCreators.createBookstore

    let hotels =
        [ ("Fairmont Miramar Hotel & Bungalows", 92<quality>, 380m<dd>) ]
        |> List.map PlaceCreators.createHotel

    let restaurants =
        [ ("FIG Restaurant", 80<quality>, Vietnamese)
          ("The Water Grill", 85<quality>, American) ]
        |> List.map PlaceCreators.createRestaurant

    let recordingStudios =
        [ ("Sound City Studios",
           93<quality>,
           350m<dd>,
           (Character.from "Butch Vig" Male (August 2 1957)))
          ("Record Plant",
           90<quality>,
           320m<dd>,
           (Character.from "Jack Joseph Puig" Male (May 2 1967))) ]
        |> List.map PlaceCreators.createStudio

    World.Street.create "Promenade Path" (StreetType.Split(West, 3))
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces hotels
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces recordingStudios


let createZone city =
    let oceanAvenue = oceanAvenue city
    let pierWay = pierWay
    let promenadePath = promenadePath

    let metroStation =
        { Line = Blue
          LeavesToStreet = oceanAvenue.Id }

    World.Zone.create
        "Santa Monica"
        (World.Node.create oceanAvenue.Id oceanAvenue)
    |> World.Zone.addStreet (World.Node.create pierWay.Id pierWay)
    |> World.Zone.addStreet (World.Node.create promenadePath.Id promenadePath)
    |> World.Zone.connectStreets oceanAvenue.Id pierWay.Id East
    |> World.Zone.connectStreets pierWay.Id promenadePath.Id North
    |> World.Zone.addDescriptor Coastal
    |> World.Zone.addDescriptor Luxurious
    |> World.Zone.addMetroStation metroStation
