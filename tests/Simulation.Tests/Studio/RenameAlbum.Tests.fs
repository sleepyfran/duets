module Duets.Simulation.Tests.Studio.RenameAlbum

open FsUnit
open NUnit.Framework
open Test.Common

open Duets.Common
open Duets.Entities
open Duets.Simulation.Studio.RenameAlbum

[<Test>]
let rec ``validateName should fail if name is empty`` () =
    Album.validateName ""
    |> Result.unwrapError
    |> should be (ofCase <@ Album.NameTooShort @>)

[<Test>]
let ``validateName should fail if name is too long`` () =
    Album.validateName
        "Nothing to Go Home To, Nothing There to Come Home For, No Home to Return To,  Nothing to Go Home To, Nothing There to Come Home For, No Home to Return To"
    |> Result.unwrapError
    |> should be (ofCase <@ Album.NameTooLong @>)

[<Test>]
let ``renameAlbum should generate AlbumRenamed effect`` () =
    renameAlbum dummyBand dummyUnreleasedAlbum "Great Mass Of Color"
    |> fun effect ->
        match effect with
        | AlbumUpdated(_, unreleasedAlbum) ->
            let album = unreleasedAlbum |> Album.fromUnreleased
            album.Name |> should equal "Great Mass Of Color"
        | _ -> raise <| invalidOp "Not possible"
