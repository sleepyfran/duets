module Simulation.Tests.Mutations.ImproveSong

open Test.Common
open NUnit.Framework
open FsUnit

open Entities.Song
open Entities.Skill
open Mediator.Mutations.Songs

let dummySong =
  { Name = "Test"
    Length = 100
    VocalStyle = "Instrumental" }

[<SetUp>]
let Setup () =
  initStateWithDummies ()
  let character = currentCharacter ()
  addSkillTo character (createSkillWithLevel SkillId.Composition 50)
  addSkillTo character (createSkillWithLevel (Genre dummyBand.Genre) 50)
  addSong dummySong

[<Test>]
let ShouldImproveIfPossibleAndReturnCanBeImproved () =
  let song = lastUnfinishedSong ()

  improveSong song
  |> should be (ofCase <@ CanBeImproved 14 @>)

[<Test>]
let ShouldImproveForALastTimeIfPossibleAndReturnReachedMaxQuality () =
  improveLastUnfinishedSongTimes 4

  lastUnfinishedSong ()
  |> improveSong
  |> should be (ofCase <@ ReachedMaxQuality 33 @>)
