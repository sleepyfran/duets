module Duets.Simulation.Tests.Interactions.Drink

open FsUnit
open NUnit.Framework
open Test.Common.Generators

open Duets.Common
open Duets.Entities
open Duets
open Duets.Simulation
open Duets.Simulation.Interactions

let state = State.generateOne State.defaultOptions

let character = Queries.Characters.playableCharacter state

[<Test>]
let ``Drinking a beer of 500ml and 4.4 in alcohol increases drunkenness by 6``
    ()
    =
    let effects =
        Items.perform
            state
            (fst Data.Items.Drink.Beer.pilsnerUrquellPint)
            ItemInteraction.Drink
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
        Items.perform
            state
            (fst Data.Items.Drink.Beer.matushkaPint)
            ItemInteraction.Drink
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
        Items.perform
            state
            (fst Data.Items.Drink.Beer.matushkaPint)
            ItemInteraction.Drink
        |> Result.unwrap

    let updatedState =
        Simulation.State.Root.applyEffect state (effects |> List.item 0)

    let updatedEffects =
        Items.perform
            updatedState
            (fst Data.Items.Drink.Beer.matushkaPint)
            ItemInteraction.Drink
        |> Result.unwrap

    let updatedCharacter = Queries.Characters.playableCharacter updatedState

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
    let item = fst Data.Items.Drink.Beer.pilsnerUrquellPint

    let state = state |> State.Inventory.addToCharacter item

    let effects =
        Items.perform state item ItemInteraction.Drink |> Result.unwrap

    effects
    |> List.filter (function
        | ItemRemovedFromCharacterInventory _ -> true
        | _ -> false)
    |> List.head
    |> should equal (ItemRemovedFromCharacterInventory item)

[<Test>]
let ``Drinking non-drink items should not be allowed`` () =
    let item = Data.Items.Food.Czech.all |> List.head |> fst

    Items.perform state item ItemInteraction.Drink
    |> Result.unwrapError
    |> should be (ofCase <@@ Items.ActionNotPossible @@>)
