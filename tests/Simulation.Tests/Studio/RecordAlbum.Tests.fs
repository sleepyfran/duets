module Simulation.Tests.Studio.RecordAlbum

open FsUnit
open NUnit.Framework
open Test.Common

open Common
open Entities
open Simulation.Studio.RecordAlbum

[<Test>]
let ``recordAlbum should fail if the band does not have enough money`` () =
    recordAlbum
        dummyState
        dummyStudio
        dummyBand
        "Simple Math"
        [ dummyRecordedSong ]
    |> Result.unwrapError
    |> should be (ofCase <@ NotEnoughMoney(0<dd>, 200<dd>) @>)

[<Test>]
let ``recordAlbum should fail if track list is empty`` () =
    recordAlbum dummyState dummyStudio dummyBand "test" []
    |> Result.unwrapError
    |> should be (ofCase <@ NoSongsSelected @>)

[<Test>]
let ``recordAlbum should fail if name is empty`` () =
    recordAlbum dummyState dummyStudio dummyBand "" [ dummyRecordedSong ]
    |> Result.unwrapError
    |> should be (ofCase <@ NameTooShort @>)

[<Test>]
let ``recordAlbum should fail if name is too long`` () =
    recordAlbum
        dummyState
        dummyStudio
        dummyBand
        "Nothing to Go Home To, Nothing There to Come Home For, No Home to Return To,  Nothing to Go Home To, Nothing There to Come Home For, No Home to Return To"
        [ dummyRecordedSong ]
    |> Result.unwrapError
    |> should be (ofCase <@ NameTooLong @>)

let state =
    addFunds dummyBandBankAccount.Holder 1000<dd> dummyState

[<Test>]
let ``recordAlbum should create album if parameters are correct`` () =
    recordAlbum state dummyStudio dummyBand "Black Brick" [ dummyRecordedSong ]
    |> Result.unwrap
    |> fun ((UnreleasedAlbum album), _) ->
        album.Name |> should equal "Black Brick"

        album.TrackList
        |> should equal [ dummyRecordedSong ]

        album.Type |> should be (ofCase <@ Single @>)

[<Test>]
let ``recordAlbum should generate AlbumRecorded and MoneyTransferred`` () =
    let albumTitle = "Black Brick"
    let albumTrackList = [ dummyRecordedSong ]

    let album =
        Album.from albumTitle albumTrackList
        |> Result.unwrap

    recordAlbum state dummyStudio dummyBand albumTitle albumTrackList
    |> Result.unwrap
    |> fun (_, effects) ->
        effects |> should haveLength 2

        List.head effects
        |> should
            be
            (ofCase <@ AlbumRecorded(dummyBand, UnreleasedAlbum album) @>)

        List.item 1 effects
        |> should
            be
            (ofCase
                <@ MoneyTransferred(
                    dummyBandBankAccount.Holder,
                    Outgoing 200<dd>
                ) @>)
