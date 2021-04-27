module Simulation.Tests.Songs.DiscardSong

open Test.Common
open NUnit.Framework
open FsUnit

open Simulation.Songs.Queries
open Simulation.Songs.Composition.ComposeSong
open Simulation.Songs.Composition.DiscardSong

[<SetUp>]
let Setup () =
  initStateWithDummies ()
  composeSong dummySong
  lastUnfinishedSong () |> discardSong |> ignore

[<Test>]
let DiscardSongShouldRemoveSongFromUnfinishedRepertoire () =
  unfinishedSongsByBand dummyBand.Id
  |> should haveCount 0
