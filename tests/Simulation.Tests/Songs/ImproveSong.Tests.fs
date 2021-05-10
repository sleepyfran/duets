module Simulation.Tests.Songs.ImproveSong

open Test.Common
open NUnit.Framework
open FsUnit

open Entities
open Simulation.Songs.Composition.ImproveSong

let state =
    dummyState
    |> addSkillTo dummyCharacter (Skill.createWithLevel SkillId.Composition 50)
    |> addSkillTo
        dummyCharacter
        (Skill.createWithLevel (Genre dummyBand.Genre) 50)
    |> addUnfinishedSong
        dummyBand
        (UnfinishedSong dummySong, 35<quality>, 7<quality>)

[<Test>]
let ShouldImproveIfPossibleAndReturnCanBeImproved () =
    let song = lastUnfinishedSong dummyBand state

    improveSong state dummyBand song
    |> fst
    |> should be (ofCase <@ CanBeImproved 14<quality> @>)

[<Test>]
let ShouldImproveForALastTimeIfPossibleAndReturnReachedMaxQualityInLastImprovement
    ()
    =
    let updatedState =
        addUnfinishedSong
            dummyBand
            (UnfinishedSong dummySong, 35<quality>, 28<quality>)
            dummyState

    let song =
        lastUnfinishedSong dummyBand updatedState

    improveSong updatedState dummyBand song
    |> fst
    |> should be (ofCase <@ ReachedMaxQualityInLastImprovement 35<quality> @>)

[<Test>]
let ShouldNotAllowImprovementIfReachedMaxQualityAndReturnReachMaxQualityAlready
    ()
    =
    let updatedState =
        addUnfinishedSong
            dummyBand
            (UnfinishedSong dummySong, 35<quality>, 28<quality>)
            dummyState

    let song =
        lastUnfinishedSong dummyBand updatedState

    improveSong updatedState dummyBand song
    |> fst
    |> should be (ofCase <@ ReachedMaxQualityInLastImprovement 35<quality> @>)

[<Test>]
let ShouldGenerateImprovedSongEffect () =
    let song = lastUnfinishedSong dummyBand state

    improveSong state dummyBand song
    |> snd
    |> Seq.head
    |> should
        be
        (ofCase
            <@ SongImproved(
                dummyBand,
                Diff(
                    (UnfinishedSong(dummySong), 35<quality>, 7<quality>),
                    (UnfinishedSong(dummySong), 35<quality>, 14<quality>)
                )
            ) @>)
