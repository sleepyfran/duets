module Duets.Data.World.Cities.Berlin.Mitte

open Duets.Data.World.Cities.Berlin
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let unterDenLinden (cityId: CityId) (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.unterDenLinden
            (StreetType.Split(East, 2))
        |> World.Street.attachContext
            """
        Berlin's most celebrated boulevard, stretching from the Brandenburg Gate
        to Museum Island. Lined with linden trees, historic palaces, embassies,
        and cultural institutions, it is the ceremonial heart of the German capital.
        The neoclassical Staatsoper Unter den Linden, the Humboldt University, and the
        New Guardhouse (Neue Wache) define its character — a mix of imperial grandeur
        and vibrant contemporary life. The Brandenburg Gate stands at the western end
        as an iconic symbol of German reunification.
"""

    let concertSpaces =
        [ ("Staatsoper Unter den Linden",
           1396,
           96<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id)
          ("Konzerthaus Berlin",
           1400,
           94<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let restaurants =
        [ ("Borchardt", 95<quality>, German, zone.Id)
          ("Café Einstein Stammhaus", 90<quality>, German, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let hotels =
        [ ("Hotel Adlon Kempinski", 98<quality>, 700m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let cinemas =
        [ ("Kino International", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCinema cityId street.Id)

    let metroStation =
        ("Brandenburger Tor", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces hotels
        |> World.Street.addPlaces cinemas
        |> World.Street.addPlace metroStation

    street, metroStation

let alexanderplatz (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.alexanderplatz
            (StreetType.Split(North, 2))
        |> World.Street.attachContext
            """
        Alexanderplatz — or "Alex" as locals call it — is the beating public heart
        of East Berlin. Dominated by the towering TV Tower (Fernsehturm), the largest
        structure in Germany, this vast open square is surrounded by brutalist GDR-era
        architecture, modern shopping centres, and lively street life. The iconic
        World Time Clock is a popular meeting point. Markets, street performers, and
        an endless flow of Berliners and tourists make Alex one of the city's
        most energetic spaces.
"""

    let bars =
        [ ("Park Inn Rooftop Bar", 88<quality>, zone.Id)
          ("The Alex Pub", 82<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let cafes =
        [ ("Café Oliv", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let bookstores =
        [ ("Hugendubel Berlin", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let merchandiseWorkshops =
        [ ("Saturn Electronics", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    street
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces merchandiseWorkshops

let friedrichstrasse (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.friedrichstrasse
            StreetType.OneWay
        |> World.Street.attachContext
            """
        Friedrichstraße runs north-south through Mitte, once divided by the Berlin Wall.
        Today it's a prestigious shopping and entertainment corridor, home to high-end
        boutiques, the historic Friedrichstadt-Palast variety theatre, and several
        significant cultural venues. The area around Checkpoint Charlie — the famous
        Cold War crossing point — lies just to the south, lending historical weight
        to this thoroughly modern street.
"""

    let concertSpaces =
        [ ("Friedrichstadt-Palast",
           1895,
           93<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let casinos =
        [ ("Spielbank Berlin", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCasino street.Id)

    let carDealers =
        [ ("Mercedes-Benz Gallery Berlin",
           97<quality>,
           zone.Id,
           { Dealer =
               (Character.from
                   "Klaus Müller"
                   Male
                   (Shorthands.Spring 5<days> 1971<years>))
             PriceRange = CarPriceRange.Premium }) ]
        |> List.map (PlaceCreators.createCarDealer street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces casinos
    |> World.Street.addPlaces carDealers

let createZone (cityId: CityId) =
    let mitteZone = World.Zone.create Ids.Zone.mitte

    let unterDenLinden, unterDenLindenStation = unterDenLinden cityId mitteZone
    let alexanderplatz = alexanderplatz mitteZone
    let friedrichstrasse = friedrichstrasse mitteZone

    let station =
        { Lines = [ Red; Blue ]
          LeavesToStreet = unterDenLinden.Id
          PlaceId = unterDenLindenStation.Id }

    mitteZone
    |> World.Zone.addStreet (World.Node.create unterDenLinden.Id unterDenLinden)
    |> World.Zone.addStreet (World.Node.create alexanderplatz.Id alexanderplatz)
    |> World.Zone.addStreet (World.Node.create friedrichstrasse.Id friedrichstrasse)
    |> World.Zone.connectStreets unterDenLinden.Id alexanderplatz.Id East
    |> World.Zone.connectStreets alexanderplatz.Id friedrichstrasse.Id North
    |> World.Zone.addMetroStation station
