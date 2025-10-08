module rec Duets.Data.World.Cities.London.WestEnd

open Duets.Data.World.Cities
open Duets.Entities
open Duets.Entities.Calendar

let private oxfordStreet (city: City) (zone: Zone) =
    let street =
        World.Street.create "Oxford Street" (StreetType.Split(North, 3))
        |> World.Street.attachContext
            """
        Oxford Street forms the backbone of London's retail heart, a 1.2-mile stretch of
        continuous shopfronts that attracts over 300,000 visitors daily. Grand department
        stores with ornate Victorian facades alternate with modern glass-fronted chains.
        Red double-decker buses queue in heavy traffic while crowds navigate wide pavements.
        The street's commercial energy peaks during sales periods when queues snake around
        corners. Side streets lead to quieter Georgian squares and mews houses. Despite
        modernization, architectural details from the street's 18th-century origins persist
        in upper-story windows and cornices above the contemporary shop displays.
"""

    let recordingStudios =
        [ ("AIR Studios",
           96<quality>,
           450m<dd>,
           (Character.from
               "George Martin"
               Male
               (Shorthands.Winter 3<days> 1926<years>)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let shops =
        [ ("Selfridges", zone.Id); ("John Lewis", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    let cafes =
        [ ("Pret a Manger", 75<quality>, zone.Id)
          ("Cafe Nero", 70<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let concertSpaces =
        [ ("London Palladium",
           2300,
           90<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id)
          ("100 Club", 350, 80<quality>, Layouts.concertSpaceLayout1, zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let radioStudios =
        [ ("BBC Radio 1", 94<quality>, "Pop", zone.Id) ]
        |> List.map (PlaceCreators.createRadioStudio city street.Id)

    let metroStation =
        ("Oxford Circus Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces recordingStudios
        |> World.Street.addPlaces shops
        |> World.Street.addPlaces cafes
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlaces radioStudios
        |> World.Street.addPlace metroStation

    street, metroStation

let private soho (city: City) (zone: Zone) =
    let street =
        World.Street.create "Soho" (StreetType.Split(West, 2))
        |> World.Street.attachContext
            """
        Soho's narrow streets and tight alleyways preserve the area's historic village-like
        layout, though the character has transformed from 17th-century fields to London's
        entertainment and media hub. Neon signs for jazz clubs, theaters, and restaurants
        illuminate Victorian and Georgian buildings. The area transitions throughout the day
        from media professionals in Wardour Street production houses to late-night revelers
        in Dean Street bars. Independent record shops, tailors, and family-run Italian delis
        occupy ground floors beneath post-production studios and advertising agencies. The
        atmosphere balances creative industry professionalism with bohemian nightlife.
"""

    let casinos =
        [ ("Hippodrome Casino", 90<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCasino street.Id)

    let gyms =
        [ ("Third Space Soho", 91<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createGym city street.Id)

    let bars =
        [ ("Ronnie Scott's", 85<quality>, zone.Id)
          ("The French House", 80<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let rehearsalSpaces =
        [ ("Soho Studios", 75<quality>, 200m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let restaurants =
        [ ("The Ivy", 88<quality>, French, zone.Id)
          ("Barrafina", 85<quality>, Spanish, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    let home = PlaceCreators.createHome street.Id zone.Id

    street
    |> World.Street.addPlaces casinos
    |> World.Street.addPlaces gyms
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlace home

let private coventGarden (zone: Zone) =
    let street =
        World.Street.create "Covent Garden" (StreetType.Split(South, 2))
        |> World.Street.attachContext
            """
        Covent Garden centers on its historic market building, a Victorian iron and glass
        structure now housing upscale shops and restaurants. The piazza around it provides
        open space where street performers entertain crowds against the backdrop of the
        Royal Opera House's neoclassical portico. Cobbled lanes radiating from the square
        contain specialist shops, antique markets, and theaters. The area retains its market
        origins in the layout and building stock, though the former fruit and vegetable
        trade has given way to tourism and culture. Theater-goers, tourists, and Londoners
        seeking boutique shopping create a lively, cosmopolitan atmosphere day and night.
"""

    let cafes =
        [ ("Monmouth Coffee", 80<quality>, zone.Id)
          ("Le Pain Quotidien", 75<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let bookstores =
        [ ("Foyles", 85<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let concertSpaces =
        [ ("Royal Opera House",
           2256,
           95<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    street
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces concertSpaces

let createZone (city: City) =
    let zone = World.Zone.create "West End"
    let oxfordStreet, metroStation = oxfordStreet city zone
    let soho = soho city zone
    let coventGarden = coventGarden zone

    let metroStation =
        { Lines = [ Blue; Red ]
          LeavesToStreet = oxfordStreet.Id
          PlaceId = metroStation.Id }

    zone
    |> World.Zone.addStreet (World.Node.create oxfordStreet.Id oxfordStreet)
    |> World.Zone.addStreet (World.Node.create soho.Id soho)
    |> World.Zone.addStreet (World.Node.create coventGarden.Id coventGarden)
    |> World.Zone.connectStreets oxfordStreet.Id soho.Id East
    |> World.Zone.connectStreets soho.Id coventGarden.Id South
    |> World.Zone.addMetroStation metroStation

let zone = createZone
