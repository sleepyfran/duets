module Simulation.Tests.Interactions.Food

open FsUnit
open NUnit.Framework
open Test.Common.Generators

open Common
open Entities
open Simulation
open Simulation.Interactions

let initialState = State.generateOne State.defaultOptions

let character = Queries.Characters.playableCharacter initialState

let state =
    (initialState, Character.allAttributes)
    ||> List.fold (fun state attr ->
        state |> State.Characters.setAttribute character.Id attr 50)

[<Test>]
let ``Consuming junk food reduces health`` () =
    let effects =
        Items.consume
            state
            (fst Data.Items.Food.FastFood.genericBurger)
            ConsumableItemInteraction.Eat
        |> Result.unwrap

    effects
    |> List.item 1
    |> should
        equal
        (CharacterAttributeChanged(
            character.Id,
            CharacterAttribute.Health,
            Diff(50, 46)
        ))

[<Test>]
let ``Consuming food increases hunger based on the amount`` () =
    let effects =
        Items.consume
            state
            (fst Data.Items.Food.JapaneseFood.misoRamen)
            ConsumableItemInteraction.Eat
        |> Result.unwrap

    effects
    |> List.item 0
    |> should
        equal
        (CharacterAttributeChanged(
            character.Id,
            CharacterAttribute.Hunger,
            Diff(50, 100)
        ))
