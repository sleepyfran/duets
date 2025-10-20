namespace Duets.Simulation.Queries.Internal.Interactions

open Duets.Common.Math
open Duets.Data.Items
open Duets.Entities
open Duets.Entities.SituationTypes
open Duets.Simulation.Queries

module rec Items =
    /// Retrieves a list of all interactions that can performed on the given
    /// list of items.
    let internal getItemInteractions state currentCoords (items: Item list) =
        items
        |> List.collect (fun item ->
            item.Properties
            |> List.choose (function
                | Cookware -> cookingInteractions state |> Some
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
                | Rideable vehicle ->
                    let situation =
                        match vehicle with
                        | Car _ -> TravellingByCar(currentCoords, item)
                        | Metro -> TravellingByMetro

                    ItemInteraction.Ride(vehicle, situation)
                    |> Interaction.Item
                    |> Some
                | Storage _ -> ItemInteraction.Open |> Interaction.Item |> Some
                | Sleepable ->
                    ItemInteraction.Sleep |> Interaction.Item |> Some
                | Watchable ->
                    ItemInteraction.Watch |> Interaction.Item |> Some
                | Wearable _ -> None (* TODO: Add "wear" interactions once we support buying clothes *) ))
        |> List.distinct

    let private cookingInteractions state =
        let playableCharacter = Characters.playableCharacter state

        let _, cookingSkillLevel =
            Skills.characterSkillWithLevel
                state
                playableCharacter.Id
                SkillId.Cooking

        Food.Index.all
        |> List.filter (fun (item, _) ->
            let edibleProperty = Item.Property.tryMain item

            match edibleProperty with
            | Some(ItemProperty.Edible(edibleItem)) ->
                edibleItem.CookingSkillRequired
                |> between 0 (cookingSkillLevel + 5)
            | _ -> false)
        |> List.map (fun (item, price) -> item, price / 2m)
        |> List.sortBy (fun (item, _) -> item.Brand)
        |> ItemInteraction.Cook
        |> Interaction.Item
