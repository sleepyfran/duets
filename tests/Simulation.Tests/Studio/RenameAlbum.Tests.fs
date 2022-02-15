module Simulation.Tests.Studio.RenameAlbum

open FsUnit
open NUnit.Framework
open Test.Common

open Entities
open Simulation.Studio.RenameAlbum

[<Test>]
let ``renameAlbum should fail if name is empty`` () =
    renameAlbum dummyBand dummyUnreleasedAlbum ""
    |> should be (ofCase <@ Album.NameTooShort @>)

[<Test>]
let ``renameAlbum should fail if name is too long`` () =
    renameAlbum
        dummyBand
        dummyUnreleasedAlbum
        "Nothing to Go Home To, Nothing There to Come Home For, No Home to Return To,  Nothing to Go Home To, Nothing There to Come Home For, No Home to Return To"
    |> should be (ofCase <@ Album.NameTooLong @>)

[<Test>]
let ``renameAlbum should generate AlbumRenamed effect`` () =
    renameAlbum dummyBand dummyUnreleasedAlbum "Great Mass Of Color"
    |> fun effect ->
        match effect with
        | AlbumRenamed (_, UnreleasedAlbum album) ->
            album.Name |> should equal "Great Mass Of Color"
        | _ -> raise <| invalidOp "Not possible"
