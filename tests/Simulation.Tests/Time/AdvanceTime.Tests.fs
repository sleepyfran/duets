module Duets.Simulation.Tests.Time.AdvanceTime_Tests

open FsUnit
open NUnit.Framework

open Duets.Entities
open Duets.Simulation.Time.AdvanceTime

let private unwrapTime =
    function
    | TimeAdvanced time -> time
    | _ -> failwith "Unexpected effect"

let dateMatches (expected: Date) time =
    let actual = unwrapTime time
    expected.Year |> should equal actual.Year
    expected.Season |> should equal actual.Season
    expected.Day |> should equal actual.Day
    TimeAdvanced actual

let dayMomentMatches (dayMoment: DayMoment) time =
    let date = unwrapTime time
    date.DayMoment |> should equal dayMoment

[<Test>]
let ``advanceDayMoment should return next day moment`` () =
    let effects = advanceDayMoment Calendar.gameBeginning 1<dayMoments>

    List.head effects
    |> dateMatches Calendar.gameBeginning
    |> dayMomentMatches Morning

[<Test>]
let ``advanceDayMoment should roll over next day if current day moment is midnight``
    ()
    =
    let initialDate =
        Calendar.gameBeginning |> Calendar.Transform.changeDayMoment Night

    let effects = advanceDayMoment initialDate 1<dayMoments>

    List.head effects
    |> dateMatches (Calendar.gameBeginning |> Calendar.Ops.addDays 1<days>)
    |> dayMomentMatches Midnight

[<Test>]
let ``advanceDayMoment 2 should return 2 day moments later`` () =
    let effects = advanceDayMoment Calendar.gameBeginning 2<dayMoments>

    effects
    |> List.item 1
    |> dateMatches Calendar.gameBeginning
    |> dayMomentMatches Midday

[<Test>]
let ``advanceDayMoment should reset turn minutes`` () =
    let effects = advanceDayMoment Calendar.gameBeginning 1<dayMoments>

    effects |> should contain (TurnTimeUpdated 0<minute>)
