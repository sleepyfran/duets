namespace Duets.Simulation.Queries.Internal.Interactions

open Duets.Entities

module Shop =
    let private itemsWithFinalPrice shop =
        let multiplier = shop.PriceModifier |> decimal

        shop.AvailableItems
        |> List.map (fun (item, price) -> item, price * multiplier)

    /// Gather all available interactions inside a bar, calculating all the final
    /// prices of the items in the menu.
    let interactions shop =
        let availableItems = itemsWithFinalPrice shop

        [ ShopInteraction.Order availableItems |> Interaction.Shop
          ShopInteraction.SeeMenu availableItems |> Interaction.Shop ]

module Bar =
    /// Gather all available interactions inside a bar.
    let interactions roomType shop =
        match roomType with
        | RoomType.Bar -> Shop.interactions shop
        | _ -> []

module Cafe =
    /// Gather all available interactions inside a cafe.
    let interactions roomType shop =
        match roomType with
        | RoomType.Cafe -> Shop.interactions shop
        | _ -> []

module Restaurant =
    /// Gather all available interactions inside a restaurant.
    let interactions roomType shop =
        match roomType with
        | RoomType.Restaurant -> Shop.interactions shop
        | _ -> []
