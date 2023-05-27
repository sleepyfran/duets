module Duets.Simulation.Tests.Songs.FinishSong

open Duets.Entities
open Test.Common
open NUnit.Framework
open FsUnit

open Duets.Simulation.Songs.Composition.FinishSong

[<Test>]
let FinishSongShouldGenerateSongFinishedEffect () =
    finishSong dummyBand (Unfinished(dummySong, 35<quality>, 7<quality>))
    |> should
        be
        (ofCase <@ SongFinished(dummyBand, (Finished(dummySong, 7<quality>))) @>)
