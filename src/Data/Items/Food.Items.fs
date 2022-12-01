module Data.Items.Food

open Entities

module FastFood =
    let genericBurger: PurchasableItem =
        { Brand = "Burger"
          Type = Burger 150<gram> |> Food |> Consumable },
        95<dd>

    let genericChips: PurchasableItem =
        { Brand = "Chips"
          Type = Chips 150<gram> |> Food |> Consumable },
        55<dd>

    let genericFries: PurchasableItem =
        { Brand = "Fries"
          Type = Fries 200<gram> |> Food |> Consumable },
        80<dd>

    let genericNachos: PurchasableItem =
        { Brand = "Nachos"
          Type = Nachos 300<gram> |> Food |> Consumable },
        85<dd>
