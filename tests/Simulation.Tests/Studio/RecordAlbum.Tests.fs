module Duets.Simulation.Tests.Studio.RecordAlbum

open FsUnit
open NUnit.Framework
open Test.Common

open Duets.Common
open Duets.Entities
open Duets.Simulation.Bank.Operations
open Duets.Simulation.Studio.RecordAlbum

[<Test>]
let ``recordAlbum should fail if the band does not have enough money`` () =
    startAlbum dummyState dummyStudio dummyBand "Simple Math" dummyRecordedSong
    |> Result.unwrapError
    |> should be (ofCase <@ NotEnoughFunds(200m<dd>) @>)

let state = addFunds dummyBandBankAccount.Holder 40000m<dd> dummyState

[<Test>]
let ``recordAlbum should add 20% of the producer's skill to each song in the track list``
    ()
    =
    let state =
        state
        |> addSkillTo
            dummyCharacter
            (Skill.createWithLevel SkillId.MusicProduction 75)

    let (UnreleasedAlbum album) =
        startAlbum
            state
            dummyStudio
            dummyBand
            "Infinite Granite"
            dummyRecordedSong
        |> Result.unwrap
        |> List.choose (fun eff ->
            match eff with
            | AlbumStarted (_, unreleasedAlbum) -> Some unreleasedAlbum
            | _ -> None)
        |> List.head

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

    let song = RecordedSong(FinishedSong dummySong, 100<quality>)

    let (UnreleasedAlbum album) =
        startAlbum state dummyStudio dummyBand "Infinite Granite" song
        |> Result.unwrap
        |> List.choose (fun eff ->
            match eff with
            | AlbumStarted (_, unreleasedAlbum) -> Some unreleasedAlbum
            | _ -> None)
        |> List.head

    album.TrackList
    |> List.iter (fun (_, quality) -> quality |> should equal 100<quality>)

[<Test>]
let ``recordAlbum should generate AlbumRecorded and MoneyTransferred`` () =
    let albumTitle = "Black Brick"

    startAlbum state dummyStudio dummyBand albumTitle dummyRecordedSong
    |> Result.unwrap
    |> fun effects ->
        effects |> should haveLength 4

        effects |> List.item 0 |> should be (ofCase <@ AlbumStarted @>)

        effects
        |> List.item 1
        |> should
            be
            (ofCase
                <@
                    MoneyTransferred(
                        dummyBandBankAccount.Holder,
                        Outgoing(200m<dd>, 200m<dd>)
                    )
                @>)
