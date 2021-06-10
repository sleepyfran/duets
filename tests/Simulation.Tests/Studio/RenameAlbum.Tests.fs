module Simulation.Tests.Studio.RenameAlbum

open FsUnit
open NUnit.Framework
open Test.Common

open Common
open Entities
open Simulation.Studio.RenameAlbum

[<Test>]
let ``renameAlbum should fail if name is empty`` () =
    renameAlbum dummyBand dummyUnreleasedAlbum ""
    |> Result.unwrapError
    |> should be (ofCase <@ Album.NameTooShort @>)

[<Test>]
let ``renameAlbum should fail if name is too long`` () =
    renameAlbum
        dummyBand
        dummyUnreleasedAlbum
        "Nothing to Go Home To, Nothing There to Come Home For, No Home to Return To,  Nothing to Go Home To, Nothing There to Come Home For, No Home to Return To"
    |> Result.unwrapError
    |> should be (ofCase <@ Album.NameTooLong @>)

[<Test>]
let ``renameAlbum should generate AlbumRenamed effect`` () =
    renameAlbum dummyBand dummyUnreleasedAlbum "Great Mass Of Color"
    |> Result.unwrap
    |> fun (album, effect) ->
        effect
        |> should be (ofCase <@ AlbumRenamed(dummyBand, album) @>)
