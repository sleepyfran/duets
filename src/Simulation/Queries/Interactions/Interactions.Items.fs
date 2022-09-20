namespace Simulation.Queries.Internal.Interactions

open Entities

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
        |> ItemInteraction.Interactive
        |> Interaction.Item

    let getItemInteractions (items: Item list) =
        items
        |> Set.ofList
        |> Set.map (fun item ->
            match item.Type with
            | Consumable (ConsumableItemType.Drink _) ->
                ConsumableItemInteraction.Drink
                |> ItemInteraction.Consumable
                |> Interaction.Item
            | Consumable (ConsumableItemType.Food _) ->
                ConsumableItemInteraction.Eat
                |> ItemInteraction.Consumable
                |> Interaction.Item
            | Interactive (InteractiveItemType.Electronics electronicsItemType) ->
                electronicsInteractions electronicsItemType
            | Interactive (InteractiveItemType.Furniture furnitureItemType) ->
                furnitureInteractions furnitureItemType)
        |> List.ofSeq
