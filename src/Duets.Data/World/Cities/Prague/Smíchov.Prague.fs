module rec Duets.Data.World.Cities.Prague.Smíchov

open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let plzeňská (zone: Zone) =
    let street =
        World.Street.create "Plzeňská" (StreetType.Split(East, 2))
        |> World.Street.attachContext
            """
        Plzeňská is a major commercial corridor in Smíchov, centered around the
        bustling Anděl district with its modern shopping centers and office towers.
        The street underwent significant redevelopment in the late 1990s, transforming
        from an industrial area into a contemporary commercial hub. The iconic
        "dancing house" (though on nearby Rašínovo nábřeží) and the golden Anděl
        metro station tower are visible landmarks. Wide pedestrian areas accommodate
        heavy foot traffic, while tram lines run down the center. The atmosphere
        is energetic and modern, with international businesses, chain restaurants,
        and a younger demographic defining the area.
"""

    let bars =
        [ ("The Pub Praha 5", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("El Sabor Mexicano", 89<quality>, Mexican, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let metroStation =
        ("Anděl Station", zone.Id) |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlace metroStation

    street, metroStation

let štefánikova city (zone: Zone) =
    let street =
        World.Street.create "Štefánikova" StreetType.OneWay
        |> World.Street.attachContext
            """
        Štefánikova runs through a mixed-use area of Smíchov, combining residential
        buildings with commercial establishments and fitness centers. The street
        features turn-of-the-century apartment blocks with decorative facades alongside
        more modern developments. Tree-lined sections provide a pleasant walking environment,
        while local businesses including Vietnamese restaurants reflect Prague's diverse
        community. The area maintains a lived-in, authentic neighborhood character
        despite Smíchov's ongoing modernization. Side streets reveal glimpses of older
        Prague, with courtyard entrances and period architectural details.
"""

    let bars =
        [ ("BackDoors Bar", 84<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("Modrý zub", 87<quality>, Vietnamese, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let gyms =
        [ ("John Reed Fitness", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let recordingStudios =
        [ ("Smíchov Sound Studio",
           87<quality>,
           220m<dd>,
           (Character.from
               "Tomáš Novák"
               Male
               (Shorthands.Winter 5<days> 1980<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    street
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces gyms
    |> World.Street.addPlaces recordingStudios

let keSklárně (zone: Zone) =
    let street =
        World.Street.create "Ke Sklárně" StreetType.OneWay
        |> World.Street.attachContext
            """
        Ke Sklárně is an industrial street leading to MeetFactory, David Černý's
        renowned contemporary art and music venue housed in a former glass factory.
        The street retains its industrial character with warehouse buildings,
        loading docks, and utilitarian architecture. Graffiti and street art adorn
        various walls, giving the area an underground, alternative atmosphere.
        The proximity to railway tracks and the Smíchov rail yards adds to the gritty aesthetic.
        This is the edgier side of Prague's cultural scene, attracting experimental
        artists and alternative music lovers.
"""

    let concertSpaces =
        [ ("MeetFactory", 500, 88<quality>, Layouts.concertSpaceLayout2, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let janáčkovoNábřeží (zone: Zone) =
    let street =
        World.Street.create Ids.Street.janáčkovoNábřeží StreetType.OneWay
        |> World.Street.attachContext
            """
        Janáčkovo nábřeží runs along the Vltava River embankment, offering panoramic
        views of the water and Prague Castle across the river. The riverside promenade
        is popular with joggers, cyclists, and evening strollers. Jazz Dock, a
        floating jazz club, is moored along this stretch, its distinctive boat structure
        visible from the waterfront path. Houseboats and river cruisers dock nearby,
        adding maritime character. The atmosphere is relaxed and scenic, especially
        at sunset when the castle is illuminated. Plane trees line sections of the embankment,
        providing shade in summer.
"""

    let concertSpaces =
        [ ("Jazz Dock", 150, 95<quality>, Layouts.concertSpaceLayout2, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let zborovská (zone: Zone) =
    let street =
        World.Street.create "Zborovská" StreetType.OneWay
        |> World.Street.attachContext
            """
        Zborovská climbs uphill through residential Smíchov, connecting the riverfront
        to higher elevations with views over the district. The street features predominantly
        late 19th and early 20th-century apartment buildings with Art Nouveau and
        eclectic architectural elements. Local shops, small cafes, and the Futurum Music Bar
        serve the neighborhood. Tram lines run along the street, providing convenient
        transport connections. The area has a quieter, more residential feel compared
        to the commercial bustle of Anděl, preserving an older Prague neighborhood atmosphere.
"""

    let concertSpaces =
        [ ("Futurum Music Bar",
           650,
           89<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let createZone city =
    let smíchovZone = World.Zone.create Ids.Zone.smíchov

    let plzeňská, metroStation = plzeňská smíchovZone
    let štefánikova = štefánikova city smíchovZone
    let zborovská = zborovská smíchovZone
    let janáčkovoNábřeží = janáčkovoNábřeží smíchovZone
    let keSklárně = keSklárně smíchovZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = plzeňská.Id
          PlaceId = metroStation.Id }

    smíchovZone
    |> World.Zone.addStreet (World.Node.create plzeňská.Id plzeňská)
    |> World.Zone.addStreet (World.Node.create štefánikova.Id štefánikova)
    |> World.Zone.addStreet (World.Node.create zborovská.Id zborovská)
    |> World.Zone.addStreet (
        World.Node.create janáčkovoNábřeží.Id janáčkovoNábřeží
    )
    |> World.Zone.addStreet (World.Node.create keSklárně.Id keSklárně)
    |> World.Zone.connectStreets plzeňská.Id štefánikova.Id North
    |> World.Zone.connectStreets plzeňská.Id zborovská.Id East
    |> World.Zone.connectStreets plzeňská.Id janáčkovoNábřeží.Id South
    |> World.Zone.connectStreets štefánikova.Id keSklárně.Id South
    |> World.Zone.connectStreets štefánikova.Id zborovská.Id East

    |> World.Zone.addMetroStation station
