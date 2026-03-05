module Duets.Data.World.Cities.Santiago.Bellavista

open Duets.Data.World.Cities.Santiago
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let callePioNono (zone: Zone) =
    let street =
        World.Street.create Ids.Street.callePioNono (StreetType.Split(North, 2))
        |> World.Street.attachContext
            """
        Calle Pío Nono is the beating heart of Bellavista, Santiago's
        bohemian barrio nestled at the foot of Cerro San Cristóbal. Named
        after Pope Pius IX, the street is famous for its clusters of bars,
        outdoor restaurants, and live music venues that draw artists,
        students, and night owls from across the city. Murals cover the
        walls of the narrow side streets branching off it, and the smell
        of grilled meat and fresh empanadas drifts through the air from
        dusk onwards.
"""

    let concertSpaces =
        [ ("La Batuta", 150, 80<quality>, Layouts.concertSpaceLayout1, zone.Id)
          ("Centro Arte Alameda", 250, 82<quality>, Layouts.concertSpaceLayout1, zone.Id)
          ("Club Blondie", 500, 84<quality>, Layouts.concertSpaceLayout2, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Bar Constitución", 86<quality>, zone.Id)
          ("Etniko Bar", 84<quality>, zone.Id)
          ("La Casa en el Aire", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("Como Agua para Chocolate", 88<quality>, Chilean, zone.Id)
          ("Azul Profundo", 84<quality>, Chilean, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let metroStation =
        ("Baquedano Norte Metro Station", zone.Id)
        |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlace metroStation

    street, metroStation

let calleConstitucion (city: City) (zone: Zone) =
    let street =
        World.Street.create Ids.Street.calleConstitucion StreetType.OneWay
        |> World.Street.attachContext
            """
        Calle Constitución is one of the quieter lanes that run parallel to
        Pío Nono in Bellavista, offering a slightly more tranquil experience
        amid the barrio's energy. It is home to independent bookshops,
        small recording studios, and cafés favoured by writers and musicians.
        The street's low-rise architecture and leafy pavements give it
        a village-like quality that contrasts pleasantly with the bustle nearby.
"""

    let bookstores =
        [ ("Librería Metales Pesados", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let cafes =
        [ ("Café Escondido", 82<quality>, zone.Id)
          ("Bravo Café", 80<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let studios =
        [ ("Estudios Bellavista",
           82<quality>,
           280m<dd>,
           Character.from
               "Valentina Rojas"
               Female
               (Shorthands.Summer 20<days> 1985<years>),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let radioStudios =
        [ ("Radio Futuro", 84<quality>, "Rock", zone.Id) ]
        |> List.map (PlaceCreators.createRadioStudio city street.Id)

    street
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces studios
    |> World.Street.addPlaces radioStudios

let createZone (city: City) =
    let bellavistaZone = World.Zone.create Ids.Zone.bellavista

    let callePioNono, bellavistaMetroStation = callePioNono bellavistaZone
    let calleConstitucion = calleConstitucion city bellavistaZone

    let station =
        { Lines = [ Red ]
          LeavesToStreet = callePioNono.Id
          PlaceId = bellavistaMetroStation.Id }

    bellavistaZone
    |> World.Zone.addStreet (World.Node.create callePioNono.Id callePioNono)
    |> World.Zone.addStreet (World.Node.create calleConstitucion.Id calleConstitucion)
    |> World.Zone.connectStreets callePioNono.Id calleConstitucion.Id East
    |> World.Zone.addMetroStation station
