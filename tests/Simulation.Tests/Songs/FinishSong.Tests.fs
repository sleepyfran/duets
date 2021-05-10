module Simulation.Tests.Songs.FinishSong

open Entities.Types
open Test.Common
open NUnit.Framework
open FsUnit

open Simulation.Songs.Composition.FinishSong

[<Test>]
let FinishSongShouldGenerateSongFinishedEffect () =
    finishSong dummyBand (UnfinishedSong dummySong, 35<quality>, 7<quality>)
    |> should
        be
        (ofCase
            <@ SongFinished(dummyBand, (FinishedSong dummySong, 7<quality>)) @>)
