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
    |> ongoingConcertFromEffectList
    |> Optic.get Lenses.Concerts.Ongoing.points_
    |> should equal 5<quality>

    response
    |> pointsFromEffectList
    |> should equal 5<quality>

[<Test>]
let ``greetAudience results in Done when greeting for the first time`` () =
    greetAudience dummyState dummyOngoingConcert
    |> resultFromEffectList
    |> should be (ofCase <@ Done @>)

[<Test>]
let ``greetAudience takes 10 points when greeting after the first time`` () =
    let ongoingConcert =
        dummyOngoingConcert
        |> Optic.set Lenses.Concerts.Ongoing.points_ 10<quality>
        |> Optic.set Lenses.Concerts.Ongoing.events_ [ GreetAudience ]

    let response = greetAudience dummyState ongoingConcert

    response
    |> ongoingConcertFromEffectList
    |> Optic.get Lenses.Concerts.Ongoing.points_
    |> should equal 0<quality>

    response
    |> pointsFromEffectList
    |> should equal -10<quality>

[<Test>]
let ``greetAudience results in GreetedMoreThanOnce when greeting after the first time ``
    ()
    =
    greetAudience dummyState dummyOngoingConcert
    |> ongoingConcertFromEffectList
    |> greetAudience dummyState
    |> resultFromEffectList
    |> should be (ofCase <@ TooManyRepetitionsPenalized @>)
