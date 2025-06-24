module rec Duets.Data.World.Cities.London.WestEnd

open Duets.Data.World.Cities
open Duets.Entities

let private oxfordStreet (zone: Zone) =
    let street =
        World.Street.create "Oxford Street" (StreetType.Split(North, 3))

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

    let metroStation =
        ("Oxford Circus Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    let street =
        street
        |> World.Street.addPlaces shops
        |> World.Street.addPlaces cafes
        |> World.Street.addPlaces concertSpaces
        |> World.Street.addPlace metroStation

    street, metroStation

let private soho (zone: Zone) =
    let street = World.Street.create "Soho" (StreetType.Split(West, 2))

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
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces restaurants
    |> World.Street.addPlace home

let private coventGarden (zone: Zone) =
    let street =
        World.Street.create "Covent Garden" (StreetType.Split(South, 2))

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

let zone =
    let zone = World.Zone.create "West End"
    let oxfordStreet, metroStation = oxfordStreet zone
    let soho = soho zone
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
    |> World.Zone.addDescriptor EntertainmentHeart
    |> World.Zone.addDescriptor Glitz
    |> World.Zone.addMetroStation metroStation
