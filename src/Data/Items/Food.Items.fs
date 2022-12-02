module Data.Items.Food

open Entities

module FastFood =
    let genericBurger: PurchasableItem =
        { Brand = "Burger"
          Type = Burger 150<gram> |> Food |> Consumable },
        2.5m<dd>

    let genericChips: PurchasableItem =
        { Brand = "Chips"
          Type = Chips 150<gram> |> Food |> Consumable },
        0.5m<dd>

    let genericFries: PurchasableItem =
        { Brand = "Fries"
          Type = Fries 200<gram> |> Food |> Consumable },
        1.2m<dd>

    let genericNachos: PurchasableItem =
        { Brand = "Nachos"
          Type = Nachos 300<gram> |> Food |> Consumable },
        2.3m<dd>
