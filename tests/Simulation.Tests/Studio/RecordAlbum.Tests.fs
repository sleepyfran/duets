module Duets.Simulation.Tests.Studio.RecordAlbum

open FsUnit
open NUnit.Framework
open Test.Common

open Duets.Common
open Duets.Entities
open Duets.Simulation.Bank.Operations
open Duets.Simulation.Studio.RecordAlbum

[<Test>]
let ``startAlbum should fail if the band does not have enough money`` () =
    startAlbum
        dummyState
        dummyStudio
        SelectedProducer.StudioProducer
        dummyBand
        "Simple Math"
        dummyFinishedSong
    |> Result.unwrapError
    |> should be (ofCase <@ NotEnoughFunds(200m<dd>) @>)

let state = addFunds dummyBandBankAccount.Holder 40000m<dd> dummyState

[<Test>]
let ``startAlbum should cost the band the assigned price per song if selected producer is the playable character``
    ()
    =
    startAlbum
        state
        dummyStudio
        SelectedProducer.PlayableCharacter
        dummyBand
        "Simple Math"
        dummyFinishedSong
    |> Result.unwrap
    |> List.filter (fun eff ->
        match eff with
        | MoneyTransferred _ -> true
        | _ -> false)
    |> List.head
    |> should
        be
        (ofCase
            <@
                MoneyTransferred(
                    dummyBandBankAccount.Holder,
                    Outgoing(200m<dd>, 200m<dd>)
                )
            @>)

[<Test>]
let ``startAlbum should cost the band double the assigned price per song if selected producer is studio producer``
    ()
    =
    startAlbum
        state
        dummyStudio
        SelectedProducer.StudioProducer
        dummyBand
        "Simple Math"
        dummyFinishedSong
    |> Result.unwrap
    |> List.filter (fun eff ->
        match eff with
        | MoneyTransferred _ -> true
        | _ -> false)
    |> List.head
    |> should
        be
        (ofCase
            <@
                MoneyTransferred(
                    dummyBandBankAccount.Holder,
                    Outgoing(400m<dd>, 400m<dd>)
                )
            @>)

[<Test>]
let ``startAlbum should add 20% of the producer's skill to each song in the track list``
    ()
    =
    let state =
        state
        |> addSkillTo
            dummyCharacter2
            (Skill.createWithLevel SkillId.MusicProduction 75)

    let unreleasedAlbum =
        startAlbum
            state
            { dummyStudio with
                Producer = dummyCharacter2 }
            SelectedProducer.StudioProducer
            dummyBand
            "Infinite Granite"
            dummyFinishedSong
        |> Result.unwrap
        |> List.choose (fun eff ->
            match eff with
            | AlbumStarted(_, unreleasedAlbum) -> Some unreleasedAlbum
            | _ -> None)
        |> List.head

    unreleasedAlbum.Album.TrackList
    |> List.iter (fun (Recorded(_, quality)) ->
        quality |> should equal 65<quality>)

[<Test>]
let ``startAlbum should add 20% of the character's production skill to each song in the track list``
    ()
    =
    let state =
        state
        |> addSkillTo
            dummyCharacter
            (Skill.createWithLevel SkillId.MusicProduction 35)

    let unreleasedAlbum =
        startAlbum
            state
            { dummyStudio with
                Producer = dummyCharacter2 }
            SelectedProducer.PlayableCharacter
            dummyBand
            "Infinite Granite"
            dummyFinishedSong
        |> Result.unwrap
        |> List.choose (fun eff ->
            match eff with
            | AlbumStarted(_, unreleasedAlbum) -> Some unreleasedAlbum
            | _ -> None)
        |> List.head

    unreleasedAlbum.Album.TrackList
    |> List.iter (fun (Recorded(_, quality)) ->
        quality |> should equal 57<quality>)

[<Test>]
let ``startAlbum should not add producer's skill if quality is already 100``
    ()
    =
    let state =
        state
        |> addSkillTo
            dummyCharacter
            (Skill.createWithLevel SkillId.MusicProduction 100)

    let song = Finished(dummySong, 100<quality>)

    let unreleasedAlbum =
        startAlbum
            state
            dummyStudio
            SelectedProducer.StudioProducer
            dummyBand
            "Infinite Granite"
            song
        |> Result.unwrap
        |> List.choose (fun eff ->
            match eff with
            | AlbumStarted(_, unreleasedAlbum) -> Some unreleasedAlbum
            | _ -> None)
        |> List.head

    unreleasedAlbum.Album.TrackList
    |> List.iter (fun (Recorded(_, quality)) ->
        quality |> should equal 100<quality>)

[<Test>]
let ``startAlbum should generate AlbumRecorded and MoneyTransferred`` () =
    let albumTitle = "Black Brick"

    startAlbum
        state
        dummyStudio
        SelectedProducer.StudioProducer
        dummyBand
        albumTitle
        dummyFinishedSong
    |> Result.unwrap
    |> fun effects ->
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
