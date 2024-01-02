namespace Duets.Simulation.Queries.Internal.Interactions

open Duets.Data.Items
open Duets.Entities

module rec Items =
    /// Retrieves a list of all interactions that can performed on the given
    /// list of items.
    let internal getItemInteractions (items: Item list) =
        items
        |> List.collect (fun item ->
            item.Properties
            |> List.choose (function
                | Cookware -> cookingInteractions |> Some
                | Drinkable _ ->
                    ItemInteraction.Drink |> Interaction.Item |> Some
                | Deliverable _ ->
                    None (* These interactions are defined at the place level. *)
                | Edible _ -> ItemInteraction.Eat |> Interaction.Item |> Some
                | FitnessEquipment ->
                    ItemInteraction.Exercise |> Interaction.Item |> Some
                | Key _ -> None
                | Listenable _ ->
                    None (* TODO: Add "listen" interactions once we support buying albums. *)
                | PlaceableInStorage _ ->
                    ItemInteraction.Put |> Interaction.Item |> Some
                | Playable _ ->
                    ItemInteraction.Play |> Interaction.Item |> Some
                | Readable _ ->
                    ItemInteraction.Read |> Interaction.Item |> Some
                | Storage _ -> ItemInteraction.Open |> Interaction.Item |> Some
                | Sleepable ->
                    ItemInteraction.Sleep |> Interaction.Item |> Some
                | Watchable ->
                    ItemInteraction.Watch |> Interaction.Item |> Some
                | Wearable _ -> None (* TODO: Add "wear" interactions once we support buying clothes *) ))
        |> List.distinct

    let private cookingInteractions =
        Food.Index.all
        |> List.map (fun (item, price) -> item, price / 2m)
        |> List.sortBy (fun (item, _) -> item.Brand)
        |> ItemInteraction.Cook
        |> Interaction.Item
