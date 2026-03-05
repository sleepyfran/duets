module Duets.Data.World.Cities.Seoul.Gangnam

open Duets.Data.World.Cities.Seoul
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let gangnamDaero (zone: Zone) =
    let street =
        World.Street.create Ids.Street.gangnamDaero (StreetType.Split(East, 3))
        |> World.Street.attachContext
            """
        Gangnam-daero is the spine of Seoul's most affluent and fashionable
        district, stretching south from the Han River past gleaming skyscrapers,
        designer boutiques, and the sprawling COEX Mall complex. The road
        embodies the transformation of modern South Korea — polished, ambitious,
        and relentlessly forward-looking. Below street level, the enormous
        COEX underground city and its connected subway station form one of
        Asia's busiest transit hubs, feeding the district's constant stream
        of commuters, shoppers, and concertgoers.
"""

    let concertSpaces =
        [ ("COEX Artium", 3000, 92<quality>, Layouts.concertSpaceLayout4, zone.Id)
          ("SMTown Theatre", 1200, 90<quality>, Layouts.concertSpaceLayout3, zone.Id)
          ("Club Octagon", 500, 88<quality>, Layouts.concertSpaceLayout2, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Monkey Museum", 89<quality>, zone.Id)
          ("Le Chamber", 92<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

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
        World.Street.create Ids.Street.teheranRo StreetType.OneWay
        |> World.Street.attachContext
            """
        Teheran-ro, named after a street in the Iranian capital as part of a
        sister-city exchange, runs east through the heart of Gangnam's business
        district. Flanked by the glass towers of Korea's largest corporations and
        tech startups alike, it is often called the Silicon Valley of Seoul.
        After business hours the street transforms, with upscale restaurants
        and bars filling with professionals unwinding from the high-pressure
        workday.
"""

    let concertSpaces =
        [ ("Yes24 Live Hall", 2000, 88<quality>, Layouts.concertSpaceLayout3, zone.Id) ]
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
        [ ("Mingles", 94<quality>, French, zone.Id) ]
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
        |> World.Street.attachContext
            """
        Apgujeong-ro cuts through one of Seoul's most exclusive enclaves, where
        designer fashion houses sit alongside upscale plastic surgery clinics and
        Michelin-starred restaurants. The Rodeo Street area nearby draws a mix
        of celebrities, K-pop stars, and well-heeled shoppers. The neighbourhood
        has a manicured, almost cinematic quality — wide pavements, immaculate
        storefronts, and the quiet hum of serious money.
"""

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
