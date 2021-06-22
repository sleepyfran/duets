module Simulation.Tests.Albums.DailyUpdate

open Aether
open Aether.Operators
open FsUnit
open NUnit.Framework
open Test.Common

open Entities
open Simulation.Albums.DailyUpdate

let album =
    Album.Released.fromUnreleased dummyUnreleasedAlbum dummyToday 1500 1.0

let state =
    dummyState |> addReleasedAlbum dummyBand album

[<Test>]
let ``dailyUpdate should return empty list if no albums released`` () =
    dailyUpdate dummyState |> should haveLength 0

[<Test>]
let ``dailyUpdate should update streams and hype`` () =
    dailyUpdate state
    |> should
        contain
        (AlbumReleasedUpdate(dummyBand, Album.Released.update album 1500 0.9))

[<Test>]
let ``dailyUpdate should modify streams on top of the previous ones`` () =
    let streamsLenses =
        Lenses.FromState.Albums.releasedByBand_ dummyBand.Id
        >?> Map.key_ album.Album.Id
        >?> Lenses.Album.streams_

    let updatedState = Optic.set streamsLenses 1500 state

    dailyUpdate updatedState
    |> should
        contain
        (AlbumReleasedUpdate(dummyBand, Album.Released.update album 3000 0.9))
