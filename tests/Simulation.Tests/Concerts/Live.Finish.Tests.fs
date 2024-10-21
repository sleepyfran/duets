module Duets.Simulation.Tests.Concerts.Finish

open FsCheck
open FsUnit
open NUnit.Framework
open Test.Common
open Test.Common.Generators

open Aether
open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Concerts.Live.Finish
open Duets.Simulation.Time

let private attendance = 1000

let private calculateExpectedFanGain fans (modifier: float) =
    let totalFans = Queries.Bands.totalFans fans

    let fanChange =
        float attendance * modifier |> Math.ceilToNearest |> (*) 1<fans>

    totalFans + fanChange |> Math.lowerClamp 0<fans>

let private assertFanGain modifier state band concert =
    let (Diff(_, updatedFans)) =
        finishConcert state concert
        |> List.choose (fun effect ->
            match effect with
            | BandFansChanged(_, diff) -> Some diff
            | _ -> None)
        |> List.head

    let totalFans = Queries.Bands.totalFans updatedFans

    totalFans |> should equal (calculateExpectedFanGain band.Fans modifier)

let private simulateAndCheck'
    minConcertPoints
    maxConcertPoints
    participationType
    assertFn
    =
    State.generateN
        { State.defaultOptions with
            BandFansMin = 100<fans>
            BandFansMax = 1000<fans> }
        100
    |> List.iter (fun state ->
        let band = Queries.Bands.currentBand state

        let randomPoints =
            Gen.choose (minConcertPoints, maxConcertPoints)
            |> Gen.sample 1 1
            |> List.head

        let concertWithAttendance =
            { dummyConcert with
                TicketsSold = attendance
                ParticipationType = participationType }

        let concertWithPoints =
            { dummyOngoingConcert with
                Concert = concertWithAttendance
                Points = randomPoints * 1<quality> }

        assertFn state band concertWithPoints)

let private simulateAndCheck minConcertPoints maxConcertPoints assertFn =
    simulateAndCheck' minConcertPoints maxConcertPoints Headliner assertFn

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
let ``finishing an opening concert only applies 20% of the fan gain`` () =
    simulateAndCheck'
        86
        100
        (OpeningAct(dummyHeadlinerBand.Id, 50<percent>))
        (assertFanGain 0.1) (* 20% of 50% = 10% *)

[<Test>]
let ``finishing the concert should grant the band the earnings from the tickets sold``
    ()
    =
    simulateAndCheck 0 100 (fun state _ concert ->
        let moneyEarned =
            finishConcert state concert
            |> List.choose (fun effect ->
                match effect with
                | MoneyEarned(_, Incoming(amount, _)) -> Some amount
                | _ -> None)
            |> List.head

        let expectedTotal =
            (decimal concert.Concert.TicketsSold * concert.Concert.TicketPrice)
            * 0.73m (* Minus 27% from the venue cut. *)

        moneyEarned |> should equal expectedTotal)

[<Test>]
let ``finishing an opening act concert should grant the band the correct percentage of the tickets sold``
    ()
    =
    simulateAndCheck'
        0
        100
        (OpeningAct(dummyHeadlinerBand.Id, 50<percent>))
        (fun state _ concert ->
            let moneyEarned =
                finishConcert state concert
                |> List.choose (fun effect ->
                    match effect with
                    | MoneyEarned(_, Incoming(amount, _)) -> Some amount
                    | _ -> None)
                |> List.head

            let expectedEarnings =
                decimal concert.Concert.TicketsSold
                * concert.Concert.TicketPrice
                * 0.73m (* Minus 27% from the venue cut. *)
                * 0.5m (* Opening act earnings. *)

            moneyEarned |> should equal expectedEarnings)
