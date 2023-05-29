module Duets.Simulation.Tests.State.SongComposition



open FsUnit
open NUnit.Framework
open Test.Common

open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Queries

let unfinishedSong = Unfinished(dummySong, 35<quality>, 7<quality>)

let startSong () =
    SongStarted(dummyBand, unfinishedSong) |> State.Root.applyEffect dummyState

[<Test>]
let SongStartedShouldAddUnfinishedSong () =
    startSong () |> lastUnfinishedSong dummyBand |> should equal unfinishedSong

[<Test>]
let SongImprovedShouldReplaceUnfinishedSong () =
    let state = startSong ()

    let improvedSong = Unfinished(dummySong, 35<quality>, 14<quality>)

    SongImproved(dummyBand, Diff(unfinishedSong, improvedSong))
    |> State.Root.applyEffect state
    |> lastUnfinishedSong dummyBand
    |> should equal improvedSong

[<Test>]
let SongDiscardedShouldRemoveUnfinishedSong () =
    let state = startSong ()

    let state =
        SongDiscarded(dummyBand, unfinishedSong) |> State.Root.applyEffect state

    Songs.unfinishedByBand state dummyBand.Id |> should haveCount 0

[<Test>]
let SongFinishedShouldRemoveUnfinishedAndAddFinishedSong () =
    let state = startSong ()

    let finishedSong = Finished(dummySong, 14<quality>)

    let state =
        SongFinished(dummyBand, finishedSong) |> State.Root.applyEffect state

    Songs.unfinishedByBand state dummyBand.Id |> should haveCount 0

    state
    |> lastFinishedSong dummyBand
    |> should equal (FinishedWithRecordingStatus(finishedSong, false))
