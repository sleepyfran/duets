module rec Duets.Data.World.Cities.LosAngeles.StudioRow

open Duets.Data.World.Cities
open Duets.Entities
open Fugit.Months

let studioRow =
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
