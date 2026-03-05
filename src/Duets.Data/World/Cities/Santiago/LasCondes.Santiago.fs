module Duets.Data.World.Cities.Santiago.LasCondes

open Duets.Data.World.Cities.Santiago
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let avenidaApoquindo (city: City) (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.avenidaApoquindo
            (StreetType.Split(East, 3))
        |> World.Street.attachContext
            """
        Avenida Apoquindo is the financial and commercial spine of Las Condes,
        Santiago's most affluent district. The avenue is flanked by gleaming
        glass office towers, luxury hotels, and upscale shopping centres such
        as El Golf and Parque Arauco. It also hosts the El Golf metro station,
        one of the key stops on Line 1, and several of the city's premier
        performance venues that attract international touring acts.
"""

    let concertSpaces =
        [ ("Teatro Municipal Las Condes", 800, 88<quality>, Layouts.concertSpaceLayout2, zone.Id)
          ("Teatro Oriente", 1200, 86<quality>, Layouts.concertSpaceLayout3, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let hotels =
        [ ("W Santiago", 96<quality>, 450m<dd>, zone.Id)
          ("The Ritz-Carlton Santiago", 98<quality>, 520m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let radioStudios =
        [ ("Radio Concierto", 88<quality>, "Rock", zone.Id) ]
        |> List.map (PlaceCreators.createRadioStudio city street.Id)

    let metroStation =
        ("El Golf Metro Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces hotels
        |> World.Street.addPlaces radioStudios
        |> World.Street.addPlace metroStation

    street, metroStation

let avenidaIsidoraGoyenechea (cityId: CityId) (zone: Zone) =
    let street =
        World.Street.create Ids.Street.avenidaIsidoraGoyenechea StreetType.OneWay
        |> World.Street.attachContext
            """
        Avenida Isidora Goyenechea is the social hub of Las Condes,
        a tree-lined street packed with Santiago's finest restaurants,
        wine bars, and cocktail lounges. Known locally as 'El Barrio El Golf',
        it is the go-to destination for business dinners, celebrations,
        and after-work drinks among Santiago's professional class. High-end
        cinemas and concept stores fill in the gaps between the restaurants.
"""

    let restaurants =
        [ ("Osaka Santiago", 92<quality>, Japanese, zone.Id)
          ("Boragó", 96<quality>, Chilean, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let bars =
        [ ("Bar de Cócteles 40/40", 90<quality>, zone.Id)
          ("Whisky Bar Las Condes", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let cinemas =
        [ ("Cine Hoyts El Golf", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCinema cityId street.Id)

    street
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces cinemas

let avenidaManquehue (city: City) (zone: Zone) =
    let street =
        World.Street.create Ids.Street.avenidaManquehue StreetType.OneWay
        |> World.Street.attachContext
            """
        Avenida Manquehue runs north-south through the upper reaches of
        Las Condes, connecting the residential highlands to the busy
        commercial corridor below. Flanked by exclusive apartment blocks,
        luxury car dealerships, and large-scale sports venues, it serves
        a well-heeled population that values quality, convenience, and access.
        The nearby Estadio Nacional sits at the boundary between districts
        and hosts the largest concerts and sporting events in Chile.
"""

    let concertSpaces =
        [ ("Estadio Nacional Julio Martínez Prádanos", 50000, 82<quality>, Layouts.concertSpaceLayout4, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let carDealers =
        [ ("Salón BMW Las Condes",
           94<quality>,
           zone.Id,
           { Dealer =
               (Character.from
                   "Andrés Mora"
                   Male
                   (Shorthands.Winter 3<days> 1975<years>))
             PriceRange = CarPriceRange.Premium }) ]
        |> List.map (PlaceCreators.createCarDealer street.Id)

    let gyms =
        [ ("Gold's Gym Las Condes", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let home = PlaceCreators.createHome street.Id zone.Id

    let cafes =
        [ ("Starbucks Manquehue", 78<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces carDealers
    |> World.Street.addPlaces gyms
    |> World.Street.addPlace home
    |> World.Street.addPlaces cafes

let createZone (city: City) =
    let lasCondesZone = World.Zone.create Ids.Zone.lasCondes

    let avenidaApoquindo, lasCondesMetroStation =
        avenidaApoquindo city lasCondesZone

    let avenidaIsidoraGoyenechea = avenidaIsidoraGoyenechea city.Id lasCondesZone
    let avenidaManquehue = avenidaManquehue city lasCondesZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = avenidaApoquindo.Id
          PlaceId = lasCondesMetroStation.Id }

    lasCondesZone
    |> World.Zone.addStreet (World.Node.create avenidaApoquindo.Id avenidaApoquindo)
    |> World.Zone.addStreet (World.Node.create avenidaIsidoraGoyenechea.Id avenidaIsidoraGoyenechea)
    |> World.Zone.addStreet (World.Node.create avenidaManquehue.Id avenidaManquehue)
    |> World.Zone.connectStreets avenidaApoquindo.Id avenidaIsidoraGoyenechea.Id South
    |> World.Zone.connectStreets avenidaIsidoraGoyenechea.Id avenidaManquehue.Id East
    |> World.Zone.addMetroStation station
