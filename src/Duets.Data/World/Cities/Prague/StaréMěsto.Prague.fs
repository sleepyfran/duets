module rec Duets.Data.World.Cities.Prague.StaréMěsto

open Duets.Data.World.Cities.Prague
open Duets.Data.World.Cities
open Duets.Entities

let staroměstskéNáměstí (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.staroměstskéNáměstí
            (StreetType.Split(South, 3))
        |> World.Street.attachContext
            """
        Old Town Square is Prague's historic centerpiece, dominated by the Gothic
        towers of the Church of Our Lady before Týn and the medieval Astronomical Clock
        on the Old Town Hall. The square is surrounded by colorful baroque facades,
        Gothic and Romanesque architectural foundations, and the Art Nouveau Kinský Palace.
        The Jan Hus memorial stands at the center, while the square hosts seasonal markets
        including the famous Christmas market. Street performers, horse-drawn carriages,
        and outdoor café terraces create a vibrant, tourist-filled atmosphere.
        Every hour, crowds gather before the Astronomical Clock to watch its
        mechanical apostles parade.
"""

    let bars =
        [ ("The Dubliner Irish Bar", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("U Prince", 92<quality>, Czech, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let hotels =
        [ ("Grand Hotel Praha", 93<quality>, 250m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    street
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces hotels

let kaprova (zone: Zone) =
    let street =
        World.Street.create "Kaprova" (StreetType.OneWay)
        |> World.Street.attachContext
            """
        Kaprova connects the Old Town to the historic Jewish Quarter (Josefov),
        featuring a mix of baroque buildings and early 20th-century architecture.
        The street provides access to the Staroměstská metro station and serves
        as a busy pedestrian thoroughfare. Buildings display a variety of architectural
        styles, from medieval foundations to Art Nouveau facades. The atmosphere
        is cosmopolitan, with tourists navigating between the Old Town Square
        and the Jewish Quarter's synagogues. Small shops and cafes occupy ground
        floors of historic structures.
"""

    let metroStation =
        ("Staroměstská Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street = street |> World.Street.addPlace metroStation

    street, metroStation

let jilská (zone: Zone) =
    let street =
        World.Street.create "Jilská" (StreetType.OneWay)
        |> World.Street.attachContext
            """
        Jilská is a narrow, winding medieval street connecting the Old Town Square
        to the southern parts of Staré Město. Historic buildings with Gothic cellars
        and Renaissance facades line both sides, creating an intimate, historic atmosphere.
        The street retains its medieval layout with irregular building lines and
        cobblestone paving. Jazz clubs in historic cellars attract music lovers
        in the evenings. During the day, the street is relatively quiet compared
        to main tourist routes, offering glimpses of authentic old Prague architecture.
"""

    let concertSpaces =
        [ ("Jazz Republic",
           120,
           88<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let dlouhá (cityId: CityId) (zone: Zone) =
    let street =
        World.Street.create Ids.Street.dlouhá (StreetType.OneWay)
        |> World.Street.attachContext
            """
        Dlouhá (meaning "Long Street") runs from Old Town Square northward, transforming
        from tourist-oriented shops near the square to a trendy nightlife district
        further along. The street features beautifully restored Renaissance and
        baroque buildings with sgraffito decorations and painted facades.
        Art galleries, design shops, and contemporary bars occupy historic structures.
        The Roxy club, housed in a former cinema, anchors the nightlife scene.
        The atmosphere shifts from day to night, from cultural browsing to Prague's
        alternative clubbing scene.
"""

    let concertSpaces =
        [ ("Roxy Prague", 900, 90<quality>, Layouts.concertSpaceLayout3, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let cinemas =
        [ ("Bio Oko", 85<quality>, zone.Id)
          ("Kino Světozor", 83<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCinema cityId street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces cinemas

let karlova (zone: Zone) =
    let street =
        World.Street.create Ids.Street.karlova (StreetType.Split(West, 2))
        |> World.Street.attachContext
            """
        Karlova is the historic Royal Route connecting Old Town Square to Charles
        Bridge, one of Prague's most tourist-dense streets. The winding medieval
        street features baroque facades, Renaissance portals, and numerous souvenir
        shops and cafes. The Klementinum complex, a massive baroque library and
        astronomical tower, dominates one section. Buildings display house
        signs (traditional pre-numbering identifiers) including the famous Golden Well.
        The street narrows and curves, following its medieval layout, creating
        intimate architectural vistas. Constant tourist traffic gives it a commercial,
        bustling character year-round.
"""

    let concertSpaces =
        [ ("Klementinum Mirror Chapel",
           200,
           94<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Anonymous Bar", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("Terasa U Zlaté studně", 95<quality>, French, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let rehearsalSpaces =
        [ ("Old Town Rehearsal", 85<quality>, 130m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces rehearsalSpaces

let createZone (city: City) =
    let staréMěstoZone = World.Zone.create Ids.Zone.staréMěsto

    let staroměstskéNáměstí = staroměstskéNáměstí staréMěstoZone

    let karlova = karlova staréMěstoZone
    let dlouhá = dlouhá city.Id staréMěstoZone
    let jilská = jilská staréMěstoZone
    let kaprova, metroStation = kaprova staréMěstoZone

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = kaprova.Id
          PlaceId = metroStation.Id }

    staréMěstoZone
    |> World.Zone.addStreet (
        World.Node.create staroměstskéNáměstí.Id staroměstskéNáměstí
    )
    |> World.Zone.addStreet (World.Node.create karlova.Id karlova)
    |> World.Zone.addStreet (World.Node.create dlouhá.Id dlouhá)
    |> World.Zone.addStreet (World.Node.create jilská.Id jilská)
    |> World.Zone.addStreet (World.Node.create kaprova.Id kaprova)
    |> World.Zone.connectStreets staroměstskéNáměstí.Id karlova.Id East
    |> World.Zone.connectStreets staroměstskéNáměstí.Id dlouhá.Id North
    |> World.Zone.connectStreets staroměstskéNáměstí.Id jilská.Id South
    |> World.Zone.connectStreets staroměstskéNáměstí.Id kaprova.Id West

    |> World.Zone.addMetroStation station
