module Core.Tests

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
let Setup () = initStateWithDummies ()

[<Test>]
let ShouldAddSongToState () =
  addSong dummySong

  currentBand ()
  |> unfinishedSongs
  |> should haveLength 1

[<Test>]
let ShouldHaveAssignedProperties () =
  addSong dummySong

  currentBand ()
  |> unfinishedSongs
  |> List.head
  |> fun ((UnfinishedSong (s)), _, _) -> (s.Name, s.Length, s.VocalStyle)
  |> should equal (dummySong.Name, dummySong.Length, VocalStyle.Instrumental)

[<Test>]
let QualitiesShouldDefaultTo5MaxAndCurrent1IfLevelIs0 () =
  addSong dummySong

  currentBand ()
  |> unfinishedSongs
  |> List.head
  |> fun (_, (MaxQuality (mq)), (Quality (q))) -> (mq, q)
  |> should equal (5, 1)

[<Test>]
let QualitiesShouldBeCalculatedBasedOnBandMemberSkills () =
  let character = currentCharacter ()
  addSkillTo character (createSkillWithLevel SkillId.Composition 50)
  addSkillTo character (createSkillWithLevel (Genre dummyBand.Genre) 50)
  addSong dummySong

  currentBand ()
  |> unfinishedSongs
  |> List.head
  |> fun (_, (MaxQuality (mq)), (Quality (q))) -> (mq, q)
  |> should equal (33, 7)
