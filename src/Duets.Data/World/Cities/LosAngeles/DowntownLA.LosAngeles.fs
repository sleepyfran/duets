module Duets.Data.World.Cities.LosAngeles.DowntownLA

open Duets.Data.World.Cities
open Duets.Data.World.Cities.LosAngeles
open Duets.Entities

let figueroaStreet (zone: Zone) =
    let street =
        World.Street.create
            Ids.Street.figueroaStreet
            (StreetType.Split(North, 2))
        |> World.Street.attachContext
            """
        Figueroa Street in Downtown LA is a major north-south artery known as the
        "Arts, Sports, and Entertainment Corridor." This lively stretch is dominated
        by large-scale venues and modern developments. Key non-interactive landmarks
        include the Los Angeles Convention Center, and the bustling L.A. Live entertainment
        complex with its sleek neon signs and high-rise hotels. Farther south, the
        street connects to the historic Shrine Auditorium and the campus of the
        University of Southern California (USC). The street feels busy, commercial,
        and is characterized by a mix of tall office towers and modern entertainment
        architecture, defining the southwestern edge of the central business district.
"""

    let casinos =
        [ ("The Commerce Casino", 91<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCasino street.Id)

    let concertSpaces =
        [ ("Crypto.com Arena",
           17000,
           98<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id)
          ("Microsoft Theater",
           7100,
           94<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id)
          ("Orpheum Theatre",
           2000,
           91<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("W Hotel/L.A. Live Lounge", 93<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let rehearsalSpaces =
        [ ("Los Angeles Convention Center", 95<quality>, 500m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let hotels =
        [ ("The Grand Biltmore", 95<quality>, 500m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let metroStation =
        ("7th St/Metro Center", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces casinos
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces rehearsalSpaces
        |> World.Street.addPlaces hotels
        |> World.Street.addPlace metroStation

    street, metroStation

let grandAvenue (zone: Zone) =
    let street =
        World.Street.create Ids.Street.grandAvenue StreetType.OneWay
        |> World.Street.attachContext
            """
        Grand Avenue is DTLA's cultural and civic heart, contrasting with the commercial
        vibe of Figueroa. It runs along the top of Bunker Hill and is an architectural showcase.
        The street is home to world-class cultural institutions like the contemporary art
        museum The Broad, and the Museum of Contemporary Art (MOCA).
        The avenue also features the expansive green space of Grand Park, which stretches
        from the Music Center complex down to City Hall, providing an open, public
        contrast to the surrounding skyscrapers. The atmosphere is upscale, art-focused,
        and distinctly metropolitan, centered around performance and visual arts.
"""

    let concertSpaces =
        [ ("Walt Disney Concert Hall",
           2265,
           97<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id)
          ("Dorothy Chandler Pavilion",
           3197,
           95<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let restaurants =
        [ ("Patina Restaurant", 94<quality>, French, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Anya's Coffee", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces cafes

let zone =
    let downtownLAZone = World.Zone.create Ids.Zone.downtownLA

    let figueroaStreet, figueroaStreetStation = figueroaStreet downtownLAZone
    let grandAvenue = grandAvenue downtownLAZone

    let station =
        { Lines = [ Red; Blue ]
          LeavesToStreet = figueroaStreet.Id
          PlaceId = figueroaStreetStation.Id }

    downtownLAZone
    |> World.Zone.addStreet (World.Node.create figueroaStreet.Id figueroaStreet)
    |> World.Zone.addStreet (World.Node.create grandAvenue.Id grandAvenue)
    |> World.Zone.connectStreets figueroaStreet.Id grandAvenue.Id East
    |> World.Zone.addMetroStation station
