module rec Duets.Data.World.Cities.NewYork.LowerManhattan

open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let private bleeckerStreet (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.bleeckerStreet
            (StreetType.Split(East, 3))

    let bookstores =
        [ ("McNally Jackson Books", 91<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let concerts =
        [ ("The Bitter End",
           400,
           87<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id)
          ("(Le) Poisson Rouge",
           700,
           89<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id)
          ("The Lynn Redgrave Theater",
           350,
           85<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let restaurants =
        [ ("John's of Bleecker Street", 88<quality>, Italian, zone.Id)
          ("Kesté Pizza & Vino", 86<quality>, Italian, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    street
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces concerts
    |> World.Street.addPlaces restaurants
    |> World.Street.attachContext
        """
    Bleecker Street in Greenwich Village is a bohemian enclave with narrow sidewalks,
    historic brownstones, and small storefronts that have survived gentrification.
    The street has an artistic, countercultural legacy visible in its independent
    music venues, vintage record shops, and poetry bookstores. Trees arch over the
    street creating dappled shade, while the aroma of wood-fired pizza wafts from
    corner restaurants. Jazz and folk music sometimes drift from open doorways,
    and locals sit on stoops chatting with neighbors. The area maintains a village-like
    intimacy despite being in the heart of Manhattan, with cobblestone side streets
    and iron fire escapes adorning brick facades.
"""

let private bowery (zone: Zone) =
    let street =
        World.Street.create Ids.Street.bowery (StreetType.Split(North, 3))

    let bookstores =
        [ ("Strand Bookstore", 94<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let concerts =
        [ ("Bowery Ballroom",
           575,
           90<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    // TODO: Add New Museum once we support art museums

    let bars =
        [ ("Capitale", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let shops =
        [ ("John Varvatos Store", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    let hotels =
        [ ("The Bowery Hotel", 92<quality>, 450m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    street
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces concerts
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces shops
    |> World.Street.addPlaces hotels
    |> World.Street.attachContext
        "The Bowery has transformed from its gritty past into a corridor of luxury hotels and contemporary art spaces, though echoes of its historic character remain. Cast-iron buildings stand alongside modern glass and steel structures, creating an architectural timeline of New York's evolution. The street is wider than typical Manhattan avenues, allowing more light to reach the pavement. Street art and murals still appear on select walls, nodding to the area's punk rock heritage. The New Museum's stacked-box architecture dominates the skyline, while boutique retailers occupy ground floors of renovated industrial buildings. The atmosphere mixes artistic credibility with upscale development."

let private irvingPlace city (zone: Zone) =
    let street =
        World.Street.create Ids.Street.irvingPlace (StreetType.Split(North, 3))

    let recordingStudios =
        [ ("Electric Lady Studios",
           96<quality>,
           350m<dd>,
           (Character.from
               "Steve Rosenthal"
               Male
               (Shorthands.Winter 15<days> 1965<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let concerts =
        [ ("Irving Plaza",
           1025,
           88<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Pete's Tavern", 84<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let hotels =
        [ ("The W New York – Union Square", 90<quality>, 400m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let hospitals =
        [ ("NewYork-Presbyterian Lower Manhattan Hospital", 93<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createHospital street.Id)

    // TODO: Add Union Square Park once we support parks

    let metroStation =
        ("Union Square Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces recordingStudios
        |> World.Street.addPlaces concerts
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces hotels
        |> World.Street.addPlaces hospitals
        |> World.Street.addPlace metroStation
        |> World.Street.attachContext
            """
        Irving Place is a short, tree-lined street with a residential character unusual
        for its proximity to Union Square. Historic brick townhouses with ornate cornices
        and wrought-iron railings create an elegant streetscape reminiscent of old
        New York. Gas lamps (now electric) still line the sidewalk, and small parks
        with benches offer quiet respite. The street maintains a neighborhood feel
        with locals walking dogs and reading newspapers at sidewalk cafes.
        Gramercy Park's exclusive, gated greenery lies just blocks away, lending the
        area an air of privilege. The architecture dates primarily from the
        19th century, with well-preserved facades and original details.
"""

    street, metroStation

let private lowerEastSide (city: City) (zone: Zone) =
    let street =
        World.Street.create Ids.Street.lowerEastSide (StreetType.Split(East, 2))

    let concerts =
        [ ("Mercury Lounge",
           250,
           88<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id)
          ("The Red Room at KGB Bar",
           80,
           85<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let radioStudios =
        [ ("WBAI (Pacifica)", 86<quality>, "Jazz", zone.Id) ]
        |> List.map (PlaceCreators.createRadioStudio city street.Id)

    street
    |> World.Street.addPlaces concerts
    |> World.Street.addPlaces radioStudios
    |> World.Street.attachContext
        """
    The Lower East Side retains its immigrant heritage character with tenement
    buildings, fire escapes zigzagging down facades, and small shops at street level.
    The neighborhood has gentrified but still shows its roots in kosher delis,
    old synagogues, and discount retailers alongside newer cocktail bars and indie
    music venues. Graffiti and street art cover many walls, reflecting the
    area's punk and hip-hop history. The streets are narrower here, creating
    intimate urban canyons where laundry sometimes still hangs from windows.
    Multicultural restaurants, vintage clothing stores, and underground performance spaces
    give the area a creative, alternative edge that attracts artists and musicians.
"""

let createZone (city: City) =
    let lowerManhattanZone = World.Zone.create Ids.Zone.lowerManhattan

    let bleeckerStreet = bleeckerStreet lowerManhattanZone
    let bowery = bowery lowerManhattanZone
    let irvingPlace, unionSquareMetro = irvingPlace city lowerManhattanZone
    let lowerEastSide = lowerEastSide city lowerManhattanZone

    let unionSquareMetroStation =
        { Lines = [ Blue ]
          LeavesToStreet = irvingPlace.Id
          PlaceId = unionSquareMetro.Id }

    lowerManhattanZone
    |> World.Zone.addStreet (World.Node.create bleeckerStreet.Id bleeckerStreet)
    |> World.Zone.addStreet (World.Node.create bowery.Id bowery)
    |> World.Zone.addStreet (World.Node.create irvingPlace.Id irvingPlace)
    |> World.Zone.addStreet (World.Node.create lowerEastSide.Id lowerEastSide)
    |> World.Zone.connectStreets bleeckerStreet.Id bowery.Id North
    |> World.Zone.connectStreets bowery.Id irvingPlace.Id North
    |> World.Zone.connectStreets bowery.Id lowerEastSide.Id East
    |> World.Zone.addMetroStation unionSquareMetroStation
