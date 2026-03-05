module Duets.Data.World.Cities.Santiago.Providencia

open Duets.Data.World.Cities.Santiago
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let avenidaProvidencia (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.avenidaProvidencia
            (StreetType.Split(East, 3))
        |> World.Street.attachContext
            """
        Avenida Providencia is the commercial and cultural backbone of
        Santiago's most cosmopolitan neighbourhood. Tree-lined and vibrant,
        it stretches from the western edge of Providencia through a succession
        of bookshops, cafés, restaurants, and boutiques that attract a
        creative, professional crowd. The avenue also hosts some of the city's
        best live music venues and is a central gathering point for locals
        and visitors who want to experience Santiago at its most energetic.
"""

    let concertSpaces =
        [ ("Bar El Clan", 300, 84<quality>, Layouts.concertSpaceLayout1, zone.Id)
          ("Club de Jazz de Santiago", 200, 82<quality>, Layouts.concertSpaceLayout1, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Bar Loreto", 85<quality>, zone.Id)
          ("The Clinic Bar", 82<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let hotels =
        [ ("Hotel Bidasoa", 82<quality>, 180m<dd>, zone.Id)
          ("NH Collection Plaza Santiago", 88<quality>, 250m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let metroStation =
        ("Pedro de Valdivia Metro Station", zone.Id)
        |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces hotels
        |> World.Street.addPlace metroStation

    street, metroStation

let avenidaSuecia (zone: Zone) =
    let street =
        World.Street.create Ids.Street.avenidaSuecia StreetType.OneWay
        |> World.Street.attachContext
            """
        Avenida Suecia is a charming street in the heart of Providencia,
        known for its concentration of cosy restaurants, lively bars, and
        independent bookshops. Its relaxed atmosphere makes it a favourite
        for long lunches and evening outings. A short walk from the main
        Providencia boulevard, it captures the quieter, more intimate side
        of the district.
"""

    let restaurants =
        [ ("Liguria Providencia", 88<quality>, Chilean, zone.Id)
          ("Astrid y Gastón Santiago", 92<quality>, French, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Café Mosqueto", 84<quality>, zone.Id)
          ("Café del Poeta", 80<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let bookstores =
        [ ("Librería Antártica", 82<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    street
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces bookstores

let avenidaNuevaProvidencia (city: City) (zone: Zone) =
    let street =
        World.Street.create Ids.Street.avenidaNuevaProvidencia StreetType.OneWay
        |> World.Street.attachContext
            """
        Avenida Nueva Providencia, also known as Avenida 11 de Septiembre
        in part of its stretch, is a key commercial artery in Providencia
        lined with service businesses, car showrooms, fitness centres, and
        modern office towers. It serves the daily needs of the neighbourhood's
        residents and the thousands of professionals who work in the area.
"""

    let carDealers =
        [ ("Automotora Gildemeister Providencia",
           82<quality>,
           zone.Id,
           { Dealer =
               (Character.from
                   "Rodrigo Fuentes"
                   Male
                   (Shorthands.Autumn 10<days> 1978<years>))
             PriceRange = CarPriceRange.MidRange }) ]
        |> List.map (PlaceCreators.createCarDealer street.Id)

    let rehearsalSpaces =
        [ ("Sala de Ensayo ProSound", 80<quality>, 90m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let studios =
        [ ("Studio 33 Santiago",
           84<quality>,
           300m<dd>,
           Character.from
               "Marcelo Díaz"
               Male
               (Shorthands.Spring 5<days> 1982<years>),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let gyms =
        [ ("SmartFit Providencia", 80<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    street
    |> World.Street.addPlaces carDealers
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces studios
    |> World.Street.addPlaces gyms

let createZone (city: City) =
    let providenciaZone = World.Zone.create Ids.Zone.providencia

    let avenidaProvidencia, providenciaMetroStation =
        avenidaProvidencia providenciaZone

    let avenidaSuecia = avenidaSuecia providenciaZone
    let avenidaNuevaProvidencia = avenidaNuevaProvidencia city providenciaZone

    let station =
        { Lines = [ Red ]
          LeavesToStreet = avenidaProvidencia.Id
          PlaceId = providenciaMetroStation.Id }

    providenciaZone
    |> World.Zone.addStreet (World.Node.create avenidaProvidencia.Id avenidaProvidencia)
    |> World.Zone.addStreet (World.Node.create avenidaSuecia.Id avenidaSuecia)
    |> World.Zone.addStreet (World.Node.create avenidaNuevaProvidencia.Id avenidaNuevaProvidencia)
    |> World.Zone.connectStreets avenidaProvidencia.Id avenidaSuecia.Id East
    |> World.Zone.connectStreets avenidaSuecia.Id avenidaNuevaProvidencia.Id South
    |> World.Zone.addMetroStation station
