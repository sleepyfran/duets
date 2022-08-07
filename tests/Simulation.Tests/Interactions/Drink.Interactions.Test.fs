module Simulation.Tests.Interactions.Drink

open FsUnit
open NUnit.Framework
open Test.Common.Generators

open Common
open Entities
open Simulation
open Simulation.Interactions

let state =
    State.generateOne State.defaultOptions

let character =
    Queries.Characters.playableCharacter state

[<Test>]
let ``Drinking a beer of 500ml and 4.4 in alcohol increases drunkenness by 6``
    ()
    =
    let effects =
        Items.consume state Data.Items.Drink.Beer.pilsnerUrquellPint Items.Drink
        |> Result.unwrap

    effects
    |> List.item 1
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
        Items.consume state Data.Items.Drink.Beer.matushkaPint Items.Drink
        |> Result.unwrap

    effects
    |> List.item 1
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
        Items.consume state Data.Items.Drink.Beer.matushkaPint Items.Drink
        |> Result.unwrap

    let updatedState =
        Simulation.State.Root.applyEffect state (effects |> List.item 1)

    let updatedEffects =
        Items.consume
            updatedState
            Data.Items.Drink.Beer.matushkaPint
            Items.Drink
        |> Result.unwrap

    let updatedCharacter =
        Queries.Characters.playableCharacter updatedState

    updatedEffects
    |> List.item 1
    |> should
        equal
        (CharacterAttributeChanged(
            updatedCharacter.Id,
            CharacterAttribute.Drunkenness,
            Diff(8, 16)
        ))

[<Test>]
let ``Drinking any item should remove it from the inventory`` () =
    let item =
        Data.Items.Drink.Beer.pilsnerUrquellPint

    let effects =
        Items.consume state item Items.Drink
        |> Result.unwrap

    effects
    |> List.item 0
    |> should equal (InventoryItemRemoved item)
