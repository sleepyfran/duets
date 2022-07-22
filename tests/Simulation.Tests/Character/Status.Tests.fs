module Simulation.Tests.Character.Status

open FsUnit
open NUnit.Framework
open Test.Common
open Test.Common.Generators

open Entities
open Simulation

[<Test>]
let ``addHealth returns CharacterHealthChange when result health is more than 5``
    ()
    =
    Character.Status.addHealth dummyCharacter -25
    |> should equal (CharacterHealthChange(dummyCharacter, 75))

[<Test>]
let ``addHealth returns CharacterHealthDepleted when result health is lower than 5``
    ()
    =
    Character.Status.addHealth dummyCharacter -95
    |> should equal (CharacterHealthDepleted dummyCharacter)
