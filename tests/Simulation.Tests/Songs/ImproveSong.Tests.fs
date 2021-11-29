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
let ``Should improve song if it's possible and return CanBeImproved`` () =
    let song = lastUnfinishedSong dummyBand state

    improveSong dummyBand song
    |> fun status ->
        match status with
        | (CanBeImproved, effects) -> effects
        | _ -> invalidOp "Unexpected status"
    |> should equal [ createSongImprovedEffect 35 7 14 ]

[<Test>]
let ``Should improve for one last time if possible and return ReachedMaxQualityInLastImprovement``
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
        equal
        (ReachedMaxQualityInLastImprovement,
         [ createSongImprovedEffect 35 28 35 ])

[<Test>]
let ``Should not allow improvement if it already reached max quality and return ReachedMaxQualityAlready``
    ()
    =
    let updatedState =
        addUnfinishedSong
            dummyBand
            (UnfinishedSong dummySong, 35<quality>, 35<quality>)
            dummyState

    let song =
        lastUnfinishedSong dummyBand updatedState

    let result = improveSong dummyBand song

    result
    |> fst
    |> should be (ofCase (<@ ReachedMaxQualityAlready @>))

    result |> snd |> should haveLength 0
