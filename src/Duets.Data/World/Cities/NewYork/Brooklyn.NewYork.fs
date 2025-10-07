module rec Duets.Data.World.Cities.NewYork.Brooklyn

open Duets.Data.World.Cities
open Duets.Entities

let private lafayetteAtlantic (city: City) (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.lafayetteAtlantic
            (StreetType.Split(East, 3))

    let gyms =
        [ ("Blink Fitness Brooklyn", 84<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let concerts =
        [ ("Barclays Center",
           19000,
           96<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id)
          ("BAM (Brooklyn Academy of Music)",
           2100,
           92<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("baba cool", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("Bacchus", 88<quality>, French, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let metroStation =
        ("Atlantic Avenue-Barclays Center", zone.Id)
        |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces gyms
        |> World.Street.addPlaces concerts
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlace metroStation
        |> World.Street.attachContext
            """
        Lafayette Avenue and Atlantic Avenue converge at a major Brooklyn intersection
        dominated by Barclays Center's weathered steel facade and distinctive oculus.
        The area is a transit nexus with multiple subway lines meeting beneath the streets,
        creating constant foot traffic day and night. The Brooklyn Academy of
        Music's cultural district brings theater patrons and art enthusiasts to
        the neighborhood, mixing with sports fans and daily commuters. Wide sidewalks
        accommodate the crowds, while newer residential towers and commercial developments
        have transformed the once-industrial area. Street vendors cluster near the
        subway entrance selling everything from bootleg merchandise to grilled meat,
        adding an entrepreneurial energy to the corporate development.
"""

    street, metroStation

let private frostStreet (zone: Zone) =
    let street = World.Street.create Ids.Street.frostStreet StreetType.OneWay

    let home = PlaceCreators.createHome street.Id zone.Id

    // TODO: Add Frost Playground once we support parks
    // TODO: Add Cooper Park once we support parks

    street
    |> World.Street.attachContext
        """
    Frost Street is a residential Brooklyn street in East Williamsburg characterized
    by low-rise apartment buildings, community gardens, and small parks. The Cooper
    Park green space provides a neighborhood gathering point with basketball courts,
    playgrounds, and open grass areas where locals exercise dogs and children play.
    Old industrial warehouses converted to residential lofts mix with public housing complexes,
    creating economic diversity. Street trees provide shade in summer, and small
    bodegas anchor the corners. The area has a working-class, family-oriented atmosphere
    with strollers on sidewalks and older residents sitting outside in good weather.
    Murals painted on building sides add color to the brick and concrete streetscape.
"""

let private bedfordAvenue (zone: Zone) =
    let street = World.Street.create Ids.Street.bedfordAvenue StreetType.OneWay

    let concerts =
        [ ("The Knitting Factory",
           150,
           87<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let rehearsalSpaces =
        [ ("Music Building", 88<quality>, 175m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    street
    |> World.Street.addPlaces concerts
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.attachContext
        """
    Bedford Avenue in Williamsburg is Brooklyn's hipster main street, lined
    with vintage boutiques, independent coffee shops, and music venues occupying
    converted industrial spaces. The L train subway entrance creates a constant flow
    of young creatives, musicians, and artists. Street art and wheatpaste posters
    cover walls and construction barriers, advertising underground shows and
    political causes. The sidewalks are crowded on weekends with shoppers browsing
    thrift stores and record shops. Former factories with exposed brick have been
    repurposed as galleries, rehearsal spaces, and small concert halls. The neighborhood
    maintains an alternative, artistic identity despite rising rents, with dive bars
    and experimental performance spaces still thriving alongside newer, upscale establishments.
"""

let zone city =
    let brooklynZone = World.Zone.create Ids.Zone.brooklyn

    let lafayetteAtlantic, atlanticMetro = lafayetteAtlantic city brooklynZone
    let frostStreet = frostStreet brooklynZone
    let bedfordAvenue = bedfordAvenue brooklynZone

    let atlanticMetroStation =
        { Lines = [ Blue ]
          LeavesToStreet = lafayetteAtlantic.Id
          PlaceId = atlanticMetro.Id }

    brooklynZone
    |> World.Zone.addStreet (
        World.Node.create lafayetteAtlantic.Id lafayetteAtlantic
    )
    |> World.Zone.addStreet (World.Node.create frostStreet.Id frostStreet)
    |> World.Zone.addStreet (World.Node.create bedfordAvenue.Id bedfordAvenue)
    |> World.Zone.connectStreets lafayetteAtlantic.Id frostStreet.Id North
    |> World.Zone.connectStreets lafayetteAtlantic.Id bedfordAvenue.Id East
    |> World.Zone.addMetroStation atlanticMetroStation
