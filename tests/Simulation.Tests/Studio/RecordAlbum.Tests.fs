module Simulation.Tests.Studio.RecordAlbum

open FsUnit
open NUnit.Framework
open Test.Common

open Common
open Entities
open Simulation.Bank.Operations
open Simulation.Studio.RecordAlbum

[<Test>]
let ``recordAlbum should fail if the band does not have enough money`` () =
    Album.Unreleased.from "Simple Math" [ dummyRecordedSong ]
    |> recordAlbum dummyState dummyStudio dummyBand
    |> Result.unwrapError
    |> should be (ofCase <@ NotEnoughFunds(200m<dd>) @>)

let state =
    addFunds dummyBandBankAccount.Holder 40000m<dd> dummyState

[<Test>]
let ``recordAlbum should create album if parameters are correct`` () =
    Album.Unreleased.from "Black Brick" [ dummyRecordedSong ]
    |> recordAlbum state dummyStudio dummyBand
    |> Result.unwrap
    |> fun ((UnreleasedAlbum album), _) ->
        album.Name |> should equal "Black Brick"

        album.TrackList
        |> should equal [ dummyRecordedSong ]

        album.Type |> should be (ofCase <@ Single @>)

[<Test>]
let ``recordAlbum should add 20% of the producer's skill to each song in the track list``
    ()
    =
    let state =
        state
        |> addSkillTo
            dummyCharacter
            (Skill.createWithLevel SkillId.MusicProduction 75)

    Album.Unreleased.from "Infinite Granite" [ dummyRecordedSong ]
    |> recordAlbum state dummyStudio dummyBand
    |> Result.unwrap
    |> fun ((UnreleasedAlbum album), _) ->
        album.TrackList
        |> List.iter (fun (_, quality) -> quality |> should equal 65<quality>)

[<Test>]
let ``recordAlbum should not add producer's skill if quality is already 100``
    ()
    =
    let state =
        state
        |> addSkillTo
            dummyCharacter
            (Skill.createWithLevel SkillId.MusicProduction 100)

    let song =
        RecordedSong(FinishedSong dummySong, 100<quality>)

    Album.Unreleased.from "Infinite Granite" [ RecordedSong song ]
    |> recordAlbum state dummyStudio dummyBand
    |> Result.unwrap
    |> fun ((UnreleasedAlbum album), _) ->
        album.TrackList
        |> List.iter (fun (_, quality) -> quality |> should equal 100<quality>)

[<Test>]
let ``recordAlbum should generate AlbumRecorded and MoneyTransferred`` () =
    let albumTitle = "Black Brick"
    let albumTrackList = [ dummyRecordedSong ]

    let album =
        Album.from albumTitle albumTrackList

    Album.Unreleased.from albumTitle albumTrackList
    |> recordAlbum state dummyStudio dummyBand
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
                <@
                    MoneyTransferred(
                        dummyBandBankAccount.Holder,
                        Outgoing(200m<dd>, 200m<dd>)
                    )
                @>)
