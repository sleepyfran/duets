module Duets.Simulation.Tests.Songs.DiscardSong

open Duets.Entities
open Test.Common
open NUnit.Framework
open FsUnit

open Duets.Simulation.Songs.Composition.DiscardSong

[<Test>]
let DiscardSongShouldGenerateSongDiscarded () =
    let unfinishedSong =
        (UnfinishedSong dummySong, 35<quality>, 7<quality>)

    discardSong dummyBand unfinishedSong
    |> should be (ofCase <@ SongDiscarded(dummyBand, unfinishedSong) @>)
