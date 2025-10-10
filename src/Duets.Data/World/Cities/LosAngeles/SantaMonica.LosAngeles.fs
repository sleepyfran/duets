module Duets.Data.World.Cities.LosAngeles.SantaMonica

open Duets.Data.World.Cities.LosAngeles
open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let oceanAvenue (zone: Zone) =
    let street =
        World.Street.create Ids.Street.oceanAvenue StreetType.OneWay
        |> World.Street.attachContext
            """
        This is the iconic, palm-tree-lined westernmost street, running along a
        bluff overlooking the Pacific Ocean and Santa Monica State Beach. It is a
        vital link to the Santa Monica Pier (at the intersection with Colorado Ave),
        a massive landmark with its amusement park, Ferris Wheel, and the
        "End of the Trail" Route 66 sign. The street is bordered by Palisades Park
        and Tongva Park, offering stunning ocean views and a mix of high-end hotels and residences.
"""

    let concertSpaces =
        [ ("Santa Monica Pier",
           5000,
           89<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id)
          ("The Ocean Room",
           150,
           91<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let restaurants =
        [ ("The Pacific Hideaway", 93<quality>, French, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    // TODO: Add wellness center/spa support - using hotel as placeholder for wellness center
    let hotels =
        [ ("The Coastal Spa", 94<quality>, 450m<dd>, zone.Id)
          ("The Ocean House", 95<quality>, 500m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let metroStation =
        ("Downtown Santa Monica", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces hotels
        |> World.Street.addPlace metroStation

    street, metroStation

let mainStreet (zone: Zone) =
    let street =
        World.Street.create Ids.Street.mainStreet StreetType.OneWay
        |> World.Street.attachContext
            """
        Running parallel to the beach, a couple of blocks inland, Main Street is
        known for its "local, surfer vibe"—a sophisticated yet laid-back shopping
        and dining district in the Ocean Park neighborhood. It features eclectic
        and upscale shops, cafes, and restaurants, including the famous Chinois
        on Main restaurant. The area also showcases modern architecture, including
        two buildings designed by Frank Gehry and the striking Ballerina Clown
        sculpture by Jonathan Borofsky.
"""

    let concertSpaces =
        [ ("The Central Club",
           250,
           83<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bookstores =
        [ ("Ocean View Books", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let cafes =
        [ ("The Beach Bean", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let merchandiseWorkshops =
        [ ("Vintage Guitar Shop", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces merchandiseWorkshops

let picoBoulevard (zone: Zone) (city: City) =
    let street =
        World.Street.create Ids.Street.picoBoulevard (StreetType.Split(East, 2))
        |> World.Street.attachContext
            """
        Pico Boulevard is a major, diverse artery connecting Santa Monica to
        Downtown Los Angeles. It is a cultural hub distinguished by numerous striking
        street art murals that have turned stretches of the street into an open-air
        art exhibition. Notable landmarks include the Santa Monica Civic Auditorium,
        Santa Monica High School, and Santa Monica College. The boulevard is also
        home to a mix of small galleries, live music venues like
        McCabe's Guitar Shop (a noted folk concert forum), and a wide variety of culturally diverse restaurants.
"""

    let home = PlaceCreators.createHome street.Id zone.Id

    let carDealers =
        [ ("Pacific Coast Motors",
           82<quality>,
           zone.Id,
           { Dealer =
               (Character.from
                   "Jennifer Martinez"
                   Female
                   (Shorthands.Spring 15<days> 1980<years>))
             PriceRange = CarPriceRange.MidRange }) ]
        |> List.map (PlaceCreators.createCarDealer street.Id)

    let rehearsalSpaces =
        [ ("The Lockout Rooms", 84<quality>, 350m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let studios =
        [ ("The Garage Studio",
           82<quality>,
           400m<dd>,
           (Character.from
               "Sarah Mitchell"
               Female
               (Shorthands.Autumn 12<days> 1988<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let hospitals =
        [ ("Santa Monica Urgent Care", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createHospital street.Id)

    let gyms =
        [ ("Local Iron Gym", 83<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    street
    |> World.Street.addPlace home
    |> World.Street.addPlaces carDealers
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces studios
    |> World.Street.addPlaces hospitals
    |> World.Street.addPlaces gyms

let createZone (city: City) =
    let santaMonicaZone = World.Zone.create Ids.Zone.santaMonica

    let oceanAvenue, oceanAvenueStation = oceanAvenue santaMonicaZone
    let mainStreet = mainStreet santaMonicaZone
    let picoBoulevard = picoBoulevard santaMonicaZone city

    let station =
        { Lines = [ Blue ]
          LeavesToStreet = oceanAvenue.Id
          PlaceId = oceanAvenueStation.Id }

    santaMonicaZone
    |> World.Zone.addStreet (World.Node.create oceanAvenue.Id oceanAvenue)
    |> World.Zone.addStreet (World.Node.create mainStreet.Id mainStreet)
    |> World.Zone.addStreet (World.Node.create picoBoulevard.Id picoBoulevard)
    |> World.Zone.connectStreets oceanAvenue.Id mainStreet.Id East
    |> World.Zone.connectStreets mainStreet.Id picoBoulevard.Id East
    |> World.Zone.addMetroStation station
