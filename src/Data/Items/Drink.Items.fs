module Data.Items.Drink

open Entities

/// Defines all the beers available in the game.
module Beer =
    let pilsnerUrquellPint =
        { BasePrice = 10<dd>
          Brand = "Pilsner Urquell"
          Type = Beer(500<milliliter>, 4.4) |> Drink }

    let cernaHoraPint =
        { BasePrice = 11<dd>
          Brand = "Černa Horá"
          Type = Beer(500<milliliter>, 4.8) |> Drink }

    let kozelPint =
        { BasePrice = 10<dd>
          Brand = "Kozel"
          Type = Beer(500<milliliter>, 4.6) |> Drink }

    let matushkaPint =
        { BasePrice = 12<dd>
          Brand = "Matuška California"
          Type = Beer(500<milliliter>, 5.4) |> Drink }
