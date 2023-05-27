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
    let result = practiceSong dummyState dummyBand dummyFinishedSong

    match result with
    | SongImproved effects ->
        effects
        |> should
            contain
            (SongPracticed(
                dummyBand,
                dummyFinishedWithPracticeLevel 20<practice>
            ))
    | _ -> failwith "Unexpected result"

[<Test>]
let ``practiceSong should allow practice level to get to 100`` () =
    let result =
        practiceSong
            dummyState
            dummyBand
            (dummyFinishedWithPracticeLevel 80<practice>)

    match result with
    | SongImproved effects ->
        effects
        |> should
            contain
            (SongPracticed(
                dummyBand,
                dummyFinishedWithPracticeLevel 100<practice>
            ))
    | _ -> failwith "Unexpected result"

[<Test>]
let ``practiceSong should return SongAlreadyImprovedToMax if level is already 100 or more``
    ()
    =
    practiceSong
        dummyState
        dummyBand
        (dummyFinishedWithPracticeLevel 100<practice>)
    |> should
        equal
        (SongAlreadyImprovedToMax(dummyFinishedWithPracticeLevel 100<practice>))
