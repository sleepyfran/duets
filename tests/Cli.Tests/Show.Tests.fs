module Duets.Cli.Tests.Show

open FsUnit
open NUnit.Framework
open Test.Common

open Duets.Cli.Components.Scene.Room
open Duets.Entities

let private withSongs songs points =
    { dummyOngoingConcert with
        Events = List.replicate songs (PlaySong(dummySong, Energetic))
        Points = points * 1<quality> }

[<Test>]
let ``moodOf is warm before the first song`` () =
    Show.moodOf dummyOngoingConcert |> should equal CrowdMood.Warm

[<Test>]
let ``moodOf depends on the points per song`` () =
    Show.moodOf (withSongs 4 4) |> should equal CrowdMood.Bored
    Show.moodOf (withSongs 4 16) |> should equal CrowdMood.Warm
    Show.moodOf (withSongs 4 32) |> should equal CrowdMood.Hyped
    Show.moodOf (withSongs 2 30) |> should equal CrowdMood.Ecstatic

[<Test>]
let ``moodOf keeps the crowd up during a great concert`` () =
    Show.moodOf (withSongs 20 80) |> should equal CrowdMood.Ecstatic

[<Test>]
let ``fromConcert caps the attendance to the capacity`` () =
    let concert =
        { dummyOngoingConcert with
            Concert =
                { dummyOngoingConcert.Concert with
                    TicketsSold = 500 } }

    (Show.fromConcert 100 concert).Attendance |> should equal 1.0
    (Show.fromConcert 1000 concert).Attendance |> should equal 0.5
