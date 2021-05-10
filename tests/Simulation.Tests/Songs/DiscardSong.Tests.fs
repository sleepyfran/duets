module Simulation.Tests.Songs.DiscardSong

open Entities.Types
open Test.Common
open NUnit.Framework
open FsUnit

open Simulation.Songs.Composition.DiscardSong

[<Test>]
let DiscardSongShouldGenerateSongDiscarded () =
  let unfinishedSong =
    (UnfinishedSong dummySong, 35<quality>, 7<quality>)

  discardSong dummyBand unfinishedSong
  |> should be (ofCase <@ SongDiscarded(dummyBand, unfinishedSong) @>)
