namespace Simulation.Queries.Internal.Interactions

open Entities

module Shop =
    let private itemsWithFinalPrice shop =
        let multiplier = shop.PriceModifier / 1<multiplier>

        shop.AvailableItems
        |> List.map (fun item -> { item with Price = item.Price * multiplier })

    /// Gather all available interactions inside a bar, calculating all the final
    /// prices of the items in the menu.
    let rec barInteractions bar =
        let availableItems = itemsWithFinalPrice bar

        [ BarInteraction.Order availableItems
          |> Interaction.Bar
          BarInteraction.SeeMenu availableItems
          |> Interaction.Bar ]
