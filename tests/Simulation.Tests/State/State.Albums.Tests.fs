module Duets.Simulation.Tests.State.Albums

open FsUnit
open NUnit.Framework
open Test.Common

open Aether
open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Queries

[<Test>]
let ``AlbumRecorded should add an unreleased album`` () =
    let state =
        SongFinished(dummyBand, dummyFinishedSong)
        |> State.Root.applyEffect dummyState

    Songs.finishedByBand state dummyBand.Id |> should haveCount 1

    let state =
        AlbumStarted(dummyBand, dummyUnreleasedAlbum)
        |> State.Root.applyEffect state

    Albums.unreleasedByBand state dummyBand.Id |> should haveCount 1

[<Test>]
let ``AlbumRecorded should set finished song as recorded`` () =
    let state =
        SongFinished(dummyBand, dummyFinishedSong)
        |> State.Root.applyEffect dummyState

    Songs.finishedByBand state dummyBand.Id |> should haveCount 1

    let state =
        AlbumStarted(dummyBand, dummyUnreleasedAlbum)
        |> State.Root.applyEffect state

    state.BandSongRepertoire.FinishedSongs
    |> Map.find dummyBand.Id
    |> List.ofMapValues
    |> List.head
    |> fun (FinishedWithRecordingStatus(_, recorded)) ->
        recorded |> should equal true

[<Test>]
let ``AlbumUpdated should set finished songs as recorded`` () =
    let state =
        SongFinished(dummyBand, dummyFinishedSong)
        |> State.Root.applyEffect dummyState

    Songs.finishedByBand state dummyBand.Id |> should haveCount 1

    let state =
        AlbumUpdated(dummyBand, dummyUnreleasedAlbum)
        |> State.Root.applyEffect state

    state.BandSongRepertoire.FinishedSongs
    |> Map.find dummyBand.Id
    |> List.ofMapValues
    |> List.head
    |> fun (FinishedWithRecordingStatus(_, recorded)) ->
        recorded |> should equal true

[<Test>]
let ``AlbumRenamed should replace album with same with different name`` () =
    let state =
        AlbumStarted(dummyBand, dummyUnreleasedAlbum)
        |> State.Root.applyEffect dummyState

    let state =
        AlbumUpdated(
            dummyBand,
            UnreleasedAlbum { dummyAlbum with Name = "Test." }
        )
        |> State.Root.applyEffect state

    Albums.unreleasedByBand state dummyBand.Id
    |> Map.head
    |> fun (UnreleasedAlbum album) -> album.Name |> should equal "Test."

[<Test>]
let ``AlbumReleased should remove unreleased album and add it as released`` () =
    let state =
        AlbumStarted(dummyBand, dummyUnreleasedAlbum)
        |> State.Root.applyEffect dummyState

    let state =
        AlbumReleased(dummyBand, dummyReleasedAlbum)
        |> State.Root.applyEffect state

    Albums.unreleasedByBand state dummyBand.Id |> should haveCount 0

    Albums.releasedByBand state dummyBand.Id |> should haveLength 1
