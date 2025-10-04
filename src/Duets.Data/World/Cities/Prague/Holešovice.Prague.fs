module rec Duets.Data.World.Cities.Prague.Holešovice

open Duets.Data.World.Cities
open Duets.Entities

let plynární (zone: Zone) =
    let street =
        World.Street.create "Plynární" (StreetType.Split(West, 2))
        |> World.Street.attachContext
            """
        Plynární runs through the heart of Holešovice's industrial-turned-cultural
        quarter, where former gasworks and factories have been transformed into
        vibrant cultural venues. The street features the iconic Cross Club,
        a steampunk-aesthetic venue housed in a converted industrial building,
        and the upscale SaSaZu restaurant complex. The area retains its industrial
        character with exposed brick facades and metal framework, while the nearby
        Vltava River embankment provides scenic views. The Nádraží Holešovice
        metro station serves as a major transport hub, connecting the district
        to the rest of Prague.
"""

    let concertSpaces =
        [ ("Cross Club", 400, 86<quality>, Layouts.concertSpaceLayout3, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Láhev Sud", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("SaSaZu", 90<quality>, Japanese, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let metroStation =
        ("Nádraží Holešovice Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces bars
        |> World.Street.addPlaces restaurants
        |> World.Street.addPlace metroStation

    street, metroStation

let komunardů (zone: Zone) =
    let street =
        World.Street.create "Komunardů" (StreetType.Split(North, 2))
        |> World.Street.attachContext
            """
        Komunardů street is the cultural artery of modern Holešovice, anchored
        by the striking white DOX Centre for Contemporary Art with its distinctive
        airship-like structure on its roof. The street showcases post-industrial
        architecture, with La Fabrika and other venues occupying converted warehouses
        and factory buildings. Tree-lined sections provide shade over wide sidewalks,
        and the area attracts a bohemian crowd with its galleries, contemporary art spaces,
        and eclectic cafes. The atmosphere is decidedly artistic and alternative,
        reflecting Holešovice's transformation into Prague's creative hub.
"""

    let concertSpaces =
        [ ("DOX Centre for Contemporary Art",
           600,
           84<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id)
          ("La Fabrika", 800, 85<quality>, Layouts.concertSpaceLayout2, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Bar Cobra", 87<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("Home Kitchen", 85<quality>, French, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let cafes =
        [ ("Vnitroblock Café", 88<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlaces cafes

let zaElektrarnou (zone: Zone) =
    let street =
        World.Street.create "Za Elektrárnou" (StreetType.Split(East, 2))
        |> World.Street.attachContext
            """
        Za Elektrárnou borders the expansive Stromovka Park and the Exhibition Grounds
        (Výstaviště), dominated by the massive Tipsport Arena sports and concert complex.
        The street features a mix of early 20th-century exhibition pavilions and modern sports
        infrastructure. During events, the area bustles with crowds heading to concerts
        or hockey matches. The proximity to Stromovka, Prague's largest park, brings
        a greener character to this section, with joggers and cyclists frequently passing through.
        Historic exhibition buildings from the 1891 Jubilee Exhibition still stand nearby,
        creating an interesting architectural contrast.
"""

    let concertSpaces =
        [ ("Tipsport Arena",
           13000,
           88<quality>,
           Layouts.concertSpaceLayout4,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let bars =
        [ ("Restaurace U Výstaviště", 82<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let restaurants =
        [ ("Bistro Stromovka", 85<quality>, Czech, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    street
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces restaurants

let zone =
    let holešoviceZone = World.Zone.create Ids.Zone.holešovice

    let plynární, metroStation = plynární holešoviceZone
    let komunardů = komunardů holešoviceZone
    let zaElektrarnou = zaElektrarnou holešoviceZone

    let station =
        { Lines = [ Red ]
          LeavesToStreet = plynární.Id
          PlaceId = metroStation.Id }

    holešoviceZone
    |> World.Zone.addStreet (World.Node.create plynární.Id plynární)
    |> World.Zone.addStreet (World.Node.create komunardů.Id komunardů)
    |> World.Zone.addStreet (World.Node.create zaElektrarnou.Id zaElektrarnou)
    |> World.Zone.connectStreets plynární.Id komunardů.Id South
    |> World.Zone.connectStreets komunardů.Id zaElektrarnou.Id East
    |> World.Zone.addMetroStation station
