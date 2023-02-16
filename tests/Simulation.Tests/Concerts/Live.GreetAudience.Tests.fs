module Duets.Simulation.Tests.Concerts.Live.GreetAudience

open FsUnit
open NUnit.Framework
open Test.Common

open Aether
open Duets.Entities
open Duets.Simulation.Concerts.Live

[<Test>]
let ``greetAudience gives 5 points when greeting for the first time`` () =
    let response =
        greetAudience dummyState dummyOngoingConcert

    response
    |> ongoingConcertFromResponse
    |> Optic.get Lenses.Concerts.Ongoing.points_
    |> should equal 5<quality>

    response
    |> pointsFromResponse
    |> should equal 5<quality>

[<Test>]
let ``greetAudience results in Done when greeting for the first time`` () =
    greetAudience dummyState dummyOngoingConcert
    |> resultFromResponse
    |> should be (ofCase <@ Done @>)

[<Test>]
let ``greetAudience takes 10 points when greeting after the first time`` () =
    let ongoingConcert =
        dummyOngoingConcert
        |> Optic.set Lenses.Concerts.Ongoing.points_ 10<quality>
        |> Optic.set Lenses.Concerts.Ongoing.events_ [ GreetAudience ]

    let response = greetAudience dummyState ongoingConcert

    response
    |> ongoingConcertFromResponse
    |> Optic.get Lenses.Concerts.Ongoing.points_
    |> should equal 0<quality>

    response
    |> pointsFromResponse
    |> should equal -10<quality>

[<Test>]
let ``greetAudience results in GreetedMoreThanOnce when greeting after the first time ``
    ()
    =
    greetAudience dummyState dummyOngoingConcert
    |> ongoingConcertFromResponse
    |> greetAudience dummyState
    |> resultFromResponse
    |> should be (ofCase <@ TooManyRepetitionsPenalized @>)
