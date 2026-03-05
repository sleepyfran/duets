module Duets.Data.World.Cities.Santiago.SantiagoCentro

open Duets.Data.World.Cities.Santiago
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let paseoAhumada (cityId: CityId) (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.paseoAhumada
            (StreetType.Split(North, 2))
        |> World.Street.attachContext
            """
        Paseo Ahumada is Santiago's most famous pedestrian thoroughfare,
        cutting through the historic centre from the Alameda to the Plaza
        de Armas. Closed to traffic, it is perpetually alive with street
        performers, vendors, commuters, and tourists. Flanking the paseo
        are department stores, banks, and some of the city's most iconic
        cultural institutions. Beneath the street runs the Baquedano metro
        interchange, one of the busiest transit nodes in South America.
"""

    let concertSpaces =
        [ ("Teatro Caupolicán", 5000, 88<quality>, Layouts.concertSpaceLayout3, zone.Id)
          ("Movistar Arena", 15000, 90<quality>, Layouts.concertSpaceLayout4, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let casinos =
        [ ("Casino Dreams Santiago", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCasino street.Id)

    let cinemas =
        [ ("Cine Arte Normandie", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCinema cityId street.Id)

    let hotels =
        [ ("Hotel Plaza San Francisco", 86<quality>, 200m<dd>, zone.Id)
          ("Grand Hyatt Santiago", 95<quality>, 380m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let metroStation =
        ("Baquedano Metro Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces casinos
        |> World.Street.addPlaces cinemas
        |> World.Street.addPlaces hotels
        |> World.Street.addPlace metroStation

    street, metroStation

let calleBandera (cityId: CityId) (zone: Zone) =
    let street =
        World.Street.create Ids.Street.calleBandera StreetType.OneWay
        |> World.Street.attachContext
            """
        Calle Bandera is one of the oldest streets in Santiago's historic
        centre, running alongside government buildings and civic institutions.
        The area mixes colonial architecture with small bars, independent
        eateries, and intimate music venues that have served the city's
        working crowd for generations. After dark, the street comes alive
        with a local music scene rooted in folk, cumbia, and rock.
"""

    let concertSpaces =
        [ ("Club Chocolate", 150, 78<quality>, Layouts.concertSpaceLayout1, zone.Id)
          ("Centro Cultural Gabriela Mistral (GAM)", 400, 86<quality>, Layouts.concertSpaceLayout2, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Bocanáriz", 88<quality>, zone.Id)
          ("Bar Nacional", 78<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("El Hoyo", 82<quality>, Chilean, zone.Id)
          ("Fuente Alemana", 80<quality>, Chilean, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces restaurants

let avenidaOHiggins (city: City) (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.avenidaOHiggins
            (StreetType.Split(East, 2))
        |> World.Street.attachContext
            """
        Avenida Libertador Bernardo O'Higgins, commonly known as La Alameda,
        is the main boulevard of Santiago and one of the widest avenues in
        South America. Lined with universities, cultural centres, hospitals,
        and monuments, it serves as both a physical and symbolic spine of
        the city. The avenue runs from the hills of Cerro San Cristóbal all
        the way to the western suburbs, carrying metro lines, buses, and
        the daily pulse of millions of Santiaguinos.
"""

    let hospitals =
        [ ("Hospital San Borja Arriarán", 84<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createHospital street.Id)

    let merchandiseWorkshops =
        [ ("Taller de Mercancía Alameda", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    let rehearsalSpaces =
        [ ("Sala de Ensayo Alameda", 78<quality>, 80m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let cafes =
        [ ("Café Literario Parque Bustamante", 82<quality>, zone.Id)
          ("Juan Valdez Café Alameda", 80<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let gyms =
        [ ("SportLife Alameda", 82<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    street
    |> World.Street.addPlaces hospitals
    |> World.Street.addPlaces merchandiseWorkshops
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces gyms

let createZone (city: City) =
    let santiagoCentroZone = World.Zone.create Ids.Zone.santiagoCentro

    let paseoAhumada, santiagoCentroMetroStation =
        paseoAhumada city.Id santiagoCentroZone

    let calleBandera = calleBandera city.Id santiagoCentroZone
    let avenidaOHiggins = avenidaOHiggins city santiagoCentroZone

    let station =
        { Lines = [ Red; Blue ]
          LeavesToStreet = paseoAhumada.Id
          PlaceId = santiagoCentroMetroStation.Id }

    santiagoCentroZone
    |> World.Zone.addStreet (World.Node.create paseoAhumada.Id paseoAhumada)
    |> World.Zone.addStreet (World.Node.create calleBandera.Id calleBandera)
    |> World.Zone.addStreet (World.Node.create avenidaOHiggins.Id avenidaOHiggins)
    |> World.Zone.connectStreets paseoAhumada.Id calleBandera.Id West
    |> World.Zone.connectStreets calleBandera.Id avenidaOHiggins.Id South
    |> World.Zone.addMetroStation station
