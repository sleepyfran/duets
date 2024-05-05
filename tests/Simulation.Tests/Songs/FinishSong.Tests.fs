module Duets.Simulation.Tests.Songs.FinishSong

open Duets.Entities
open Test.Common
open NUnit.Framework
open FsUnit

open Duets.Simulation.Songs.Composition.FinishSong

[<Test>]
let ``finishSong should generate a FinishSong effect`` () =
    finishSong
        dummyState
        dummyBand
        (Unfinished(dummySong, 35<quality>, 7<quality>))
    |> List.head
    |> should be (ofCase <@ SongFinished @>)
