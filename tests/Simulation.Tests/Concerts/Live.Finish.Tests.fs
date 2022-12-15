module Simulation.Tests.Concerts.Finish

open FsCheck
open FsUnit
open NUnit.Framework
open Test.Common
open Test.Common.Generators

open Aether
open Common
open Entities
open Simulation.Concerts.Live.Finish
open Simulation

let private attendance = 1000

let private calculateExpectedFanGain fans (modifier: float) =
    let fanChange = float attendance * modifier |> Math.ceilToNearest

    fans + fanChange |> Math.lowerClamp 0

let private assertFanGain modifier state band concert =
    let (Diff (_, updatedFans)) =
        finishConcert state concert
        |> List.choose (fun effect ->
            match effect with
            | BandFansChanged (_, diff) -> Some diff
            | _ -> None)
        |> List.head

    updatedFans |> should equal (calculateExpectedFanGain band.Fans modifier)

let private simulateAndCheck minConcertPoints maxConcertPoints assertFn =
    State.generateN
        { State.defaultOptions with
            BandFansMin = 100
            BandFansMax = 1000 }
        100
    |> List.iter (fun state ->
        let band = Queries.Bands.currentBand state

        let randomPoints =
            Gen.choose (minConcertPoints, maxConcertPoints)
            |> Gen.sample 1 1
            |> List.head

        let concertWithAttendance =
            { dummyConcert with TicketsSold = attendance }

        let concertWithPoints =
            { dummyOngoingConcert with
                Concert = concertWithAttendance
                Points = randomPoints * 1<quality> }

        assertFn state band concertWithPoints)

[<Test>]
let ``finishing the concert with less than 40 points should decrease the band fans by 30% of the attendance``
    ()
    =
    simulateAndCheck
        0
        40
        (assertFanGain Config.MusicSimulation.concertLowPointFanDecreaseRate)

[<Test>]
let ``finishing the concert with points between 41 and 65 increases the band fame by 0.15``
    ()
    =
    simulateAndCheck
        41
        65
        (assertFanGain Config.MusicSimulation.concertMediumPointFanIncreaseRate)

[<Test>]
let ``finishing the concert with points between 66 and 85 increases the band fame by 25% of the attendance``
    ()
    =
    simulateAndCheck
        66
        85
        (assertFanGain Config.MusicSimulation.concertGoodPointFanIncreaseRate)

[<Test>]
let ``finishing the concert with more than 85 points increases the band fame by 50% of the attendance``
    ()
    =
    simulateAndCheck
        86
        100
        (assertFanGain Config.MusicSimulation.concertHighPointFanIncreaseRate)


[<Test>]
let ``finishing the concert should grant the band the earnings from the tickets sold``
    ()
    =
    simulateAndCheck 0 100 (fun state _ concert ->
        let moneyEarned =
            finishConcert state concert
            |> List.choose (fun effect ->
                match effect with
                | MoneyEarned (_, Incoming (amount, _)) -> Some amount
                | _ -> None)
            |> List.head

        moneyEarned
        |> should
            equal
            (decimal concert.Concert.TicketsSold * concert.Concert.TicketPrice))
