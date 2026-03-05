module rec Duets.Data.World.Cities.Toronto.DowntownCore

open Duets.Data.World.Cities.Toronto
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let private kingStreetWest (city: City) (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.kingStreetWest
            (StreetType.Split(East, 2))

    let concerts =
        [ ("Scotiabank Arena",
           19800,
           96<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id)
          ("Roy Thomson Hall",
           2630,
           94<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id)
          ("Four Seasons Centre",
           2000,
           93<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let cinemas =
        [ ("TIFF Bell Lightbox", 92<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCinema city.Id street.Id)

    let bars =
        [ ("The Fifth Social Club", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let metroStation =
        ("Union Station", zone.Id)
        |> PlaceCreators.createMetro street.Id

    let street =
        street
        |> World.Street.addPlaces concerts
        |> World.Street.addPlaces cinemas
        |> World.Street.addPlaces bars
        |> World.Street.addPlace metroStation
        |> World.Street.attachContext
            """
        King Street West in the Entertainment District is Toronto's cultural
        powerhouse. Scotiabank Arena anchors the south end, hosting arena-scale
        concerts and sports events, while Roy Thomson Hall's mirrored glass facade
        reflects the surrounding towers. The Four Seasons Centre for the Performing
        Arts adds opera and ballet to the mix. Streetcar tracks run down the center
        of the road as crowds flow between venues, restaurants, and the cluster of
        theatres that line the blocks.
"""

    street, metroStation

let private bayStreet (zone: Zone) =
    let street =
        World.Street.create Ids.Street.bayStreet StreetType.OneWay

    let casinos =
        [ ("Great Canadian Casino", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCasino street.Id)

    let carDealers =
        [ ("Bay Street Premium Motors",
           94<quality>,
           zone.Id,
           { Dealer =
               (Character.from
                   "James Thornton"
                   Male
                   (Shorthands.Autumn 15<days> 1976<years>))
             PriceRange = CarPriceRange.Premium }) ]
        |> List.map (PlaceCreators.createCarDealer street.Id)

    let hotels =
        [ ("Fairmont Royal York", 95<quality>, 450m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let restaurants =
        [ ("Canoe", 94<quality>, French, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Balzac's Coffee", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlaces casinos
    |> World.Street.addPlaces carDealers
    |> World.Street.addPlaces hotels
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces cafes
    |> World.Street.attachContext
        """
    Bay Street is Toronto's financial canyon, lined with glass and steel towers
    housing the country's major banks. The Fairmont Royal York, a grand chateau-style
    hotel, faces Union Station across Front Street. Suited professionals stride
    between PATH underground entrances while street-level cafes and restaurants
    serve the lunch rush. The atmosphere is corporate and purposeful, softened by
    occasional public art installations between the towers.
"""

let private frontStreet (zone: Zone) =
    let street =
        World.Street.create Ids.Street.frontStreet StreetType.OneWay

    let hotels =
        [ ("Delta Hotels Toronto", 80<quality>, 220m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let concerts =
        [ ("Meridian Hall",
           3000,
           90<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let restaurants =
        [ ("The Keg", 82<quality>, American, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    street
    |> World.Street.addPlaces hotels
    |> World.Street.addPlaces concerts
    |> World.Street.addPlaces restaurants
    |> World.Street.attachContext
        """
    Front Street runs along the southern edge of downtown Toronto, with views
    toward the waterfront and the CN Tower looming overhead. Meridian Hall's
    modernist facade hosts touring Broadway shows and large-scale concerts. The
    street mixes office towers with older brick buildings, and the St. Lawrence
    Market neighbourhood to the east draws weekend crowds for fresh produce and
    artisan food stalls.
"""

let createZone (city: City) =
    let downtownCoreZone = World.Zone.create Ids.Zone.downtownCore

    let kingStreetWest, unionMetro = kingStreetWest city downtownCoreZone
    let bayStreet = bayStreet downtownCoreZone
    let frontStreet = frontStreet downtownCoreZone

    let unionMetroStation =
        { Lines = [ Red; Blue ]
          LeavesToStreet = kingStreetWest.Id
          PlaceId = unionMetro.Id }

    downtownCoreZone
    |> World.Zone.addStreet (
        World.Node.create kingStreetWest.Id kingStreetWest
    )
    |> World.Zone.addStreet (World.Node.create bayStreet.Id bayStreet)
    |> World.Zone.addStreet (World.Node.create frontStreet.Id frontStreet)
    |> World.Zone.connectStreets kingStreetWest.Id bayStreet.Id North
    |> World.Zone.connectStreets kingStreetWest.Id frontStreet.Id South
    |> World.Zone.addMetroStation unionMetroStation
