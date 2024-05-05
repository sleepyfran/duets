module Duets.Simulation.Tests.Songs.DiscardSong

open Duets.Entities
open Test.Common
open NUnit.Framework
open FsUnit

open Duets.Simulation.Songs.Composition.DiscardSong

[<Test>]
let ``discardSong should generate SongDiscarded effect`` () =
    let unfinishedSong = Unfinished(dummySong, 35<quality>, 7<quality>)

    discardSong dummyBand unfinishedSong
    |> List.head
    |> should be (ofCase <@ SongDiscarded(dummyBand, unfinishedSong) @>)
