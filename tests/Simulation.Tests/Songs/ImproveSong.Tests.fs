module Simulation.Tests.Songs.ImproveSong

open Test.Common
open NUnit.Framework
open FsUnit

open Entities
open Simulation.Songs.Composition.ComposeSong
open Simulation.Songs.Composition.ImproveSong

[<SetUp>]
let Setup () =
  initStateWithDummies ()
  let character = currentCharacter ()
  addSkillTo character (Skill.createWithLevel SkillId.Composition 50)
  addSkillTo character (Skill.createWithLevel (Genre dummyBand.Genre) 50)
  composeSong dummySong

[<Test>]
let ShouldImproveIfPossibleAndReturnCanBeImproved () =
  let song = lastUnfinishedSong ()

  improveSong song
  |> should be (ofCase <@ CanBeImproved 14<quality> @>)

[<Test>]
let ShouldImproveForALastTimeIfPossibleAndReturnReachedMaxQualityInLastImprovement
  ()
  =
  improveLastUnfinishedSongTimes 3

  lastUnfinishedSong ()
  |> improveSong
  |> should be (ofCase <@ ReachedMaxQualityInLastImprovement 35<quality> @>)

[<Test>]
let ShouldNotAllowImprovementIfReachedMaxQualityAndReturnReachMaxQualityAlready
  ()
  =
  improveLastUnfinishedSongTimes 4

  lastUnfinishedSong ()
  |> improveSong
  |> should be (ofCase <@ ReachedMaxQualityAlready 35<quality> @>)
