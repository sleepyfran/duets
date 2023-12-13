module Duets.Simulation.Tests.Interactions.Exercise

open Duets.Entities
open FsUnit
open NUnit.Framework
open Test.Common
open Test.Common.Generators

open Duets.Common
open Duets.Data
open Duets.Simulation
open Duets.Simulation.Interactions

let initialState = State.generateOne State.defaultOptions

let state =
    let character = initialState |> Queries.Characters.playableCharacter

    initialState
    |> State.Characters.setAttribute character.Id CharacterAttribute.Health 50


[<Test>]
let ``exercising in non-gym equipment is not allowed`` () =
    [ Items.Drink.Beer.alhambra
      Items.Furniture.Bed.ikeaBed
      Items.Furniture.Stove.lgStove ]
    |> List.iter (fun item ->
        Items.interact state (fst item) InteractiveItemInteraction.Exercise
        |> Result.unwrapError
        |> should equal Items.ActionNotPossible)

[<Test>]
let ``exercising in gym equipment advances time by one day moment`` () =
    Items.interact
        state
        (fst Items.Gym.Treadmills.elliptical)
        InteractiveItemInteraction.Exercise
    |> Result.unwrap
    |> List.item 0
    |> should be (ofCase <@ TimeAdvanced @>)

[<Test>]
let ``exercising in gym equipment decreases energy`` () =
    Items.interact
        state
        (fst Items.Gym.Treadmills.elliptical)
        InteractiveItemInteraction.Exercise
    |> Result.unwrap
    |> List.item 1
    |> function
        | CharacterAttributeChanged(_, attr, amount) ->
            attr |> should equal CharacterAttribute.Energy
            amount |> should equal (Diff(100, 75))
        | _ -> failwith "Effect was not of correct type"

[<Test>]
let ``exercising in gym equipment increases health`` () =
    Items.interact
        state
        (fst Items.Gym.Treadmills.elliptical)
        InteractiveItemInteraction.Exercise
    |> Result.unwrap
    |> List.item 2
    |> function
        | CharacterAttributeChanged(_, attr, amount) ->
            attr |> should equal CharacterAttribute.Health
            amount |> should equal (Diff(50, 58))
        | _ -> failwith "Effect was not of correct type"

[<Test>]
let ``exercising in gym equipment increases skill if chance succeeds`` () =
    use _ = changeToStaticRandom 10

    Items.interact
        state
        (fst Items.Gym.Treadmills.elliptical)
        InteractiveItemInteraction.Exercise
    |> Result.unwrap
    |> List.item 3
    |> function
        | SkillImproved(_, Diff(_, (skill, level))) ->
            skill.Id |> should equal SkillId.Fitness
            level |> should equal 1
        | _ -> failwith "Effect was not of correct type"

[<Test>]
let ``exercising in gym equipment does not increase skill if chance does not succeed``
    ()
    =
    use _ = changeToStaticRandom 100

    Items.interact
        state
        (fst Items.Gym.Treadmills.elliptical)
        InteractiveItemInteraction.Exercise
    |> Result.unwrap
    |> List.filter (function
        | SkillImproved _ -> true
        | _ -> false)
    |> should haveLength 0
