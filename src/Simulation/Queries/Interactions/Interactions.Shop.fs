namespace Simulation.Queries.Internal.Interactions

open Entities

module Shop =
    let private itemsWithFinalPrice shop =
        let multiplier =
            shop.PriceModifier / 1<multiplier>

        shop.AvailableItems
        |> List.map (fun (item, price) -> item, price * multiplier)

    /// Gather all available interactions inside a bar, calculating all the final
    /// prices of the items in the menu.
    let rec shopInteractions shop =
        let availableItems =
            itemsWithFinalPrice shop

        [ ShopInteraction.Order availableItems
          |> Interaction.Shop
          ShopInteraction.SeeMenu availableItems
          |> Interaction.Shop ]
