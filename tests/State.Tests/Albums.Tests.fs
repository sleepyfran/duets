module State.Tests.Albums

open FsUnit
open NUnit.Framework
open Test.Common

open Aether
open Common
open Entities
open Simulation.Queries

[<SetUp>]
let Setup () = initState ()

[<Test>]
let ``AlbumRecorded should add an unreleased album and remove all track list from finished songs``
    ()
    =
    State.Root.apply
    <| SongFinished(dummyBand, dummyFinishedSong)

    Songs.finishedByBand (State.Root.get ()) dummyBand.Id
    |> should haveCount 1

    State.Root.apply
    <| AlbumRecorded(dummyBand, dummyUnreleasedAlbum)

    Albums.unreleasedByBand (State.Root.get ()) dummyBand.Id
    |> should haveCount 1

    Songs.finishedByBand (State.Root.get ()) dummyBand.Id
    |> should haveCount 0

[<Test>]
let ``AlbumRenamed should remove replace album with same with different name``
    ()
    =
    State.Root.apply
    <| AlbumRecorded(dummyBand, dummyUnreleasedAlbum)

    State.Root.apply
    <| AlbumRenamed(
        dummyBand,
        UnreleasedAlbum { dummyAlbum with Name = "Test." }
    )

    Albums.unreleasedByBand (State.Root.get ()) dummyBand.Id
    |> Map.head
    |> fun (UnreleasedAlbum album) -> album.Name |> should equal "Test."

[<Test>]
let ``AlbumReleased should remove unreleased album and add it as released`` () =
    State.Root.apply
    <| AlbumRecorded(dummyBand, dummyUnreleasedAlbum)

    State.Root.apply
    <| AlbumReleased(dummyBand, dummyReleasedAlbum)

    Albums.unreleasedByBand (State.Root.get ()) dummyBand.Id
    |> should haveCount 0

    Albums.releasedByBand (State.Root.get ()) dummyBand.Id
    |> should haveLength 1
