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

let private calculateExpected fans (modifier: float) =
    let fanChange = float attendance * modifier |> Math.ceilToNearest

    fans + fanChange |> Math.lowerClamp 0

let private simulateAndCheck minConcertPoints maxConcertPoints expectedFn =
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

        let (Diff (_, updatedFans)) =
            finishConcert state concertWithPoints
            |> List.choose (fun effect ->
                match effect with
                | BandFansChanged (_, diff) -> Some diff
                | _ -> None)
            |> List.head

        updatedFans |> should equal (expectedFn band))

[<Test>]
let ``finishing the concert with less than 40 points should decrease the band fans by 30% of the attendance``
    ()
    =
    simulateAndCheck 0 40 (fun band ->
        calculateExpected
            band.Fans
            Config.MusicSimulation.concertLowPointFanDecreaseRate)

[<Test>]
let ``finishing the concert with points between 41 and 65 increases the band fame by 0.15``
    ()
    =
    simulateAndCheck 41 65 (fun band ->
        calculateExpected
            band.Fans
            Config.MusicSimulation.concertMediumPointFanIncreaseRate)

[<Test>]
let ``finishing the concert with points between 66 and 85 increases the band fame by 25% of the attendance``
    ()
    =
    simulateAndCheck 66 85 (fun band ->
        calculateExpected
            band.Fans
            Config.MusicSimulation.concertGoodPointFanIncreaseRate)

[<Test>]
let ``finishing the concert with more than 85 points increases the band fame by 50% of the attendance``
    ()
    =
    simulateAndCheck 86 100 (fun band ->
        calculateExpected
            band.Fans
            Config.MusicSimulation.concertHighPointFanIncreaseRate)
