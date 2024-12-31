module rec Duets.Data.World.Cities.LosAngeles.Hollywood

open Duets.Data.World.Cities
open Duets.Entities
open Fugit.Months

let private studioRow =
    let studios =
        [ ("Starlight Studios",
           88<quality>,
           260m<dd>,
           (Character.from "Max Sterling" Male (June 20 1981)))
          ("Soundwave Labs",
           75<quality>,
           220m<dd>,
           (Character.from "Eva Ríos" Female (March 15 1985)))
          ("Echo Chamber",
           68<quality>,
           180m<dd>,
           (Character.from "Leo Vance" Male (July 10 1978)))
          ("Retro Tracks",
           82<quality>,
           240m<dd>,
           (Character.from "Mia Chen" Female (September 5 1990))) ]
        |> List.map PlaceCreators.createStudio

    let rehearsalSpaces =
        [ ("The Rehearsal Room", 70<quality>, 160m<dd>)
          ("The Sound Box", 65<quality>, 140m<dd>) ]
        |> List.map PlaceCreators.createRehearsalSpace

    let concertSpaces =
        [ ("The Roxy", 500, 85<quality>, Layouts.concertSpaceLayout1)
          ("Whiskey a Go Go", 500, 82<quality>, Layouts.concertSpaceLayout2)
          ("The Troubadour", 400, 78<quality>, Layouts.concertSpaceLayout1) ]
        |> List.map PlaceCreators.createConcertSpace

    let merchStores =
        [ "Hollywood Memorabilia" ]
        |> List.map PlaceCreators.createMerchandiseWorkshop

    let cafes =
        [ ("Daily Grind", 72<quality>) ] |> List.map PlaceCreators.createCafe

    let metroStation = "Hollywood/Vine Station" |> PlaceCreators.createMetro

    World.Street.create "Studio Row" (StreetType.Split(North, 3))
    |> World.Street.addPlaces studios
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces merchStores
    |> World.Street.addPlaces cafes
    |> World.Street.addPlace metroStation

let private boulevardOfStars =
    let bars =
        [ ("The Froolic Room", 75<quality>); ("Boardner's", 73<quality>) ]
        |> List.map PlaceCreators.createBar

    let bookstores =
        [ ("Larry Edmund's Bookshop", 80<quality>); ("Booksoup", 78<quality>) ]
        |> List.map PlaceCreators.createBookstore

    let hotels =
        [ ("The Hollywood Roosevelt", 90<quality>, 550m<dd>)
          ("The Dream Hollywood", 85<quality>, 500m<dd>) ]
        |> List.map PlaceCreators.createHotel

    let rehearsalSpaces =
        [ ("The Rehearsal Room", 70<quality>, 160m<dd>)
          ("The Sound Box", 65<quality>, 140m<dd>) ]
        |> List.map PlaceCreators.createRehearsalSpace

    let concertSpaces =
        [ ("The Hollywood Bowl", 17500, 95<quality>, Layouts.concertSpaceLayout3)
          ("The Dolby Theater", 3400, 92<quality>, Layouts.concertSpaceLayout2) ]
        |> List.map PlaceCreators.createConcertSpace

    let restaurants =
        [ ("Spago", 92<quality>, Italian)
          ("Musso & Frank Grill", 88<quality>, American)
          ("Mel's Drive-In", 72<quality>, American) ]
        |> List.map PlaceCreators.createRestaurant

    World.Street.create "Boulevard of Stars" (StreetType.Split(North, 2))
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces hotels
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces restaurants

let private alleyway =
    let bars =
        [ ("The Viper Room", 80<quality>) ] |> List.map PlaceCreators.createBar

    let cafes =
        [ ("The Coffee Bean", 68<quality>); ("Cafe Tropical", 70<quality>) ]
        |> List.map PlaceCreators.createCafe

    let studios =
        [ ("Backstage Recording",
           65<quality>,
           190m<dd>,
           (Character.from "Sarah Jones" Male (January 10 1988)))
          ("Urban Beats Studio",
           58<quality>,
           160m<dd>,
           (Character.from "Carlos Ramirez" Female (February 12 1980))) ]
        |> List.map PlaceCreators.createStudio

    let rehearsalSpaces =
        [ ("The Jam Space", 60<quality>, 120m<dd>) ]
        |> List.map PlaceCreators.createRehearsalSpace

    World.Street.create "Alleyway" StreetType.OneWay
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces studios
    |> World.Street.addPlaces rehearsalSpaces

let zone =
    let studioRow = studioRow
    let boulevardOfStars = boulevardOfStars
    let alleyway = alleyway

    let metroStation =
        { Line = Blue
          LeavesToStreet = studioRow.Id }

    World.Zone.create "Hollywood" (World.Node.create studioRow.Id studioRow)
    |> World.Zone.addStreet (
        World.Node.create boulevardOfStars.Id boulevardOfStars
    )
    |> World.Zone.addStreet (World.Node.create alleyway.Id alleyway)
    |> World.Zone.connectStreets studioRow.Id boulevardOfStars.Id East
    |> World.Zone.connectStreets boulevardOfStars.Id alleyway.Id North
    |> World.Zone.addDescriptor Glitz
    |> World.Zone.addDescriptor EntertainmentHeart
    |> World.Zone.addMetroStation metroStation
