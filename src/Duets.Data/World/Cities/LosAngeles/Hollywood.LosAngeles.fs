module rec Duets.Data.World.Cities.LosAngeles.Hollywood

open Duets.Data.World.Cities
open Duets.Entities
open Fugit.Months

let private studioRow (zone: Zone) =
    let street = World.Street.create "Studio Row" (StreetType.Split(North, 3))

    let studios =
        [ ("Starlight Studios",
           88<quality>,
           260m<dd>,
           (Character.from "Max Sterling" Male (June 20 1981)),
           zone.Id)
          ("Soundwave Labs",
           75<quality>,
           220m<dd>,
           (Character.from "Eva Ríos" Female (March 15 1985)),
           zone.Id)
          ("Echo Chamber",
           68<quality>,
           180m<dd>,
           (Character.from "Leo Vance" Male (July 10 1978)),
           zone.Id)
          ("Retro Tracks",
           82<quality>,
           240m<dd>,
           (Character.from "Mia Chen" Female (September 5 1990)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let rehearsalSpaces =
        [ ("The Rehearsal Room", 70<quality>, 160m<dd>, zone.Id)
          ("The Sound Box", 65<quality>, 140m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let concertSpaces =
        [ ("The Roxy", 500, 85<quality>, Layouts.concertSpaceLayout1, zone.Id)
          ("Whiskey a Go Go",
           500,
           82<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id)
          ("The Troubadour",
           400,
           78<quality>,
           Layouts.concertSpaceLayout1,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let merchStores =
        [ ("Hollywood Memorabilia", zone.Id) ]
        |> List.map (PlaceCreators.createMerchandiseWorkshop street.Id)

    let cafes =
        [ ("Daily Grind", 72<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let metroStation =
        ("Hollywood/Vine Station", zone.Id)
        |> (PlaceCreators.createMetro street.Id)

    street
    |> World.Street.addPlaces studios
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces merchStores
    |> World.Street.addPlaces cafes
    |> World.Street.addPlace metroStation

let private boulevardOfStars (zone: Zone) =
    let street =
        World.Street.create "Boulevard of Stars" (StreetType.Split(North, 2))

    let bars =
        [ ("The Froolic Room", 75<quality>, zone.Id)
          ("Boardner's", 73<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let bookstores =
        [ ("Larry Edmund's Bookshop", 80<quality>, zone.Id)
          ("Booksoup", 78<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBookstore street.Id)

    let hotels =
        [ ("The Hollywood Roosevelt", 90<quality>, 550m<dd>, zone.Id)
          ("The Dream Hollywood", 85<quality>, 500m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createHotel street.Id)

    let rehearsalSpaces =
        [ ("The Rehearsal Room", 70<quality>, 160m<dd>, zone.Id)
          ("The Sound Box", 65<quality>, 140m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    let concertSpaces =
        [ ("The Hollywood Bowl",
           17500,
           95<quality>,
           Layouts.concertSpaceLayout3,
           zone.Id)
          ("The Dolby Theater",
           3400,
           92<quality>,
           Layouts.concertSpaceLayout2,
           zone.Id) ]
        |> List.map (PlaceCreators.createConcertSpace street.Id)

    let restaurants =
        [ ("Spago", 92<quality>, Italian, zone.Id)
          ("Musso & Frank Grill", 88<quality>, American, zone.Id)
          ("Mel's Drive-In", 72<quality>, American, zone.Id) ]
        |> List.map (PlaceCreators.createRestaurant street.Id)

    street
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces hotels
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces restaurants

let private alleyway (zone: Zone) =
    let street = World.Street.create "Alleyway" StreetType.OneWay

    let bars =
        [ ("The Viper Room", 80<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createBar street.Id)

    let cafes =
        [ ("The Coffee Bean", 68<quality>, zone.Id)
          ("Cafe Tropical", 70<quality>, zone.Id) ]
        |> List.map (PlaceCreators.createCafe street.Id)

    let studios =
        [ ("Backstage Recording",
           65<quality>,
           190m<dd>,
           (Character.from "Sarah Jones" Male (January 10 1988)),
           zone.Id)
          ("Urban Beats Studio",
           58<quality>,
           160m<dd>,
           (Character.from "Carlos Ramirez" Female (February 12 1980)),
           zone.Id) ]
        |> List.map (PlaceCreators.createStudio street.Id)

    let rehearsalSpaces =
        [ ("The Jam Space", 60<quality>, 120m<dd>, zone.Id) ]
        |> List.map (PlaceCreators.createRehearsalSpace street.Id)

    street
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces studios
    |> World.Street.addPlaces rehearsalSpaces

let zone =
    let hollywoodZone = World.Zone.create "Hollywood"

    let studioRow = studioRow hollywoodZone
    let boulevardOfStars = boulevardOfStars hollywoodZone
    let alleyway = alleyway hollywoodZone

    let metroStation =
        { Line = Blue
          LeavesToStreet = studioRow.Id }

    hollywoodZone
    |> World.Zone.addStreet (World.Node.create studioRow.Id studioRow)
    |> World.Zone.addStreet (
        World.Node.create boulevardOfStars.Id boulevardOfStars
    )
    |> World.Zone.addStreet (World.Node.create alleyway.Id alleyway)
    |> World.Zone.connectStreets studioRow.Id boulevardOfStars.Id East
    |> World.Zone.connectStreets boulevardOfStars.Id alleyway.Id North
    |> World.Zone.addDescriptor Glitz
    |> World.Zone.addDescriptor EntertainmentHeart
    |> World.Zone.addMetroStation metroStation
