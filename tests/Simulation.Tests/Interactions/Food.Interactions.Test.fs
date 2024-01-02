module Duets.Simulation.Tests.Interactions.Food

open FsUnit
open NUnit.Framework
open Test.Common.Generators

open Duets
open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Interactions

let initialState = State.generateOne State.defaultOptions

let character = Queries.Characters.playableCharacter initialState

let state =
    (initialState, Character.allAttributes)
    ||> List.fold (fun state attr ->
        state |> State.Characters.setAttribute character.Id attr 50)

[<Test>]
let ``Consuming junk food reduces health`` () =
    let effects =
        Items.perform
            state
            (Data.Items.Food.USA.all |> List.head |> fst)
            ItemInteraction.Eat
        |> Result.unwrap

    effects
    |> List.item 1
    |> should
        equal
        (CharacterAttributeChanged(
            character.Id,
            CharacterAttribute.Health,
            Diff(50, 45)
        ))

[<Test>]
let ``Consuming food increases hunger based on the amount`` () =
    let effects =
        Items.perform
            state
            (Data.Items.Food.Japanese.all |> List.head |> fst)
            ItemInteraction.Eat
        |> Result.unwrap

    effects
    |> List.item 0
    |> should
        equal
        (CharacterAttributeChanged(
            character.Id,
            CharacterAttribute.Hunger,
            Diff(50, 65)
        ))

[<Test>]
let ``Eating an item should remove it from the inventory if it was there`` () =
    let item = Data.Items.Food.Japanese.all |> List.head |> fst

    let state = state |> State.Inventory.addToCharacter item

    let effects = Items.perform state item ItemInteraction.Eat |> Result.unwrap

    effects
    |> List.filter (function
        | ItemRemovedFromCharacterInventory _ -> true
        | _ -> false)
    |> List.head
    |> should equal (ItemRemovedFromCharacterInventory item)
