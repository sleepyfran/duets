module Duets.Simulation.Tests.Songs.PracticeSong

open Test.Common
open NUnit.Framework
open FsUnit

open Duets.Entities
open Duets.Simulation.Songs.Practice

let dummyFinishedWithPracticeLevel practice =
    let (Finished(song, quality)) = dummyFinishedSong
    Finished({ song with Practice = practice }, quality)

[<Test>]
let ``practiceSong should generate SongPracticed with increase if practice level is below 100``
    ()
    =
    RehearsalRoomPracticeSong
        {| Band = dummyBand
           Song = dummyFinishedSong |}
    |> runSucceedingAction dummyState
    |> fst
    |> should
        contain
        (SongPracticed(dummyBand, dummyFinishedWithPracticeLevel 20<practice>))

[<Test>]
let ``practiceSong should allow practice level to get to 100`` () =
    RehearsalRoomPracticeSong
        {| Band = dummyBand
           Song = (dummyFinishedWithPracticeLevel 80<practice>) |}
    |> runSucceedingAction dummyState
    |> fst
    |> should
        contain
        (SongPracticed(dummyBand, dummyFinishedWithPracticeLevel 100<practice>))

[<Test>]
let ``practiceSong should return SongAlreadyImprovedToMax if level is already 100 or more``
    ()
    =
    RehearsalRoomPracticeSong
        {| Band = dummyBand
           Song = (dummyFinishedWithPracticeLevel 100<practice>) |}
    |> runFailingAction dummyState
    |> should
        equal
        (SongAlreadyImprovedToMax(dummyFinishedWithPracticeLevel 100<practice>))
