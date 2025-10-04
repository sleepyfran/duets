module rec Duets.Data.World.Cities.London.Camden

open Duets.Data.World.Cities
open Duets.Entities

let private camdenHighStreet (city: City) (zone: Zone) =
    let street =
        World.Street.create "Camden High Street" (StreetType.Split(North, 2))
        |> World.Street.attachContext
            """
        Camden High Street pulses with alternative culture and creative energy. The street
        is lined with Victorian buildings adorned with striking murals and unconventional
        storefronts shaped like giant boots and aircraft fuselages. Street vendors sell
        vintage clothing, handmade jewelry, and eclectic artwork. The famous Camden Market
        sprawls through covered arcades and canal-side stalls, offering everything from
        international street food to punk memorabilia. The area attracts a diverse mix of
        tourists, musicians, artists, and counterculture enthusiasts. Music venues tucked
        into side streets have launched countless careers since the punk era.
"""

    let bars =
        [ ("The World's End", 80<quality>, zone.Id)
          ("Dublin Castle", 78<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let concertSpaces =
        [ ("Roundhouse", 1700, 88<quality>, Layouts.concertSpaceLayout2, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let rehearsalSpaces =
        [ ("Camden Practice Rooms", 75<quality>, 160m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let radioStudios =
        [ ("Absolute Radio", 90<quality>, "Rock", zone.Id) ]
        |> List.map (PlaceCreators.createRadioStudio city)

    let metroStation =
        ("Camden Town Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces rehearsalSpaces
        |> World.Street.addPlaces radioStudios
        |> World.Street.addPlace metroStation

    street, metroStation

let private chalkFarm (zone: Zone) =
    let street =
        World.Street.create "Chalk Farm" (StreetType.Split(West, 2))
        |> World.Street.attachContext
            """
        Chalk Farm sits at the northern edge of Camden, where the area's bohemian character
        meets a more residential atmosphere. The street runs alongside the Regent's Canal,
        with converted warehouses and industrial buildings now housing independent shops
        and cafes. Historic pubs with Victorian tilework stand alongside modern gastropubs.
        The Roundhouse, a circular former railway engine shed, dominates the area as a major
        performance venue. The street retains a village-like feel despite its proximity to
        Camden's bustle, with local residents mixing with venue-goers.
"""

    let cafes =
        [ ("The Coffee Jar", 75<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let bookstores =
        [ ("Owl Bookshop", 80<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    street |> World.Street.addPlaces cafes |> World.Street.addPlaces bookstores

let private regentsPark (zone: Zone) =
    let street =
        World.Street.create "Regent's Park" StreetType.OneWay
        |> World.Street.attachContext
            """
        Regent's Park stretches across 410 acres of manicured gardens, sports facilities,
        and open meadows. John Nash designed the park in the early 19th century as part of
        a grand route from Regent Street to Primrose Hill. Tree-lined avenues encircle the
        park, while the inner circle contains Queen Mary's Gardens with over 12,000 roses.
        The Broad Walk provides grand north-south vistas. Boating lake, outdoor theatre, and
        London Zoo attract visitors year-round. The park maintains a serene, classical
        atmosphere despite heavy use, with formal planting schemes and carefully maintained
        lawns creating an idealized vision of English landscape design.
"""

    let concertSpaces =
        [ ("Open Air Theatre",
           1250,
           85<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let cafes =
        [ ("Regent's Park Cafe", 70<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces cafes

let createZone (city: City) =
    let zone = World.Zone.create "Camden"
    let camdenHighStreet, metroStation = camdenHighStreet city zone
    let chalkFarm = chalkFarm zone
    let regentsPark = regentsPark zone

    let metroStation =
        { Lines = [ Red; Blue ]
          LeavesToStreet = camdenHighStreet.Id
          PlaceId = metroStation.Id }

    zone
    |> World.Zone.addStreet (
        World.Node.create camdenHighStreet.Id camdenHighStreet
    )
    |> World.Zone.addStreet (World.Node.create chalkFarm.Id chalkFarm)
    |> World.Zone.addStreet (World.Node.create regentsPark.Id regentsPark)
    |> World.Zone.connectStreets camdenHighStreet.Id chalkFarm.Id West
    |> World.Zone.connectStreets camdenHighStreet.Id regentsPark.Id North
    |> World.Zone.addMetroStation metroStation

let zone = createZone
