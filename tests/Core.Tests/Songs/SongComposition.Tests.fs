module Core.Tests

open Test.Common
open NUnit.Framework
open FsUnit

open Entities.Song
open Mediator.Mutation
open Mediator.Mutations.Songs

let dummySong =
  { Name = "Test"
    Length = 100
    VocalStyle = "Instrumental" }

[<SetUp>]
let Setup () =
  initStateWithDummies ()
  ComposeSongMutation dummySong |> mutate |> ignore

[<Test>]
let ShouldAddSongToState () =
  currentBand ()
  |> unfinishedSongs
  |> should haveLength 1

[<Test>]
let ShouldHaveAssignedProperties () =
  currentBand ()
  |> unfinishedSongs
  |> List.head
  |> fun ((UnfinishedSong (s)), _, _) -> (s.Name, s.Length, s.VocalStyle)
  |> should equal (dummySong.Name, dummySong.Length, VocalStyle.Instrumental)

[<Test>]
let ShouldComputeCorrectMaxQualityAndCurrentQuality () =
  currentBand ()
  |> unfinishedSongs
  |> List.head
  |> fun (_, (MaxQuality (mq)), (Quality (q))) -> (mq, q)
  |> should equal (0, 0)
