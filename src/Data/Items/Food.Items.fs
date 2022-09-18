module Data.Items.Food

open Entities

/// Defines all types of fast food available in the game.
module FastFood =
    let genericBurger: PurchasableItem =
        { Brand = "Generic"
          Type = Burger 150<gram> |> Food },
        95<dd>

    let genericFries: PurchasableItem =
        { Brand = "Generic"
          Type = Fries 200<gram> |> Food },
        80<dd>

    let genericNachos: PurchasableItem =
        { Brand = "Generic"
          Type = Nachos 300<gram> |> Food },
        85<dd>
