module rec Duets.Data.World.Cities.Toronto.Midtown

open Duets.Data.World.Cities.Toronto
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let private yongeStreet (city: City) (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.yongeStreet
            (StreetType.Split(North, 2))

    let concerts =
        [ ("Massey Hall",
           2750,
           95<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id)
          ("Elgin Theatre",
           1500,
           91<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let cinemas =
        [ ("Scotiabank Theatre", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCinema city.Id street.Id)

    let hotels =
        [ ("Park Hyatt", 93<quality>, 400m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let metroStation =
        ("Bloor-Yonge Station", zone.Id)
        |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces concerts
        |> World.Street.addPlaces cinemas
        |> World.Street.addPlaces hotels
        |> World.Street.addPlace metroStation
        |> World.Street.attachContext
            """
        Yonge Street, once billed as the longest street in the world, is Toronto's
        central spine. The stretch through Midtown passes the newly restored Massey
        Hall, the city's most storied concert venue, alongside the Elgin Theatre's
        ornate Edwardian facade. Neon signs compete with digital billboards as the
        street transitions from the commercial bustle of Dundas Square northward
        into the upscale Yorkville neighbourhood. The Bloor-Yonge subway interchange
        below is the system's busiest station.
"""

    street, metroStation

let private bloorStreetWest (city: City) (zone: Zone) =
    let street =
        World.Street.create Ids.Street.bloorStreetWest StreetType.OneWay

    let home = PlaceCreators.createHome street.Id zone.Id

    let carDealers =
        [ ("Yorkville Auto Gallery",
           88<quality>,
           zone.Id,
           { Dealer =
               (Character.from
                   "Sophie Laurent"
                   Female
                   (Shorthands.Summer 20<days> 1982<years>))
             PriceRange = CarPriceRange.MidRange }) ]
        |> List.map (PlaceCreators.createCarDealer street.Id)

    let gyms =
        [ ("Equinox Yorkville", 92<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let hospitals =
        [ ("Mount Sinai Hospital", 94<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createHospital street.Id)

    street
    |> World.Street.addPlace home
    |> World.Street.addPlaces carDealers
    |> World.Street.addPlaces gyms
    |> World.Street.addPlaces hospitals
    |> World.Street.attachContext
        """
    Bloor Street West through Yorkville is Toronto's luxury corridor, lined with
    designer boutiques, art galleries, and the Victorian houses-turned-shops of
    the old village. The Royal Ontario Museum's crystal extension juts dramatically
    over the sidewalk. Mount Sinai Hospital's research tower rises nearby,
    anchoring the Discovery District where medicine meets upscale retail.
"""

let private avenueRoad (zone: Zone) =
    let street =
        World.Street.create Ids.Street.avenueRoad StreetType.OneWay

    let studios =
        [ ("Noble Street Studios",
           90<quality>,
           380m<dd>,
           (Character.from
               "Marcus Webb"
               Male
               (Shorthands.Autumn 3<days> 1975<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let rehearsalSpaces =
        [ ("Rehearsal Room Yorkville", 88<quality>, 170m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let bookstores =
        [ ("Indigo Yorkville", 87<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let cafes =
        [ ("Pilot Coffee", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlaces studios
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces cafes
    |> World.Street.attachContext
        """
    Avenue Road climbs from Bloor Street into the leafy residential streets of
    upper Yorkville and Forest Hill. Recording studios and rehearsal rooms occupy
    converted mansions alongside independent bookshops and specialty coffee roasters.
    The tree-canopied street has a quieter, more residential character than the
    commercial strips to the south, favored by musicians seeking focused creative space.
"""

let createZone (city: City) =
    let midtownZone = World.Zone.create Ids.Zone.midtown

    let yongeStreet, bloorYongeMetro = yongeStreet city midtownZone
    let bloorStreetWest = bloorStreetWest city midtownZone
    let avenueRoad = avenueRoad midtownZone

    let bloorYongeMetroStation =
        { Lines = [ Red ]
          LeavesToStreet = yongeStreet.Id
          PlaceId = bloorYongeMetro.Id }

    midtownZone
    |> World.Zone.addStreet (
        World.Node.create yongeStreet.Id yongeStreet
    )
    |> World.Zone.addStreet (
        World.Node.create bloorStreetWest.Id bloorStreetWest
    )
    |> World.Zone.addStreet (World.Node.create avenueRoad.Id avenueRoad)
    |> World.Zone.connectStreets yongeStreet.Id bloorStreetWest.Id West
    |> World.Zone.connectStreets yongeStreet.Id avenueRoad.Id East
    |> World.Zone.addMetroStation bloorYongeMetroStation
