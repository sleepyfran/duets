module Duets.Simulation.Tests.Concerts.Live.Solo

open FsUnit
open NUnit.Framework
open Test.Common

open Aether
open Duets.Entities
open Duets.Simulation.Concerts.Live

let private stateWithSkills skills = dummyState |> addSkillsTo dummyCharacter skills

[<Test>]
let ``solo adds a solo event for the given instrument`` () =
    solo dummyState dummyOngoingConcert Guitar
    |> ongoingConcertFromResponse
    |> Optic.get Lenses.Concerts.Ongoing.events_
    |> should equal [ Solo Guitar ]

[<Test>]
let ``solo gives points based on the skill of the given instrument`` () =
    let state =
        stateWithSkills
            [ Skill.createWithLevel (SkillId.Instrument Guitar) 100
              Skill.createWithLevel (SkillId.Instrument Bass) 0 ]

    let guitarPoints =
        solo state dummyOngoingConcert Guitar |> pointsFromResponse

    let bassPoints = solo state dummyOngoingConcert Bass |> pointsFromResponse

    guitarPoints |> should be (greaterThan bassPoints)

[<Test>]
let ``solo is penalized after two solos with the same instrument`` () =
    let ongoingConcert =
        dummyOngoingConcert
        |> Optic.set Lenses.Concerts.Ongoing.events_ [ Solo Guitar; Solo Guitar ]

    solo dummyState ongoingConcert Guitar
    |> resultFromResponse
    |> should be (ofCase <@ TooManyRepetitionsPenalized @>)

[<Test>]
let ``solos with other instruments don't count towards the limit`` () =
    let ongoingConcert =
        dummyOngoingConcert
        |> Optic.set Lenses.Concerts.Ongoing.events_ [ Solo Guitar; Solo Guitar ]

    solo dummyState ongoingConcert Bass
    |> resultFromResponse
    |> should not' (be (ofCase <@ TooManyRepetitionsPenalized @>))
