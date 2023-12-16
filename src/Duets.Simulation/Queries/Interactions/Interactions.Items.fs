namespace Duets.Simulation.Queries.Internal.Interactions

open Duets.Data.Items
open Duets.Entities

module Items =
    let private electronicsInteractions electronic =
        match electronic with
        | ElectronicsItemType.Dartboard
        | ElectronicsItemType.GameConsole -> InteractiveItemInteraction.Play
        | ElectronicsItemType.TV -> InteractiveItemInteraction.Watch
        |> ItemInteraction.Interactive
        |> Interaction.Item

    let private furnitureInteractions furniture =
        match furniture with
        | FurnitureItemType.Bed -> InteractiveItemInteraction.Sleep
        | FurnitureItemType.BilliardTable -> InteractiveItemInteraction.Play
        | FurnitureItemType.Stove ->
            // Reduce the price of all food items by 50% so that cooking is
            // cheaper than buying food from a restaurant.
            Food.Index.all
            |> List.map (fun (item, price) -> item, price / 2m)
            |> InteractiveItemInteraction.Cook
        |> ItemInteraction.Interactive
        |> Interaction.Item

    let private gymEquipmentInteractions gymEquipment =
        match gymEquipment with
        | GymEquipmentItemType.WeightMachine
        | GymEquipmentItemType.Treadmill -> InteractiveItemInteraction.Exercise
        |> ItemInteraction.Interactive
        |> Interaction.Item

    /// Retrieves a list of all interactions that can performed on the given
    /// list of items.
    let internal getItemInteractions (items: Item list) =
        items
        |> List.choose (fun item ->
            match item.Type with
            | Consumable(ConsumableItemType.Drink _) ->
                ConsumableItemInteraction.Drink
                |> ItemInteraction.Consumable
                |> Interaction.Item
                |> Some
            | Consumable(ConsumableItemType.Food _) ->
                ConsumableItemInteraction.Eat
                |> ItemInteraction.Consumable
                |> Interaction.Item
                |> Some
            | Interactive(InteractiveItemType.Book _) ->
                InteractiveItemInteraction.Read
                |> ItemInteraction.Interactive
                |> Interaction.Item
                |> Some
            | Interactive(InteractiveItemType.Electronics electronicsItemType) ->
                electronicsInteractions electronicsItemType |> Some
            | Interactive(InteractiveItemType.Furniture furnitureItemType) ->
                furnitureInteractions furnitureItemType |> Some
            | Interactive(InteractiveItemType.GymEquipment gymEquipmentItemType) ->
                gymEquipmentInteractions gymEquipmentItemType |> Some
            | Key _ -> None)
        |> List.distinct
