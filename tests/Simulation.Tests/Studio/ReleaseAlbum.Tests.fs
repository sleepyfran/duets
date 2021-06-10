module Simulation.Tests.Studio.ReleaseAlbum

open FsUnit
open NUnit.Framework
open Test.Common

open Entities
open Simulation.Studio.ReleaseAlbum

[<Test>]
let ``releaseAlbum should generate AlbumReleased effect`` () =
    releaseAlbum dummyState dummyBand dummyUnreleasedAlbum
    |> should be (ofCase <@ AlbumReleased(dummyBand, dummyReleasedAlbum) @>)
