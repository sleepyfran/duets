module Simulation.Tests.Bands.FireMember

open System.IO
open Test.Common
open NUnit.Framework
open FsUnit

open Common
open Entities
open Simulation.Bands.Members
open Simulation.Calendar.Queries

let bandMember =
  let hiredCharacter =
    Character.from "Test" 18 Other |> Result.unwrap

  Band.Member.from hiredCharacter Guitar (today ())

[<SetUp>]
let Setup () =
  initStateWithDummies ()

  let band = currentBand ()
  addMember band bandMember

let fireMember memberToFire =
  let updatedBand = currentBand ()
  updatedBand.Members |> should haveLength 2
  fireMember updatedBand memberToFire

[<Test>]
let FireMemberFailsIfGivenMemberIsPlayableCharacter () =
  let playableMember =
    Band.Member.from dummyCharacter Guitar (today ())

  fireMember playableMember
  |> Result.unwrapError
  |> should be (ofCase <@ AttemptToFirePlayableCharacter @>)

[<Test>]
let FireMemberRemovesTheMemberFromTheBand () =
  fireMember bandMember |> ignore
  let band = currentBand ()
  band.Members |> should haveLength 1

[<Test>]
let FireMemberAddsTheMemberToPastMembers () =
  fireMember bandMember |> ignore
  let band = currentBand ()
  band.PastMembers |> should haveLength 1

[<Test>]
let FiredMemberShouldHaveTodayAsFiredDay () =
  fireMember bandMember |> ignore
  let band = currentBand ()

  List.head band.PastMembers
  |> fun m -> m.Period
  |> snd
  |> fun periodEnd ->
       match periodEnd with
       | Date date -> date
       | _ -> raise <| InvalidDataException()
  |> should equal (today ())
