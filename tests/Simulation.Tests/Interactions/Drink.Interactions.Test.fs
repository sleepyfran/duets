module Duets.Simulation.Tests.Interactions.Drink

open FsUnit
open NUnit.Framework
open Test.Common.Generators

open Duets.Common
open Duets.Entities
open Duets
open Duets.Simulation
open Duets.Simulation.Interactions

let state =
    State.generateOne State.defaultOptions

let character =
    Queries.Characters.playableCharacter state

[<Test>]
let ``Drinking a beer of 500ml and 4.4 in alcohol increases drunkenness by 6``
    ()
    =
    let effects =
        Items.consume
            state
            (fst Data.Items.Drink.Beer.pilsnerUrquellPint)
            ConsumableItemInteraction.Drink
        |> Result.unwrap

    effects
    |> List.item 0
    |> should
        equal
        (CharacterAttributeChanged(
            character.Id,
            CharacterAttribute.Drunkenness,
            Diff(0, 6)
        ))

[<Test>]
let ``Drinking a beer of 500ml and 5.4 in alcohol increases drunkenness by 8``
    ()
    =
    let effects =
        Items.consume
            state
            (fst Data.Items.Drink.Beer.matushkaPint)
            ConsumableItemInteraction.Drink
        |> Result.unwrap

    effects
    |> List.item 0
    |> should
        equal
        (CharacterAttributeChanged(
            character.Id,
            CharacterAttribute.Drunkenness,
            Diff(0, 8)
        ))

[<Test>]
let ``Drinking two beers of 500ml and 5.4 in alcohol increases drunkenness by 16``
    ()
    =
    let effects =
        Items.consume
            state
            (fst Data.Items.Drink.Beer.matushkaPint)
            ConsumableItemInteraction.Drink
        |> Result.unwrap

    let updatedState =
        Simulation.State.Root.applyEffect state (effects |> List.item 0)

    let updatedEffects =
        Items.consume
            updatedState
            (fst Data.Items.Drink.Beer.matushkaPint)
            ConsumableItemInteraction.Drink
        |> Result.unwrap

    let updatedCharacter =
        Queries.Characters.playableCharacter updatedState

    updatedEffects
    |> List.item 0
    |> should
        equal
        (CharacterAttributeChanged(
            updatedCharacter.Id,
            CharacterAttribute.Drunkenness,
            Diff(8, 16)
        ))

[<Test>]
let ``Drinking an item should remove it from the inventory if it was there``
    ()
    =
    let item =
        fst Data.Items.Drink.Beer.pilsnerUrquellPint

    let state =
        state |> State.Inventory.add item

    let effects =
        Items.consume state item ConsumableItemInteraction.Drink
        |> Result.unwrap

    effects
    |> List.item 0
    |> should equal (ItemRemovedFromInventory item)

[<Test>]
let ``Drinking non-drink items should not be allowed`` () =
    let item =
        fst Data.Items.Food.FastFood.genericBurger

    Items.consume state item ConsumableItemInteraction.Drink
    |> Result.unwrapError
    |> should be (ofCase <@@ Items.ActionNotPossible @@>)
