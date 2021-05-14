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

let createSongImprovedEffect max prev current =
    SongImproved(
        dummyBand,
        Diff(
            (UnfinishedSong(dummySong), max * 1<quality>, prev * 1<quality>),
            (UnfinishedSong(dummySong), max * 1<quality>, current * 1<quality>)
        )
    )

[<Test>]
let ShouldImproveIfPossibleAndReturnCanBeImproved () =
    let song = lastUnfinishedSong dummyBand state

    improveSong dummyBand song
    |> fun status ->
        match status with
        | CanBeImproved effect -> effect
        | _ -> invalidOp "Unexpected status"
    |> should equal (createSongImprovedEffect 35 7 14)

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

    improveSong dummyBand song
    |> should
        be
        (ofCase
            <@ ReachedMaxQualityInLastImprovement(
                createSongImprovedEffect 35 21 28
            ) @>)

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

    improveSong dummyBand song
    |> should
        be
        (ofCase
            <@ ReachedMaxQualityInLastImprovement(
                createSongImprovedEffect 35 21 28
            ) @>)
