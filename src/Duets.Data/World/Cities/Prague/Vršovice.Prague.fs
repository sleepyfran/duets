module rec Duets.Data.World.Cities.Prague.Vršovice

open Duets.Data.World.Cities.Prague
open Duets.Data.World.Cities
open Duets.Entities

let krymská (zone: Zone) =
    let street =
        World.Street.create Ids.Street.krymská (StreetType.Split(East, 1))
        |> World.Street.attachContext
            """
        Krymská is the bohemian heart of Vršovice, known for its alternative culture,
        street art, and eclectic mix of venues. The street features a combination of early 20th-century
        apartment buildings, some renovated and others retaining their weathered charm.
        Independent cafes, vintage shops, and small music venues like Café v lese give the
        area its artistic character. Graffiti and murals adorn walls, reflecting the
        neighborhood's creative spirit. The atmosphere is young, casual, and slightly edgy,
        attracting artists, musicians, and students. Weekend evenings bring crowds to the
        bars and small concert spaces.
"""

    let bookstores =
        [ ("Shakespeare and Sons", 92<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let concertSpaces =
        [ ("Café v lese", 150, 85<quality>, Layouts.concertSpaceLayout1, zone.Id)
          ("Bad Flash Bar",
           100,
           82<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let restaurants =
        [ ("Café Sladkovský", 86<quality>, Czech, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    street
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces restaurants

let moskevská (zone: Zone) =
    let street =
        World.Street.create "Moskevská" (StreetType.Split(West, 2))
        |> World.Street.attachContext
            """
        Moskevská runs through a quieter residential section of Vršovice,
        lined with apartment buildings from various periods including pre-war
        and socialist-era construction. The street has a working-class character
        with local pubs like Waldeska serving as community gathering spots.
        Small shops and basic services cater to residents rather than tourists.
        Tram lines provide connections to other parts of Prague. The area retains an authentic,
        lived-in quality with less gentrification than neighboring Vinohrady.
        Side streets reveal courtyards and glimpses of everyday Prague life away from tourist areas.
"""

    let concertSpaces =
        [ ("Waldeska Pub", 80, 78<quality>, Layouts.concertSpaceLayout1, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let restaurants =
        [ ("U Veverky", 84<quality>, Czech, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces restaurants

let vyšehradská (zone: Zone) =
    let street =
        World.Street.create "Vyšehradská" (StreetType.Split(South, 2))
        |> World.Street.attachContext
            """
        Vyšehradská street connects Vršovice to the historic Vyšehrad fortress area,
        running along the southern edge of the district. The street features a mix of
        late 19th-century apartment buildings and some early modernist architecture
        from the interwar period. Wide sidewalks accommodate pedestrian traffic
        heading to and from the metro station. Local shops, pharmacies, and
        small businesses serve the residential community. Tram lines run along
        the street, creating a constant hum of urban transport. The area has a practical,
        transit-oriented character, with commuters passing through alongside neighborhood
        residents going about their daily routines.
"""

    let metroStation =
        ("Vyšehrad Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street = street |> World.Street.addPlace metroStation

    street, metroStation

let zone =
    let vršoviceZone = World.Zone.create Ids.Zone.vršovice
    let krymská = krymská vršoviceZone
    let moskevská = moskevská vršoviceZone
    let vyšehradská, metroStation = vyšehradská vršoviceZone

    let station =
        { Lines = [ Red ]
          LeavesToStreet = vyšehradská.Id
          PlaceId = metroStation.Id }

    vršoviceZone
    |> World.Zone.addStreet (World.Node.create krymská.Id krymská)
    |> World.Zone.addStreet (World.Node.create moskevská.Id moskevská)
    |> World.Zone.addStreet (World.Node.create vyšehradská.Id vyšehradská)
    |> World.Zone.connectStreets krymská.Id moskevská.Id North
    |> World.Zone.connectStreets moskevská.Id vyšehradská.Id South
    |> World.Zone.addMetroStation station
