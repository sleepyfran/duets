module Simulation.Tests.Songs.PracticeSong

open Test.Common
open NUnit.Framework
open FsUnit

open Entities
open Simulation.Songs.Practice

let dummyFinishedWithPracticeLevel practice =
    let (FinishedSong song, _) = dummyFinishedSong
    (FinishedSong { song with Practice = practice }, snd dummyFinishedSong)

[<Test>]
let ``practiceSong should generate SongPracticed with increase if practice level is below 100``
    ()
    =
    practiceSong dummyBand dummyFinishedSong
    |> should
        equal
        (SongImproved(
            SongPracticed(
                dummyBand,
                dummyFinishedWithPracticeLevel 20<practice>
            )
        ))

[<Test>]
let ``practiceSong should allow practice level to get to 100`` () =
    practiceSong dummyBand (dummyFinishedWithPracticeLevel 80<practice>)
    |> should
        equal
        (SongImproved(
            SongPracticed(
                dummyBand,
                dummyFinishedWithPracticeLevel 100<practice>
            )
        ))

[<Test>]
let ``practiceSong should return SongAlreadyImprovedToMax if level is already 100 or more``
    ()
    =
    practiceSong dummyBand (dummyFinishedWithPracticeLevel 100<practice>)
    |> should
        equal
        (SongAlreadyImprovedToMax(dummyFinishedWithPracticeLevel 100<practice>))
