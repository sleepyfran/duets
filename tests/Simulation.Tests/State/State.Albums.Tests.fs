module Simulation.State.Tests.Albums


open FsUnit
open NUnit.Framework
open Test.Common

open Aether
open Common
open Entities
open Simulation
open Simulation.Queries

[<Test>]
let ``AlbumRecorded should add an unreleased album and remove all track list from finished songs``
    ()
    =
    let state =
        SongFinished(dummyBand, dummyFinishedSong)
        |> State.Root.applyEffect dummyState

    Songs.finishedByBand state dummyBand.Id
    |> should haveCount 1

    let state =
        AlbumRecorded(dummyBand, dummyUnreleasedAlbum)
        |> State.Root.applyEffect state

    Albums.unreleasedByBand state dummyBand.Id
    |> should haveCount 1

    Songs.finishedByBand state dummyBand.Id
    |> should haveCount 0

[<Test>]
let ``AlbumRenamed should remove replace album with same with different name``
    ()
    =
    let state =
        AlbumRecorded(dummyBand, dummyUnreleasedAlbum)
        |> State.Root.applyEffect dummyState

    let state =
        AlbumRenamed(
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
        AlbumRecorded(dummyBand, dummyUnreleasedAlbum)
        |> State.Root.applyEffect dummyState

    let state =
        AlbumReleased(dummyBand, dummyReleasedAlbum)
        |> State.Root.applyEffect state

    Albums.unreleasedByBand state dummyBand.Id
    |> should haveCount 0

    Albums.releasedByBand state dummyBand.Id
    |> should haveLength 1
