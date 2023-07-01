namespace Duets.Simulation.Queries.Internal.Interactions

open Duets.Entities

module Shop =
    let private itemsWithFinalPrice shop =
        let multiplier = shop.PriceModifier |> decimal

        shop.AvailableItems
        |> List.map (fun (item, price) -> item, price * multiplier)

    /// Gather all available interactions inside a bar, calculating all the final
    /// prices of the items in the menu.
    let internal interactions shop =
        let availableItems = itemsWithFinalPrice shop

        [ ShopInteraction.Order availableItems |> Interaction.Shop
          ShopInteraction.SeeMenu availableItems |> Interaction.Shop ]

module Bar =
    /// Gather all available interactions inside a bar.
    let internal interactions roomType =
        match roomType with
        | RoomType.Bar shop -> Shop.interactions shop
        | _ -> []

module Cafe =
    /// Gather all available interactions inside a cafe.
    let internal interactions roomType =
        match roomType with
        | RoomType.Cafe shop -> Shop.interactions shop
        | _ -> []

module Restaurant =
    /// Gather all available interactions inside a restaurant.
    let internal interactions roomType =
        match roomType with
        | RoomType.Restaurant shop -> Shop.interactions shop
        | _ -> []
