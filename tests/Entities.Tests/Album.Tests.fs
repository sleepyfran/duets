module Entities.Tests.Album

open FSharp.Data.UnitSystems.SI.UnitNames
open FsUnit
open NUnit.Framework
open Test.Common

open Common
open Entities

[<Test>]
let ``validateName should fail if name is empty`` () =
    Album.validateName ""
    |> Result.unwrapError
    |> should be (ofCase <@ Album.NameTooShort @>)

[<Test>]
let ``validateName should fail if name is too long`` () =
    Album.validateName
        "Nothing to Go Home To, Nothing There to Come Home For, No Home to Return To,  Nothing to Go Home To, Nothing There to Come Home For, No Home to Return To"
    |> Result.unwrapError
    |> should be (ofCase <@ Album.NameTooLong @>)

[<Test>]
let ``validateName should succeed if given normal name`` () =
    Album.validateName "Infinite Granite"
    |> Result.unwrap
    |> should equal "Infinite Granite"

[<Test>]
let ``Album.recordType should return error if track list is empty`` () =
    Album.recordType []
    |> Result.unwrapError
    |> should be (ofCase <@ Album.EmptyTrackList @>)

[<Test>]
let ``Album.recordType should return single if only one song is given`` () =
    Album.recordType [ dummyRecordedSong ]
    |> Result.unwrap
    |> should be (ofCase <@ Single @>)

[<Test>]
let ``Album.recordType should return EP if given more than one song but its length is less than 25 minutes``
    ()
    =
    [ { Minutes = 23<minute>
        Seconds = 10<second> }
      { Minutes = 3<minute>
        Seconds = 0<second> }
      { Minutes = 0<minute>
        Seconds = 10<second> } ]
    |> List.iter
        (fun length ->
            Album.recordType [
                dummyRecordedSong
                (dummyRecordedSongWithLength length)
            ]
            |> Result.unwrap
            |> should be (ofCase <@ EP @>))

[<Test>]
let ``Album.recordType should return LP if given more than one song and its length is more than 25 minutes``
    ()
    =
    [ { Minutes = 25<minute>
        Seconds = 10<second> }
      { Minutes = 35<minute>
        Seconds = 10<second> }
      { Minutes = 72<minute>
        Seconds = 4<second> } ]
    |> List.iter
        (fun length ->
            Album.recordType [
                dummyRecordedSong
                (dummyRecordedSongWithLength length)
            ]
            |> Result.unwrap
            |> should be (ofCase <@ LP @>))
