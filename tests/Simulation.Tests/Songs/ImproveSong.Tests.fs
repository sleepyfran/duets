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
        (Skill.createWithLevel (SkillId.Genre dummyBand.Genre) 50)
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
let ``Should improve song if it's possible, return CanBeImproved and advance one day moment`` () =
    let song = lastUnfinishedSong dummyBand state

    let result = improveSong dummyState dummyBand song

    fst result |> should be (ofCase <@ CanBeImproved @>)
    snd result |> should contain (createSongImprovedEffect 35 7 14)
    snd result |> should contain (TimeAdvanced(dummyTodayOneDayMomentAfter))

[<Test>]
let ``Should improve for one last time if possible, return ReachedMaxQualityInLastImprovement and advance one day moment``
    ()
    =
    let updatedState =
        addUnfinishedSong
            dummyBand
            (UnfinishedSong dummySong, 35<quality>, 28<quality>)
            dummyState

    let song = lastUnfinishedSong dummyBand updatedState

    let result = improveSong updatedState dummyBand song

    fst result |> should be (ofCase <@ ReachedMaxQualityInLastImprovement @>)
    snd result |> should contain (createSongImprovedEffect 35 28 35)
    snd result |> should contain (TimeAdvanced(dummyTodayOneDayMomentAfter))

[<Test>]
let ``Should not allow improvement if it already reached max quality and return ReachedMaxQualityAlready``
    ()
    =
    let updatedState =
        addUnfinishedSong
            dummyBand
            (UnfinishedSong dummySong, 35<quality>, 35<quality>)
            dummyState

    let song = lastUnfinishedSong dummyBand updatedState

    let result = improveSong dummyState dummyBand song

    fst result |> should be (ofCase (<@ ReachedMaxQualityAlready @>))
    snd result |> should haveLength 0
