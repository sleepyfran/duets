module Simulation.Tests.Songs.ComposeSong

open Test.Common
open NUnit.Framework
open FsUnit

open Entities
open Simulation.Songs.Composition.ComposeSong

[<Test>]
let ComposeSongShouldGenerateSongComposedEffectWithDefaultQualities () =
    composeSong dummyState dummySong
    |> should
        be
        (ofCase
            <@ SongStarted(
                dummyBand,
                (UnfinishedSong dummySong, 5<quality>, 1<quality>)
            ) @>)

[<Test>]
let QualitiesShouldBeCalculatedBasedOnBandMemberSkills () =
    let state =
        dummyState
        |> addSkillTo
            dummyCharacter
            (Skill.createWithLevel SkillId.Composition 50)
        |> addSkillTo
            dummyCharacter
            (Skill.createWithLevel (Genre dummyBand.Genre) 50)

    composeSong state dummySong
    |> should
        be
        (ofCase
            <@ SongStarted(
                dummyBand,
                (UnfinishedSong dummySong, 33<quality>, 7<quality>)
            ) @>)
