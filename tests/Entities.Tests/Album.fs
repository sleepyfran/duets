module Entities.Tests.Album

open FSharp.Data.UnitSystems.SI.UnitNames
open FsUnit
open NUnit.Framework
open Test.Common

open Common
open Entities

[<Test>]
let ``Album.from should succeed if given correct parameters`` () =
    Album.from "Infinite Granite" [ dummyRecordedSong ]
    |> Result.unwrap
    |> fun album ->
        album.Name |> should equal "Infinite Granite"
        album.TrackList |> should haveLength 1
        album.Type |> should be (ofCase <@ Single @>)

[<Test>]
let ``Album.recordType should return single if only one song is given`` () =
    Album.recordType [ dummyRecordedSong ]
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
            |> should be (ofCase <@ EP @>))

[<Test>]
let ``Album.recordType should return LP if given more than one song and its length is more than 25 minutes``
    ()
    =
    [ 1501<second>; 10034<second>; 1680<second> ]
    |> List.iter
        (fun length ->
            Album.recordType [ dummyRecordedSong
                               (dummyRecordedSongWithLength length) ]
            |> should be (ofCase <@ LP @>))