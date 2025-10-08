module rec Duets.Data.World.Cities.London.Greenwich

open Duets.Data.World.Cities
open Duets.Entities

let private greenwichMarket (zone: Zone) =
    let street =
        World.Street.create "Greenwich Market" (StreetType.Split(South, 2))
        |> World.Street.attachContext
            """
        Greenwich Market sits in the heart of historic Greenwich, surrounded by elegant
        Georgian and Victorian architecture. The covered market hall, dating from the 1830s,
        hosts independent craftspeople, antique dealers, and food vendors under its Victorian
        roof. Cobbled streets radiate outward to the Old Royal Naval College with its Painted
        Hall and twin baroque domes. The Cutty Sark clipper ship stands in dry dock nearby,
        its copper hull gleaming. Maritime heritage permeates the area, from nautical-themed
        pubs to the National Maritime Museum. The atmosphere blends tourist bustle with local
        community life, as students from Trinity Laban and Greenwich University mingle with
        visitors.
"""

    let shops =
        [ ("Greenwich Vintage", zone.Id); ("Market Antiques", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    let cafes =
        [ ("Heap's Sausage Cafe", 72<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let concertSpaces =
        [ ("Trinity Laban Hall",
           400,
           80<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let metroStation =
        ("Cutty Sark Station", zone.Id) |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces shops
        |> World.Street.addPlaces cafes
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlace metroStation

    street, metroStation

let private royalObservatory (zone: Zone) =
    let street =
        World.Street.create "Royal Observatory" StreetType.OneWay
        |> World.Street.attachContext
            """
        The Royal Observatory stands atop Greenwich Hill, its position chosen in 1675 for
        clear views of the stars and Thames. The Prime Meridian line divides the eastern
        and western hemispheres at this point, marked by a brass strip visitors straddle.
        Flamsteed House, Christopher Wren's original building, displays historic astronomical
        instruments and telescopes. The Great Equatorial Telescope dome dominates the skyline.
        Steep paths wind up through Greenwich Park from the market below, with commanding
        views across the Thames to Canary Wharf and the City. The hilltop location creates
        a quieter, more contemplative atmosphere than the busy market area.
"""

    let cafes =
        [ ("Astronomy Cafe", 68<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let bookstores =
        [ ("Greenwich Books", 75<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    street |> World.Street.addPlaces cafes |> World.Street.addPlaces bookstores

let private greenwichPark (zone: Zone) =
    let street =
        World.Street.create "Greenwich Park" StreetType.OneWay
        |> World.Street.attachContext
            """
        Greenwich Park slopes dramatically from the Royal Observatory hilltop down to the
        Thames, its 180 acres combining formal gardens with ancient woodland. Geometric
        avenues of chestnut trees frame views toward the Queen's House and Naval College.
        The park dates to 1433, making it London's oldest Royal Park, with veteran oaks and
        sweet chestnuts surviving from the 17th century. Deer roam in a small enclosure near
        the Wilderness area. On sunny days, locals gather on the hilltop for panoramic views.
        The park maintains a peaceful character despite heavy use, with distinct areas ranging
        from manicured flowerbeds to wild meadows.
"""

    let concertSpaces =
        [ ("Bandstand", 300, 70<quality>, Layouts.concertSpaceLayout1, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street |> World.Street.addPlaces concertSpaces

let zone =
    let zone = World.Zone.create "Greenwich"
    let greenwichMarket, metroStation = greenwichMarket zone
    let royalObservatory = royalObservatory zone
    let greenwichPark = greenwichPark zone

    let metroStation =
        { Lines = [ Blue ]
          LeavesToStreet = greenwichMarket.Id
          PlaceId = metroStation.Id }

    zone
    |> World.Zone.addStreet (
        World.Node.create greenwichMarket.Id greenwichMarket
    )
    |> World.Zone.addStreet (
        World.Node.create royalObservatory.Id royalObservatory
    )
    |> World.Zone.addStreet (World.Node.create greenwichPark.Id greenwichPark)
    |> World.Zone.connectStreets greenwichMarket.Id royalObservatory.Id North
    |> World.Zone.connectStreets greenwichMarket.Id greenwichPark.Id East
    |> World.Zone.addMetroStation metroStation
