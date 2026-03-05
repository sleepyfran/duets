module rec Duets.Data.World.Cities.Toronto.QueenWest

open Duets.Data.World.Cities.Toronto
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let private queenStreetWest (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.queenStreetWest
            (StreetType.Split(East, 2))

    let concerts =
        [ ("Horseshoe Tavern",
           400,
           87<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id)
          ("Drake Hotel",
           300,
           85<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id)
          ("The Rex",
           100,
           83<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("The Gladstone", 84<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let shops =
        [ ("Rotate This", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    let metroStation =
        ("Ossington Station", zone.Id)
        |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces concerts
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces shops
        |> World.Street.addPlace metroStation
        |> World.Street.attachContext
            """
        Queen Street West is Toronto's bohemian artery, a strip of independent
        galleries, vintage shops, and legendary music venues. The Horseshoe Tavern's
        neon sign has beckoned musicians since 1947, while the Drake Hotel anchors
        the west end with its rooftop bar and intimate performance space. Street art
        covers alley walls and construction hoardings, and the sidewalks fill with
        musicians, artists, and weekend shoppers browsing thrift stores.
"""

    street, metroStation

let private ossingtonAvenue (zone: Zone) =
    let street =
        World.Street.create Ids.Street.ossingtonAvenue StreetType.OneWay

    let bars =
        [ ("Bellwoods Brewery", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let cafes =
        [ ("Jimmy's Coffee", 84<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let bookstores =
        [ ("Type Books", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let restaurants =
        [ ("Pho Tien Thanh", 86<quality>, Vietnamese, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    street
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces restaurants
    |> World.Street.attachContext
        """
    Ossington Avenue is a narrow, tree-lined corridor of craft breweries,
    independent coffee shops, and small restaurants. Former garages and warehouses
    have been converted into taprooms and galleries. The pace is slower here than
    on Queen, with locals lingering on patios and browsing the handful of
    independent bookshops that anchor the strip.
"""

let private dundasStreetWest (zone: Zone) =
    let street =
        World.Street.create Ids.Street.dundasStreetWest StreetType.OneWay

    let studios =
        [ ("Taurus Recording",
           88<quality>,
           350m<dd>,
           (Character.from
               "Nyla Chen"
               Female
               (Shorthands.Spring 8<days> 1980<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let rehearsalSpaces =
        [ ("The Rehearsal Factory", 86<quality>, 150m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let concerts =
        [ ("The Baby G",
           200,
           82<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street
    |> World.Street.addPlaces studios
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces concerts
    |> World.Street.attachContext
        """
    Dundas Street West in this stretch is a working musicians' corridor, home to
    recording studios and rehearsal spaces tucked into converted industrial
    buildings. The Baby G hosts emerging acts in its intimate back room, while
    nearby studios hum with sessions day and night. The street has a creative,
    no-frills energy favored by working artists over tourists.
"""

let zone =
    let queenWestZone = World.Zone.create Ids.Zone.queenWest

    let queenStreetWest, ossingtonMetro = queenStreetWest queenWestZone
    let ossingtonAvenue = ossingtonAvenue queenWestZone
    let dundasStreetWest = dundasStreetWest queenWestZone

    let ossingtonMetroStation =
        { Lines = [ Blue ]
          LeavesToStreet = queenStreetWest.Id
          PlaceId = ossingtonMetro.Id }

    queenWestZone
    |> World.Zone.addStreet (
        World.Node.create queenStreetWest.Id queenStreetWest
    )
    |> World.Zone.addStreet (
        World.Node.create ossingtonAvenue.Id ossingtonAvenue
    )
    |> World.Zone.addStreet (
        World.Node.create dundasStreetWest.Id dundasStreetWest
    )
    |> World.Zone.connectStreets queenStreetWest.Id ossingtonAvenue.Id South
    |> World.Zone.connectStreets queenStreetWest.Id dundasStreetWest.Id North
    |> World.Zone.addMetroStation ossingtonMetroStation
