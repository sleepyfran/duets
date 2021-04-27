module Simulation.Tests.Songs.FinishSong

open Test.Common
open NUnit.Framework
open FsUnit

open Simulation.Songs.Queries
open Simulation.Songs.Composition.ComposeSong
open Simulation.Songs.Composition.FinishSong

[<SetUp>]
let Setup () =
  initStateWithDummies ()
  composeSong dummySong
  lastUnfinishedSong () |> finishSong

[<Test>]
let FinishSongShouldRemoveSongFromUnfinishedRepertoire () =
  unfinishedSongsByBand dummyBand.Id
  |> should haveCount 0

[<Test>]
let FinishSongShouldAddSongAsFinishedToFinishedRepertoire () =
  finishedSongsByBand dummyBand.Id
  |> should haveCount 1
