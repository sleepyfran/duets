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
let ``dailyUpdate should sum new streams to the current count`` () =
    let streamsLenses =
        Lenses.FromState.Albums.releasedByBand_ dummyBand.Id
        >?> Map.key_ album.Album.Id
        >?> Lenses.Album.streams_

    let updatedState = Optic.set streamsLenses 1500 state

    dailyUpdate updatedState
    |> should
        contain
        (AlbumReleasedUpdate(dummyBand, Album.Released.update album 3000 0.9))

[<Test>]
let ``dailyUpdate should return list with money transfer if quantity is more than 0``
    ()
    =
    dailyUpdate state
    |> should
        contain
        (MoneyEarned(dummyBandBankAccount.Holder, Incoming(750<dd>, 750<dd>)))

[<Test>]
let ``dailyUpdate should return list without money transfer if quantity is 0``
    ()
    =
    let unknownAlbum =
        Album.Released.fromUnreleased dummyUnreleasedAlbum dummyToday 10 0.1

    let state =
        dummyState
        |> addReleasedAlbum dummyBand unknownAlbum

    let updateEffects = dailyUpdate state
    updateEffects |> should haveLength 1
