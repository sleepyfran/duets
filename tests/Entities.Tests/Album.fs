module Entities.Tests.Album

open FSharp.Data.UnitSystems.SI.UnitNames
open FsUnit
open NUnit.Framework
open Test.Common

open Common
open Entities

[<Test>]
let ``recordAlbum should fail if track list is empty`` () =
    Album.from "test" []
    |> Result.unwrapError
    |> should be (ofCase <@ Album.NoSongsSelected @>)

[<Test>]
let ``recordAlbum should fail if name is empty`` () =
    Album.from "" [ dummyRecordedSong ]
    |> Result.unwrapError
    |> should be (ofCase <@ Album.NameTooShort @>)

[<Test>]
let ``recordAlbum should fail if name is too long`` () =
    Album.from
        "Nothing to Go Home To, Nothing There to Come Home For, No Home to Return To,  Nothing to Go Home To, Nothing There to Come Home For, No Home to Return To"
        [ dummyRecordedSong ]
    |> Result.unwrapError
    |> should be (ofCase <@ Album.NameTooLong @>)

[<Test>]
let ``Album.from should succeed if given correct parameters`` () =
    Album.from "Infinite Granite" [ dummyRecordedSong ]
    |> Result.unwrap
    |> fun album ->
        album.Name |> should equal "Infinite Granite"
        album.TrackList |> should haveLength 1
        album.Type |> should be (ofCase <@ Single @>)

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
    [ 1400<second>; 134<second>; 1<second> ]
    |> List.iter
        (fun length ->
            Album.recordType [ dummyRecordedSong
                               (dummyRecordedSongWithLength length) ]
            |> Result.unwrap
            |> should be (ofCase <@ EP @>))

[<Test>]
let ``Album.recordType should return LP if given more than one song and its length is more than 25 minutes``
    ()
    =
    [ 1501<second>
      10034<second>
      1680<second> ]
    |> List.iter
        (fun length ->
            Album.recordType [ dummyRecordedSong
                               (dummyRecordedSongWithLength length) ]
            |> Result.unwrap
            |> should be (ofCase <@ LP @>))
