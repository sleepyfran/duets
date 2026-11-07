module rec Duets.Data.World.Cities.NewYork.MidtownWest

open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let private broadway (city: City) (zone: Zone) =
    let street =
        World.Street.create Ids.Street.broadway (StreetType.Split(North, 3))
        |> World.Street.tag "times-square"

    let home = PlaceCreators.createHome street.Id zone.Id

    let gyms =
        [ ("Equinox Times Square", 93<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let concerts =
        [ ("The Majestic Theatre",
           1500,
           95<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id)
          ("The Gershwin Theatre",
           1900,
           94<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("TSX Broadway", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let shops =
        [ ("Macy's Herald Square", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    let metroStation =
        ("Times Square Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let cinemas =
        [ ("AMC Empire 25", 91<quality>, zone.Id)
          ("Regal UA Court Street", 89<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCinema city.Id street.Id)

    let street =
        street
        |> World.Street.addPlace home
        |> World.Street.addPlaces gyms
        |> World.Street.addPlaces concerts
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces shops
        |> World.Street.addPlaces cinemas
        |> World.Street.addPlace metroStation

    street, metroStation

let private seventhAvenue (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.seventhAvenue
            (StreetType.Split(North, 2))

    let concerts =
        [ ("Madison Square Garden",
           20000,
           98<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Mustang Harry's", 84<quality>, zone.Id)
          ("Juniper Bar & Grill", 82<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let rehearsalSpaces =
        [ ("SIR Studios", 92<quality>, 200m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let street =
        street
        |> World.Street.addPlaces concerts
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces rehearsalSpaces

    street

let private fiftySeventhStreet (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.fiftySeventhStreet
            (StreetType.Split(East, 2))

    let carDealers =
        [ ("Manhattan Luxury Motors",
           96<quality>,
           zone.Id,
           { Dealer =
               (Character.from
                   "Michael Sterling"
                   Male
                   (Shorthands.Spring 5<days> 1978<years>))
             PriceRange = CarPriceRange.Premium }) ]
        |> List.map (PlaceCreators.createCarDealer street.Id)

    let concerts =
        [ ("Carnegie Hall",
           2800,
           99<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let restaurants =
        [ ("The Tea Room", 93<quality>, Czech, zone.Id)
          ("Brooklyn Diner", 85<quality>, American, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Carnegie Diner & Cafe", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlaces carDealers
    |> World.Street.addPlaces concerts
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces cafes

let private sixthAvenue (city: City) (zone: Zone) =
    let street = World.Street.create Ids.Street.sixthAvenue StreetType.OneWay

    let recordingStudios =
        [ ("Avatar Studios",
           94<quality>,
           400m<dd>,
           (Character.from
               "Roy Hendrickson"
               Male
               (Shorthands.Spring 10<days> 1968<years>)),
           zone.Id)
          ("Sear Sound",
           95<quality>,
           420m<dd>,
           (Character.from
               "Roberta Findlay"
               Female
               (Shorthands.Summer 22<days> 1970<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let radioStudios =
        [ ("Z100 (WHTZ-FM)", 94<quality>, "Pop", zone.Id)
          ("Q104.3 (WAXQ-FM)", 92<quality>, "Rock", zone.Id) ]
        |> List.map (PlaceCreators.createRadioStudio city street.Id)

    let cafes =
        [ ("Birch Coffee", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlaces recordingStudios
    |> World.Street.addPlaces radioStudios
    |> World.Street.addPlaces cafes

let createZone (city: City) =
    let midtownWestZone = World.Zone.create Ids.Zone.midtownWest

    let broadway, broadwayMetro = broadway city midtownWestZone
    let seventhAvenue = seventhAvenue midtownWestZone
    let fiftySeventhStreet = fiftySeventhStreet midtownWestZone
    let sixthAvenue = sixthAvenue city midtownWestZone

    let broadwayMetroStation =
        { Lines = [ Blue ]
          LeavesToStreet = broadway.Id
          PlaceId = broadwayMetro.Id }

    midtownWestZone
    |> World.Zone.addStreet (World.Node.create broadway.Id broadway)
    |> World.Zone.addStreet (World.Node.create seventhAvenue.Id seventhAvenue)
    |> World.Zone.addStreet (
        World.Node.create fiftySeventhStreet.Id fiftySeventhStreet
    )
    |> World.Zone.addStreet (World.Node.create sixthAvenue.Id sixthAvenue)
    |> World.Zone.connectStreets broadway.Id seventhAvenue.Id West
    |> World.Zone.connectStreets broadway.Id fiftySeventhStreet.Id North
    |> World.Zone.connectStreets broadway.Id sixthAvenue.Id East
    |> World.Zone.addMetroStation broadwayMetroStation
