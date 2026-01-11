module rec Duets.Data.World.Cities.Madrid.Centro

open Duets.Entities
open Duets.Entities.Calendar
open Duets.Data.World.Cities

let granVia (city: City) (zone: Zone) =
    let street =
        World.Street.create "Gran Vía" (StreetType.Split(East, 3))
        |> World.Street.attachContext
            """
        Madrid's most famous and bustling thoroughfare, often called the 'Spanish Broadway.'
        This wide avenue showcases early 20th-century architecture with grand buildings
        featuring ornate facades, domed roofs, and elaborate decorative elements in
        Art Deco and Beaux-Arts styles. Neon signs and digital billboards illuminate
        the street at night, advertising theaters, musicals, and shops. The sidewalks
        are constantly crowded with tourists, street performers, and locals. Historic
        cinemas and theaters with marquees line both sides, while the roar of traffic
        and street musicians create a constant urban symphony.
"""

    let merchandiseWorkshops =
        [ ("Gran Vía Merch", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    let hotels =
        [ ("Hotel Emperador", 90<quality>, 320m<dd>, zone.Id)
          ("Hotel Vincci", 85<quality>, 280m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let restaurants =
        [ ("Museo del Jamón", 80<quality>, Spanish, zone.Id)
          ("StreetXO", 92<quality>, Vietnamese, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Café de la Luz", 78<quality>, zone.Id)
          ("La Rollerie", 82<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let concertSpaces =
        [ ("Wurlitzer Ballroom",
           120,
           80<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id)
          ("Teatro Kapital",
           500,
           85<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id)
          ("WiZink Center",
           17000,
           92<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let radioStudios =
        [ ("Los40 Classic", 93<quality>, "Pop", zone.Id) ]
        |> List.map (PlaceCreators.createRadioStudio city street.Id)

    let metroStation =
        ("Gran Vía Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let cinemas =
        [ ("Cine Callao", 87<quality>, zone.Id)
          ("Capitol Cinema", 89<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCinema street.Id)

    let street =
        street
        |> World.Street.addPlaces merchandiseWorkshops
        |> World.Street.addPlaces hotels
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces cafes
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces radioStudios
        |> World.Street.addPlaces cinemas
        |> World.Street.addPlace metroStation

    street, metroStation

let puertaDelSol (zone: Zone) =
    let street =
        World.Street.create "Puerta del Sol" (StreetType.Split(South, 2))
        |> World.Street.attachContext
            """
        The symbolic heart of Madrid and one of the city's most iconic public squares.
        The semicircular plaza is paved with granite and surrounded by stately 18th and
        19th-century buildings with distinctive red facades and white trim. The famous
        clock tower of the Real Casa de Correos dominates one side, gathering crowds
        on New Year's Eve. Street performers, human statues, and artists fill the space,
        while tourists photograph the Bear and the Strawberry Tree statue. Ten streets
        radiate from the square, making it a constant hub of pedestrian activity and a
        traditional meeting point for Madrileños.
"""

    let restaurants =
        [ ("Casa Labra", 84<quality>, Spanish, zone.Id)
          ("La Mallorquina", 88<quality>, Spanish, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let bars =
        [ ("La Venencia", 80<quality>, zone.Id)
          ("El Tigre", 75<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let bookstores =
        [ ("Librería San Ginés", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let concertSpaces =
        [ ("Teatro Real",
           1700,
           98<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id)
          ("Sala El Sol", 400, 86<quality>, Layouts.concertSpaceLayout4, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces concertSpaces

let barrioDeLasLetras (zone: Zone) =
    let street =
        World.Street.create "Barrio de las Letras" (StreetType.Split(West, 2))
        |> World.Street.attachContext
            """
        The historic literary quarter where Spain's Golden Age writers once lived
        and worked. Narrow cobblestone streets wind between buildings from the
        16th and 17th centuries, many marked with plaques commemorating famous authors.
        Literary quotes from Cervantes, Lope de Vega, and Quevedo are inscribed in the
        pavement itself. The neighborhood has a bohemian atmosphere with intimate bookshops,
        antique stores, and cozy taverns tucked into centuries-old buildings.
        Wrought-iron balconies overflow with flowers, and hidden courtyards offer
        glimpses of Madrid's past.
"""

    let cafes =
        [ ("Café Central", 86<quality>, zone.Id)
          ("La Fídula", 79<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let studios =
        [ ("Estudio Uno",
           88<quality>,
           260m<dd>,
           (Character.from
               "Paco de Lucía"
               Male
               (Shorthands.Spring 21<days> 1947<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    street |> World.Street.addPlaces cafes |> World.Street.addPlaces studios

let calleArenal (zone: Zone) =
    let street =
        World.Street.create "Calle del Arenal" (StreetType.Split(West, 2))
        |> World.Street.attachContext
            """
        A historic pedestrian street connecting Puerta del Sol with the Royal Palace area.
        The street name references the sandy terrain that once characterized this area
        before Madrid's urban development. Buildings from the 18th and 19th centuries
        line both sides, many housing traditional shops, theaters, and entertainment venues.
        The street has a theatrical character with several performance spaces and nightlife
        establishments. Elegant storefronts display everything from traditional Spanish crafts
        to modern fashion, while street lamps cast warm light on the stone pavement during evening hours.
"""

    let concertSpaces =
        [ ("Joy Eslava", 800, 90<quality>, Layouts.concertSpaceLayout3, zone.Id)
          ("Costello Club",
           300,
           82<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let calleBarquillo (zone: Zone) =
    let street =
        World.Street.create "Calle del Barquillo" (StreetType.Split(North, 2))
        |> World.Street.attachContext
            """
        A charming street in the Chueca neighborhood known for its cultural venues and
        liberal atmosphere. The street features a mix of Belle Époque architecture and
        modernist facades, with decorative balconies and colorful building fronts. The
        area has been rejuvenated over recent decades, transforming into a hub for
        arts and entertainment. Cafés with outdoor seating attract a diverse, creative crowd,
        and small theaters and cultural centers contribute to the neighborhood's vibrant
        cultural scene. Rainbow flags often add splashes of color, reflecting
        the area's inclusive character.
"""

    let concertSpaces =
        [ ("Teatro Lara", 400, 89<quality>, Layouts.concertSpaceLayout3, zone.Id)
          ("Café Berlín", 400, 83<quality>, Layouts.concertSpaceLayout2, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let callePrincesa (zone: Zone) =
    let street =
        World.Street.create "Calle de la Princesa" (StreetType.Split(West, 2))
        |> World.Street.attachContext
            """
        A major commercial artery connecting central Madrid with the university district
        and western neighborhoods. The wide boulevard features a mix of early 20th-century
        buildings and modern structures, creating an eclectic architectural landscape.
        The street is lined with shops, department stores, and commercial centers
        that attract shoppers throughout the day. University students mingle with business
        professionals and tourists. The Conde Duque cultural center's imposing baroque
        facade stands as an architectural landmark. Trees provide shade along parts of the avenue,
        and the constant flow of buses and taxis emphasizes its role as a major urban corridor.
"""

    let concertSpaces =
        [ ("Teatro Barceló",
           900,
           92<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id)
          ("Café La Palma",
           300,
           80<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let calleAlcala (zone: Zone) =
    let street =
        World.Street.create "Calle de Alcalá" (StreetType.Split(East, 2))
        |> World.Street.attachContext
            """
        One of Madrid's longest and most historically significant streets, stretching
        from Puerta del Sol eastward. The avenue showcases spectacular architecture
        spanning several centuries, from baroque palaces to modernist buildings.
        The iconic Puerta de Alcalá, a neoclassical triumphal arch, stands as a
        monumental landmark. Grand institutional buildings, banks with imposing facades,
        and cultural venues line the broad boulevard. The street represents the expansion
        and grandeur of Madrid through different eras, with well-maintained historic
        buildings, wide sidewalks, and organized traffic lanes reflecting urban
        planning across generations.
"""

    let concertSpaces =
        [ ("Teatro de la Zarzuela",
           1300,
           96<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id)
          ("Teatro Nuevo Apolo",
           1200,
           88<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let createZone (city: City) =
    let centroZone = World.Zone.create "Centro"

    let granVia, metroStation = granVia city centroZone
    let puertaDelSol = puertaDelSol centroZone
    let barrioDeLasLetras = barrioDeLasLetras centroZone
    let calleArenal = calleArenal centroZone
    let calleBarquillo = calleBarquillo centroZone
    let callePrincesa = callePrincesa centroZone
    let calleAlcala = calleAlcala centroZone

    let station =
        { Lines = [ Blue; Red ]
          LeavesToStreet = granVia.Id
          PlaceId = metroStation.Id }

    centroZone
    |> World.Zone.addStreet (World.Node.create granVia.Id granVia)
    |> World.Zone.addStreet (World.Node.create puertaDelSol.Id puertaDelSol)
    |> World.Zone.addStreet (
        World.Node.create barrioDeLasLetras.Id barrioDeLasLetras
    )
    |> World.Zone.addStreet (World.Node.create calleArenal.Id calleArenal)
    |> World.Zone.addStreet (World.Node.create calleBarquillo.Id calleBarquillo)
    |> World.Zone.addStreet (World.Node.create callePrincesa.Id callePrincesa)
    |> World.Zone.addStreet (World.Node.create calleAlcala.Id calleAlcala)
    |> World.Zone.connectStreets granVia.Id puertaDelSol.Id South
    |> World.Zone.connectStreets puertaDelSol.Id barrioDeLasLetras.Id West
    |> World.Zone.connectStreets barrioDeLasLetras.Id calleArenal.Id North
    |> World.Zone.connectStreets calleArenal.Id calleBarquillo.Id East
    |> World.Zone.connectStreets calleBarquillo.Id callePrincesa.Id South
    |> World.Zone.connectStreets callePrincesa.Id calleAlcala.Id East
    |> World.Zone.addMetroStation station

let zone = createZone
