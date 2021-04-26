module Simulation.Tests.ComposeSong

open Test.Common
open NUnit.Framework
open FsUnit

open Entities
open Simulation.Songs.Composition.ComposeSong

[<SetUp>]
let Setup () = initStateWithDummies ()

[<Test>]
let ShouldAddSongToState () =
  composeSong dummySong

  currentBand ()
  |> unfinishedSongs
  |> should haveCount 1

[<Test>]
let ShouldHaveAssignedProperties () =
  composeSong dummySong

  lastUnfinishedSong ()
  |> fun ((UnfinishedSong (s)), _, _) -> (s.Name, s.Length, s.VocalStyle)
  |> should equal (dummySong.Name, dummySong.Length, VocalStyle.Instrumental)

[<Test>]
let QualitiesShouldDefaultTo5MaxAndCurrent1IfLevelIs0 () =
  composeSong dummySong

  lastUnfinishedSong ()
  |> fun (_, mq, q) -> (mq, q)
  |> should equal (5, 1)

[<Test>]
let QualitiesShouldBeCalculatedBasedOnBandMemberSkills () =
  let character = currentCharacter ()
  addSkillTo character (Skill.createWithLevel SkillId.Composition 50)
  addSkillTo character (Skill.createWithLevel (Genre dummyBand.Genre) 50)
  composeSong dummySong

  lastUnfinishedSong ()
  |> fun (_, mq, q) -> (mq, q)
  |> should equal (33, 7)
