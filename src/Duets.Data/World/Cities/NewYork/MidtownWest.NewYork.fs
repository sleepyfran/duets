module rec Duets.Data.World.Cities.NewYork.MidtownWest

open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let private broadway (city: City) (zone: Zone) =
    let street =
        World.Street.create Ids.Street.broadway (StreetType.Split(North, 3))

    let home = PlaceCreators.createHome street.Id zone.Id

    let gyms =
        [ ("Equinox Times Square", 93<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let concerts =
        [ ("The Majestic Theatre",
           1500,
           95<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id)
          ("The Gershwin Theatre",
           1900,
           94<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("TSX Broadway", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let shops =
        [ ("Macy's Herald Square", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    let metroStation =
        ("Times Square Station", zone.Id) |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlace home
        |> World.Street.addPlaces gyms
        |> World.Street.addPlaces concerts
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces shops
        |> World.Street.addPlace metroStation
        |> World.Street.attachContext
            """
        Broadway in the Theater District is the beating heart of New York's entertainment scene,
        lined with grand Broadway theaters featuring elaborate marquees and towering vertical signs.
        The street buzzes with constant activity - tourists, theatergoers, street performers,
        and vendors create a chaotic symphony of movement and sound. Times Square's
        massive digital billboards cast colorful light across the pavement even in daylight,
        while the iconic TKTS booth's red glass stairs serve as an impromptu gathering place.
        The area never truly sleeps, with neon glowing against Art Deco facades
        and steam rising from subway grates below.
"""

    street, metroStation

let private seventhAvenue (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.seventhAvenue
            (StreetType.Split(North, 2))

    let concerts =
        [ ("Madison Square Garden",
           20000,
           98<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Mustang Harry's", 84<quality>, zone.Id)
          ("Juniper Bar & Grill", 82<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let rehearsalSpaces =
        [ ("SIR Studios", 92<quality>, 200m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let street =
        street
        |> World.Street.addPlaces concerts
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces rehearsalSpaces
        |> World.Street.attachContext
            """
        7th Avenue near 34th Street is dominated by the cylindrical form of Madison
        Square Garden rising above Penn Station, creating a major transit and entertainment
        hub. The wide avenue channels rivers of commuters and tourists between the
        sidewalks, while the Penn Plaza towers loom overhead. Street vendors sell
        hot dogs and pretzels from their carts, competing with the smell of exhaust
        and subway air. The area has a working-class energy, with sports fans,
        daily commuters, and businesspeople sharing the sidewalks beneath the
        shadow of MSG's distinctive architecture.
"""

    street

let private fiftySeventhStreet (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.fiftySeventhStreet
            (StreetType.Split(East, 2))

    let concerts =
        [ ("Carnegie Hall",
           2800,
           99<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let restaurants =
        [ ("The Tea Room", 93<quality>, Czech, zone.Id)
          ("Brooklyn Diner", 85<quality>, American, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Carnegie Diner & Cafe", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlaces concerts
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces cafes
    |> World.Street.attachContext
        """
    57th Street near 7th Avenue is part of Billionaires' Row, where gleaming
    residential supertall towers pierce the Manhattan skyline. Carnegie Hall's
    terra cotta facade and arched windows stand as a historical anchor among the modern
    luxury developments. The street has an upscale atmosphere with well-dressed
    pedestrians, luxury car services idling at curbs, and high-end restaurant awnings
    extending over the sidewalk. Ornate street lamps and carefully maintained trees
    soften the canyon of buildings, while the occasional glimpse of Central Park
    can be caught between the towers to the north.
"""

let private sixthAvenue (city: City) (zone: Zone) =
    let street = World.Street.create Ids.Street.sixthAvenue StreetType.OneWay

    let recordingStudios =
        [ ("Avatar Studios",
           94<quality>,
           400m<dd>,
           (Character.from
               "Roy Hendrickson"
               Male
               (Shorthands.Spring 10<days> 1968<years>)),
           zone.Id)
          ("Sear Sound",
           95<quality>,
           420m<dd>,
           (Character.from
               "Roberta Findlay"
               Female
               (Shorthands.Summer 22<days> 1970<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let radioStudios =
        [ ("Z100 (WHTZ-FM)", 94<quality>, "Pop", zone.Id)
          ("Q104.3 (WAXQ-FM)", 92<quality>, "Rock", zone.Id) ]
        |> List.map (PlaceCreators.createRadioStudio city street.Id)

    street
    |> World.Street.addPlaces recordingStudios
    |> World.Street.addPlaces radioStudios
    |> World.Street.attachContext
        """
    6th Avenue in Midtown is a major commercial corridor lined with towering office
    buildings and media headquarters. The Avenue of the Americas, as it's officially known,
    features uniform street lamps and small planted areas that do little to soften
    the corporate atmosphere. Radio towers and satellite dishes are visible atop several
    buildings, marking the presence of broadcast studios. The sidewalks are
    crowded with office workers during rush hours, while food trucks cluster at
    corners serving quick lunches to the business crowd. The street has a purposeful,
    professional energy with less tourist presence than nearby Broadway.
"""

let createZone (city: City) =
    let midtownWestZone = World.Zone.create Ids.Zone.midtownWest

    let broadway, broadwayMetro = broadway city midtownWestZone
    let seventhAvenue = seventhAvenue midtownWestZone
    let fiftySeventhStreet = fiftySeventhStreet midtownWestZone
    let sixthAvenue = sixthAvenue city midtownWestZone

    let broadwayMetroStation =
        { Lines = [ Blue ]
          LeavesToStreet = broadway.Id
          PlaceId = broadwayMetro.Id }

    midtownWestZone
    |> World.Zone.addStreet (World.Node.create broadway.Id broadway)
    |> World.Zone.addStreet (World.Node.create seventhAvenue.Id seventhAvenue)
    |> World.Zone.addStreet (
        World.Node.create fiftySeventhStreet.Id fiftySeventhStreet
    )
    |> World.Zone.addStreet (World.Node.create sixthAvenue.Id sixthAvenue)
    |> World.Zone.connectStreets broadway.Id seventhAvenue.Id West
    |> World.Zone.connectStreets broadway.Id fiftySeventhStreet.Id North
    |> World.Zone.connectStreets broadway.Id sixthAvenue.Id East
    |> World.Zone.addMetroStation broadwayMetroStation
