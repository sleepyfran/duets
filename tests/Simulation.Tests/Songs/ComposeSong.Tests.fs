module Duets.Simulation.Tests.Songs.ComposeSong

open Test.Common
open NUnit.Framework
open FsUnit

open Duets.Entities
open Duets.Simulation.Songs.Composition.ComposeSong

[<Test>]
let ``composeSong should generate a SongStarted effect`` () =
    composeSong dummyState dummySong
    |> List.head
    |> should
        be
        (ofCase
            <@
                SongStarted(
                    dummyBand,
                    (UnfinishedSong dummySong, 5<quality>, 1<quality>)
                )
            @>)

[<Test>]
let ``composeSong should advance time 1 day moment`` () =
    composeSong dummyState dummySong
    |> List.item 1
    |> should be (ofCase <@ TimeAdvanced(dummyTodayOneDayMomentAfter) @>)

[<Test>]
let ``Qualities are calculated based on member skills`` () =
    let state =
        dummyState
        |> addSkillTo
            dummyCharacter
            (Skill.createWithLevel SkillId.Composition 50)
        |> addSkillTo
            dummyCharacter
            (Skill.createWithLevel (SkillId.Genre dummyBand.Genre) 50)

    composeSong state dummySong
    |> List.head
    |> should
        equal
        (SongStarted(
            dummyBand,
            (UnfinishedSong dummySong, 33<quality>, 7<quality>)
        ))

[<Test>]
let ``Qualities should be calculated based on members skills but never go above 100``
    ()
    =
    let skills =
        [ (Skill.createWithLevel SkillId.Composition 100)
          (Skill.createWithLevel (SkillId.Instrument(InstrumentType.Guitar)) 100)
          (Skill.createWithLevel (SkillId.Instrument(InstrumentType.Drums)) 100)
          (Skill.createWithLevel (SkillId.Instrument(InstrumentType.Bass)) 100)
          (Skill.createWithLevel SkillId.Composition 100)
          (Skill.createWithLevel (SkillId.Genre dummyBand.Genre) 100) ]

    let state =
        dummyStateWithMultipleMembers
        |> addSkillsTo dummyCharacter skills
        |> addSkillsTo dummyCharacter2 skills
        |> addSkillsTo dummyCharacter3 skills

    composeSong state dummySong
    |> List.head
    |> should
        equal
        (SongStarted(
            dummyBandWithMultipleMembers,
            (UnfinishedSong dummySong, 100<quality>, 20<quality>)
        ))
