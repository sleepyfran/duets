module rec Duets.Data.World.Cities.LosAngeles.BoulevardOfStars

open Duets.Data.World.Cities
open Duets.Entities

let boulevardOfStars =
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

    World.Street.create "Boulevard of Stars" (StreetType.Split(North, 3))
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces bookstores
    |> World.Street.addPlaces hotels
    |> World.Street.addPlaces rehearsalSpaces
    |> World.Street.addPlaces concertSpaces
    |> World.Street.addPlaces restaurants
