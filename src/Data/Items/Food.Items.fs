module Data.Items.Food

open Entities

module FastFood =
    let genericBurger: PurchasableItem =
        { Brand = "Generic"
          Type = Burger 150<gram> |> Food |> Consumable },
        95<dd>

    let genericFries: PurchasableItem =
        { Brand = "Generic"
          Type = Fries 200<gram> |> Food |> Consumable },
        80<dd>

    let genericNachos: PurchasableItem =
        { Brand = "Generic"
          Type = Nachos 300<gram> |> Food |> Consumable },
        85<dd>
