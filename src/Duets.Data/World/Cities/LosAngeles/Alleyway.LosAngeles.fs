module rec Duets.Data.World.Cities.LosAngeles.Alleyway

open Duets.Data.World.Cities
open Duets.Entities
open Fugit.Months

let alleyway =
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

    World.Street.create "Studio Row" (StreetType.Split(North, 3))
    |> World.Street.addPlaces bars
    |> World.Street.addPlaces cafes
    |> World.Street.addPlaces studios
    |> World.Street.addPlaces rehearsalSpaces
