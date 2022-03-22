module Simulation.Tests.Concerts.Live.GreetAudience

open FsUnit
open NUnit.Framework
open Test.Common

open Aether
open Entities
open Simulation.Concerts.Live

[<Test>]
let ``greetAudience gives 5 points when greeting for the first time`` () =
    let response = greetAudience dummyOngoingConcert

    response
    |> ongoingConcertFromResponse
    |> Optic.get Lenses.Concerts.Ongoing.points_
    |> should equal 5<quality>

    response
    |> pointsFromResponse
    |> should equal 5<quality>

[<Test>]
let ``greetAudience results in Ok when greeting for the first time`` () =
    greetAudience dummyOngoingConcert
    |> resultFromResponse
    |> should be (ofCase <@ Ok @>)

[<Test>]
let ``greetAudience takes 10 points when greeting after the first time`` () =
    let ongoingConcert =
        dummyOngoingConcert
        |> Optic.set Lenses.Concerts.Ongoing.points_ 10<quality>
        |> Optic.set
            Lenses.Concerts.Ongoing.events_
            [ CommonEvent GreetAudience ]

    let response = greetAudience ongoingConcert

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
    let ongoingConcert =
        dummyOngoingConcert
        |> Optic.set
            Lenses.Concerts.Ongoing.events_
            [ CommonEvent GreetAudience ]

    greetAudience ongoingConcert
    |> resultFromResponse
    |> should be (ofCase <@ GreetedMoreThanOnce @>)
