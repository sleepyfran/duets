module Duets.Data.World.Cities.Seoul.Itaewon

open Duets.Data.World.Cities.Seoul
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let itaewonRo (zone: Zone) (city: City) =
    let street =
        World.Street.create Ids.Street.itaewonRo (StreetType.Split(East, 2))
        |> World.Street.attachContext
            """
        Itaewon-ro is Seoul's most internationally flavoured street, a legacy
        of its proximity to the former US military base at Yongsan. The strip
        is lined with foreign restaurants, imported goods stores, and venues
        that have historically catered to an expat crowd. In recent years the
        neighbourhood has undergone a significant cultural shift, drawing
        younger Koreans and tourists attracted by its cosmopolitan atmosphere,
        boutique bars, and large-scale entertainment venues nearby.
"""

    let concertSpaces =
        [ ("Seoul Olympic Gymnastics Arena", 15000, 90<quality>, Layouts.concertSpaceLayout4, zone.Id)
          ("War Memorial Hall", 5000, 88<quality>, Layouts.concertSpaceLayout4, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Magpie Brewing Itaewon", 87<quality>, zone.Id)
          ("Southside Parlor", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let hotels =
        [ ("Grand Hyatt Seoul", 94<quality>, 480m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let gyms =
        [ ("CrossFit Itaewon", 84<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let metroStation =
        ("Itaewon Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces hotels
        |> World.Street.addPlaces gyms
        |> World.Street.addPlace metroStation

    street, metroStation

let gyeongndanGil (zone: Zone) (city: City) =
    let street =
        World.Street.create Ids.Street.gyeongndanGil StreetType.OneWay
        |> World.Street.attachContext
            """
        Gyeongnidan-gil is a winding hillside street that climbs away from
        the main Itaewon strip toward Namsan Mountain. It has emerged as one
        of Seoul's most eclectic neighbourhoods, packed with independent
        restaurants serving cuisines from across the world, boutique clothing
        stores, and small music studios. The incline and the narrow lane-ways
        give it a more intimate, village-like feel than the busier streets
        below — a quality that has made it a favourite haunt for creatives
        and the city's international community.
"""

    let studios =
        [ ("KOKO Sound Studio",
           89<quality>,
           600m<dd>,
           (Character.from
               "Ha-eun Jung"
               Female
               (Shorthands.Summer 14<days> 1983<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let rehearsalSpaces =
        [ ("Itaewon Music Box", 84<quality>, 320m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let restaurants =
        [ ("Linus' BBQ", 87<quality>, American, zone.Id)
          ("Plant Cafe & Kitchen", 85<quality>, Vietnamese, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let carDealers =
        [ ("BMW Korea Itaewon",
           88<quality>,
           zone.Id,
           { Dealer =
               (Character.from
                   "Dong-hyun Choi"
                   Male
                   (Shorthands.Winter 28<days> 1976<years>))
             PriceRange = CarPriceRange.MidRange }) ]
        |> List.map (PlaceCreators.createCarDealer street.Id)

    let merchandiseWorkshops =
        [ ("Millimetre Milligram", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    let cafes =
        [ ("Cafe Sukkara", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlaces studios
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces carDealers
    |> World.Street.addPlaces merchandiseWorkshops
    |> World.Street.addPlaces cafes

let createZone (city: City) =
    let itaewonZone = World.Zone.create Ids.Zone.itaewon

    let itaewonRo, itaewonMetroStation = itaewonRo itaewonZone city
    let gyeongndanGil = gyeongndanGil itaewonZone city

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = itaewonRo.Id
          PlaceId = itaewonMetroStation.Id }

    itaewonZone
    |> World.Zone.addStreet (World.Node.create itaewonRo.Id itaewonRo)
    |> World.Zone.addStreet (World.Node.create gyeongndanGil.Id gyeongndanGil)
    |> World.Zone.connectStreets itaewonRo.Id gyeongndanGil.Id North
    |> World.Zone.addMetroStation station
