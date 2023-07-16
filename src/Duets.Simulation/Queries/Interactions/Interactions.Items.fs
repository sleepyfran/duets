namespace Duets.Simulation.Queries.Internal.Interactions

open Duets.Data.Items
open Duets.Entities

module Items =
    let private electronicsInteractions electronic =
        match electronic with
        | ElectronicsItemType.TV -> InteractiveItemInteraction.Watch
        | ElectronicsItemType.GameConsole -> InteractiveItemInteraction.Play
        |> ItemInteraction.Interactive
        |> Interaction.Item

    let private furnitureInteractions furniture =
        match furniture with
        | FurnitureItemType.Bed -> InteractiveItemInteraction.Sleep
        | FurnitureItemType.Stove ->
            // Reduce the price of all food items by 50% so that cooking is
            // cheaper than buying food from a restaurant.
            Food.Index.all
            |> List.map (fun (item, price) -> item, price / 2m)
            |> InteractiveItemInteraction.Cook
        |> ItemInteraction.Interactive
        |> Interaction.Item

    let internal getItemInteractions (items: Item list) =
        items
        |> Set.ofList
        |> Set.map (fun item ->
            match item.Type with
            | Consumable(ConsumableItemType.Drink _) ->
                ConsumableItemInteraction.Drink
                |> ItemInteraction.Consumable
                |> Interaction.Item
            | Consumable(ConsumableItemType.Food _) ->
                ConsumableItemInteraction.Eat
                |> ItemInteraction.Consumable
                |> Interaction.Item
            | Interactive(InteractiveItemType.Electronics electronicsItemType) ->
                electronicsInteractions electronicsItemType
            | Interactive(InteractiveItemType.Furniture furnitureItemType) ->
                furnitureInteractions furnitureItemType)
        |> List.ofSeq
