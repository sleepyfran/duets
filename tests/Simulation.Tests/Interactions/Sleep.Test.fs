module Duets.Simulation.Tests.Interactions.Sleep

open FsUnit
open NUnit.Framework
open Test.Common
open Test.Common.Generators

open Duets.Common
open Duets.Data
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Interactions

let bed = fst Items.Furniture.Bed.ikeaBed
let stove = fst Items.Furniture.Stove.lgStove

let state =
    let baseState = State.generateOne State.defaultOptions

    let currentDate =
        Queries.Calendar.today baseState
        |> Calendar.Transform.changeDayMoment Morning

    let character = Queries.Characters.playableCharacter baseState

    State.Calendar.setTime currentDate baseState
    |> State.Characters.setAttribute character.Id CharacterAttribute.Energy 40
    |> State.Characters.setAttribute character.Id CharacterAttribute.Health 40

let playableCharacter = Queries.Characters.playableCharacter state

[<Test>]
let ``sleep returns the correct time advancement`` () =
    let currentDate = Queries.Calendar.today state

    Sleep.sleep state currentDate Night
    |> List.filter (function
        | TimeAdvanced _ -> true
        | _ -> false)
    |> should haveLength 4

[<TestFixture>]
type ``when sleeping less than 3 day moments``() =
    let attributeChanges =
        Sleep.sleep state dummyToday Midday
        |> List.filter (function
            | CharacterAttributeChanged _ -> true
            | _ -> false)

    [<Test>]
    member x.``adds 40 points of energy per day moment``() =
        attributeChanges
        |> List.head
        |> should
            equal
            (CharacterAttributeChanged(
                playableCharacter.Id,
                CharacterAttribute.Energy,
                Diff(40, 80)
            ))

    [<Test>]
    member x.``adds 10 points of health per day moment``() =
        attributeChanges
        |> List.last
        |> should
            equal
            (CharacterAttributeChanged(
                playableCharacter.Id,
                CharacterAttribute.Health,
                Diff(40, 50)
            ))

[<TestFixture>]
type ``when sleeping more than 3 day moments``() =
    let state =
        State.Characters.setAttribute
            playableCharacter.Id
            CharacterAttribute.Energy
            0
            state
        |> State.Characters.setAttribute
            playableCharacter.Id
            CharacterAttribute.Health
            100

    let attributeChanges =
        Sleep.sleep state dummyToday Night
        |> List.filter (function
            | CharacterAttributeChanged _ -> true
            | _ -> false)

    [<Test>]
    member x.``adds 20 points of energy per day moment``() =
        attributeChanges
        |> List.head
        |> should
            equal
            (CharacterAttributeChanged(
                playableCharacter.Id,
                CharacterAttribute.Energy,
                Diff(0, 80)
            ))

    [<Test>]
    member x.``reduces 1 point of health per day moment``() =
        attributeChanges
        |> List.last
        |> should
            equal
            (CharacterAttributeChanged(
                playableCharacter.Id,
                CharacterAttribute.Health,
                Diff(100, 96)
            ))
