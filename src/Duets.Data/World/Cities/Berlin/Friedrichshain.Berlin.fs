module Duets.Data.World.Cities.Berlin.Friedrichshain

open Duets.Data.World.Cities.Berlin
open Duets.Data.World.Cities
open Duets.Entities

let warschauerStrasse (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.warschauerStrasse
            (StreetType.Split(East, 2))
        |> World.Street.attachContext
            """
        Warschauer Straße is the nerve centre of Berlin's legendary club scene.
        The Oberbaumbrücke, the double-deck bridge connecting Friedrichshain and
        Kreuzberg across the Spree, is visible from here. The street and its
        surroundings host some of the world's most famous nightclubs — including
        Berghain, whose disused power station looms over the nearby rail yard —
        as well as the Mercedes-Benz Arena for major concerts and the East Side
        Gallery, the longest remaining section of the Berlin Wall, now an open-air
        mural museum.
"""

    let concertSpaces =
        [ ("Berghain",
           1500,
           97<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id)
          ("Mercedes-Benz Arena",
           17000,
           95<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id)
          ("Astra Kulturhaus",
           1400,
           88<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Hops & Barley", 87<quality>, zone.Id)
          ("BarCom", 83<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let hotels =
        [ ("nhow Berlin", 88<quality>, 250m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let metroStation =
        ("S+U Warschauer Straße Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces hotels
        |> World.Street.addPlace metroStation

    street, metroStation

let simonDachStrasse (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.simonDachStrasse
            StreetType.OneWay
        |> World.Street.attachContext
            """
        Simon-Dach-Straße is the social hub of the "Kiez" (neighbourhood) in
        Friedrichshain, packed with independent bars, restaurants, and cafés spilling
        onto the pavement. Named after a 17th-century poet, the street has a festive,
        laid-back atmosphere that draws locals and visitors alike. On warm evenings
        it becomes one long outdoor party, with tables and chairs extending across
        the wide pavement and music drifting from every doorway.
"""

    let concertSpaces =
        [ ("Cassiopeia",
           150,
           85<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let cafes =
        [ ("Café Lois", 84<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let restaurants =
        [ ("Schnitzelei Friedrichshain", 87<quality>, German, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces restaurants

let createZone =
    let friedrichshainZone = World.Zone.create Ids.Zone.friedrichshain

    let warschauerStrasse, warschauerStation = warschauerStrasse friedrichshainZone
    let simonDachStrasse = simonDachStrasse friedrichshainZone

    let station =
        { Lines = [ Red ]
          LeavesToStreet = warschauerStrasse.Id
          PlaceId = warschauerStation.Id }

    friedrichshainZone
    |> World.Zone.addStreet (World.Node.create warschauerStrasse.Id warschauerStrasse)
    |> World.Zone.addStreet (World.Node.create simonDachStrasse.Id simonDachStrasse)
    |> World.Zone.connectStreets warschauerStrasse.Id simonDachStrasse.Id North
    |> World.Zone.addMetroStation station
