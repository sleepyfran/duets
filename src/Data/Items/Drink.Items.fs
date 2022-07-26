module Data.Items.Drink

open Entities

/// Defines all the beers available in the game.
module Beer =
    let pilsnerUrquellPint =
        { Price = 45<dd>
          Brand = "Pilsner Urquell"
          Type = Beer(500<milliliter>, 4.4) |> Drink }

    let kozelPint =
        { Price = 45<dd>
          Brand = "Kozel"
          Type = Beer(500<milliliter>, 4.6) |> Drink }

    let staropramenPint =
        { Price = 45<dd>
          Brand = "Staropramen"
          Type = Beer(500<milliliter>, 4.7) |> Drink }

    let gambrinusPint =
        { Price = 45<dd>
          Brand = "Gambrinus"
          Type = Beer(500<milliliter>, 4.3) |> Drink }

    let cernaHoraPint =
        { Price = 50<dd>
          Brand = "Černa Horá"
          Type = Beer(500<milliliter>, 4.8) |> Drink }

    let matushkaPint =
        { Price = 55<dd>
          Brand = "Matuška California"
          Type = Beer(500<milliliter>, 5.4) |> Drink }
