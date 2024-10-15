module Duets.Simulation.Tests.Songs.ImproveSong

open Test.Common
open NUnit.Framework
open FsUnit

open FSharp.Data.UnitSystems.SI.UnitNames
open Duets.Entities
open Duets.Simulation.Songs.Composition.ImproveSong

let state =
    dummyState
    |> addSkillTo dummyCharacter (Skill.createWithLevel SkillId.Composition 50)
    |> addSkillTo
        dummyCharacter
        (Skill.createWithLevel (SkillId.Genre dummyBand.Genre) 50)
    |> addUnfinishedSong
        dummyBand
        (Unfinished(dummySong, 35<quality>, 7<quality>))

let createSongImprovedEffect song max prev current =
    SongImproved(
        dummyBand,
        Diff(
            Unfinished(song, max * 1<quality>, prev * 1<quality>),
            Unfinished(song, max * 1<quality>, current * 1<quality>)
        )
    )

[<Test>]
let ``Should improve song if it's possible, return CanBeImproved and advance one day moment``
    ()
    =
    let song = lastUnfinishedSong dummyBand state

    let result = improveSong dummyState dummyBand song

    fst result |> should be (ofCase <@ CanBeImproved @>)
    snd result |> should contain (createSongImprovedEffect dummySong 35 7 14)

[<Test>]
let ``Should make improvement process slower the longer the song is`` () =
    [ 3<minute>, 10; 6<minute>, 8; 12<minute>, 5; 24<minute>, 4 ]
    |> List.iter (fun (minutes, expectedQuality) ->
        let state =
            state
            |> addUnfinishedSong
                dummyBand
                (Unfinished(
                    { dummySong with
                        Length =
                            { Minutes = minutes
                              Seconds = 0<second> } },
                    50<quality>,
                    0<quality>
                ))

        let unfinishedSong = lastUnfinishedSong dummyBand state
        let song = Song.fromUnfinished unfinishedSong

        let result = improveSong dummyState dummyBand unfinishedSong

        fst result |> should be (ofCase <@ CanBeImproved @>)

        snd result
        |> should contain (createSongImprovedEffect song 50 0 expectedQuality))

[<Test>]
let ``Should improve for one last time if possible, return ReachedMaxQualityInLastImprovement and advance one day moment``
    ()
    =
    let updatedState =
        addUnfinishedSong
            dummyBand
            (Unfinished(dummySong, 35<quality>, 28<quality>))
            dummyState

    let song = lastUnfinishedSong dummyBand updatedState

    let result = improveSong updatedState dummyBand song

    fst result |> should be (ofCase <@ ReachedMaxQualityInLastImprovement @>)
    snd result |> should contain (createSongImprovedEffect dummySong 35 28 35)

[<Test>]
let ``Should not allow improvement if it already reached max quality and return ReachedMaxQualityAlready``
    ()
    =
    let updatedState =
        addUnfinishedSong
            dummyBand
            (Unfinished(dummySong, 35<quality>, 35<quality>))
            dummyState

    let song = lastUnfinishedSong dummyBand updatedState

    let result = improveSong dummyState dummyBand song

    fst result |> should be (ofCase (<@ ReachedMaxQualityAlready @>))
    snd result |> should haveLength 0
