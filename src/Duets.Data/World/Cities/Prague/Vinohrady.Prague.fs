module rec Duets.Data.World.Cities.Prague.Vinohrady

open Duets.Data.World.Cities.Prague
open Duets.Data.World.Cities
open Duets.Entities

let francouzská city (zone: Zone) =
    let street =
        World.Street.create "Francouzská" (StreetType.Split(North, 2))
        |> World.Street.attachContext
            """
        Francouzská is a residential street in the heart of Vinohrady, lined with
        elegant late 19th-century apartment buildings featuring ornate neo-Renaissance
        and Art Nouveau facades. The street is characterized by tree-lined sidewalks,
        small boutiques, and local cafes that serve the neighborhood's affluent residents.
        Ground-floor spaces host fitness centers, wine bars, and quality restaurants.
        The atmosphere is upscale yet relaxed, representing Vinohrady's reputation as
        one of Prague's most desirable residential districts. Well-maintained buildings
        with decorative balconies and period details create an architecturally cohesive streetscape.
"""

    let home = PlaceCreators.createHome street.Id zone.Id

    let gyms =
        [ ("Fitness Korunní", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let bars =
        [ ("Vinohradský Pivovar", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("U Bulínů", 91<quality>, Czech, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let concertSpaces =
        [ ("Retro Music Hall",
           1000,
           80<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let radioStudios =
        [ ("Jazz Radio", 87<quality>, "Jazz", zone.Id) ]
        |> List.map (PlaceCreators.createRadioStudio city street.Id)

    let street =
        street
        |> World.Street.addPlace home
        |> World.Street.addPlaces gyms
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces radioStudios

    street

let náměstíMíru (zone: Zone) =
    let street =
        World.Street.create Ids.Street.náměstíMíru (StreetType.Split(East, 3))
        |> World.Street.attachContext
            """
        Náměstí Míru (Peace Square) is Vinohrady's grand central square, dominated
        by the imposing neo-Gothic Church of St. Ludmila with its twin spires. The
        square is laid out as a formal park with mature trees, benches, and a children's
        playground at its center. Elegant turn-of-the-century buildings surround the square,
        including the Art Nouveau Vinohrady Theatre. The metro station provides convenient
        transport connections. Cafes with outdoor seating overlook the park, creating a
        pleasant meeting point. The atmosphere is genteel and family-friendly, with
        locals walking dogs and meeting for coffee at the square's established institutions.
"""

    let concertSpaces =
        [ ("Vinohrady Theatre",
           700,
           89<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("The Down Under", 86<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("La Bohème Café", 93<quality>, French, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let metroStation =
        ("Náměstí Míru Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlace metroStation

    street, metroStation

let createZone city =
    let vinohradyZone = World.Zone.create Ids.Zone.vinohrady

    let francouzská = francouzská city vinohradyZone
    let náměstíMíru, náměstíMíruMetro = náměstíMíru vinohradyZone

    let náměstíMíruStation =
        { Lines = [ Red ]
          LeavesToStreet = náměstíMíru.Id
          PlaceId = náměstíMíruMetro.Id }

    vinohradyZone
    |> World.Zone.addStreet (World.Node.create francouzská.Id francouzská)
    |> World.Zone.addStreet (World.Node.create náměstíMíru.Id náměstíMíru)
    |> World.Zone.connectStreets francouzská.Id náměstíMíru.Id South
    |> World.Zone.addMetroStation náměstíMíruStation
