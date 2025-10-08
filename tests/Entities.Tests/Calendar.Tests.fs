module Duets.Entities.Tests.Calendar

open FsUnit
open NUnit.Framework

open Duets.Entities
open Duets.Entities.Calendar

let private baseDate = Shorthands.Winter 9<days> 2023<years>

(* ------ QUERY ------ *)

(* -- datesBetween -- *)
[<Test>]
let ``datesBetween should return all dates between two dates`` () =
    Calendar.Query.datesBetween
        (Shorthands.Winter 1<days> 2023<years>)
        (Shorthands.Winter 5<days> 2023<years>)
    |> should
        equal
        [ Shorthands.Winter 1<days> 2023<years>
          Shorthands.Winter 2<days> 2023<years>
          Shorthands.Winter 3<days> 2023<years>
          Shorthands.Winter 4<days> 2023<years>
          Shorthands.Winter 5<days> 2023<years> ]

[<Test>]
let ``datesBetween handles next season correctly`` () =
    Calendar.Query.datesBetween
        (Shorthands.Winter 20<days> 2023<years>)
        (Shorthands.Spring 2<days> 2024<years>)
    |> should
        equal
        [ Shorthands.Winter 20<days> 2023<years>
          Shorthands.Winter 21<days> 2023<years>
          Shorthands.Spring 1<days> 2024<years>
          Shorthands.Spring 2<days> 2024<years> ]

[<Test>]
let ``datesBetween handles negative ranges correctly`` () =
    Calendar.Query.datesBetween
        (Shorthands.Winter 2<days> 2023<years>)
        (Shorthands.Autumn 21<days> 2023<years>)
    |> List.ofSeq
    |> should
        equal
        [ Shorthands.Autumn 21<days> 2023<years>
          Shorthands.Winter 1<days> 2023<years>
          Shorthands.Winter 2<days> 2023<years> ]

[<Test>]
let ``datesBetween can handle a lot of dates`` () =
    Calendar.Query.datesBetween
        (Shorthands.Winter 1<days> 2023<years>)
        (Shorthands.Winter 1<days> 2024<years>)
    |> should haveLength 85

(* -- next -- *)

[<Test>]
let ``next should return next day moment`` () =
    let next =
        (Shorthands.Winter 9<days> 2023<years>
         |> Calendar.Transform.changeDayMoment EarlyMorning
         |> Calendar.Query.next)

    next.Day |> should equal 9<days>
    next.Season |> should equal Winter
    next.Year |> should equal 2023<years>
    next |> Calendar.Query.dayMomentOf |> should equal Morning

[<Test>]
let ``next should return next day when next day moment is midnight`` () =
    let next =
        (Shorthands.Winter 9<days> 2023<years>
         |> Calendar.Transform.changeDayMoment Night
         |> Calendar.Query.next)

    next.Day |> should equal 10<days>
    next.Season |> should equal Winter
    next.Year |> should equal 2023<years>
    next |> Calendar.Query.dayMomentOf |> should equal Midnight

(* -- dayMomentsBetween -- *)
[<Test>]
let ``dayMomentsBetween should return 0 when dates are the same`` () =
    Calendar.Query.dayMomentsBetween
        (Shorthands.Winter 9<days> 2023<years>)
        (Shorthands.Winter 9<days> 2023<years>)
    |> should equal 0<dayMoments>

[<Test>]
let ``dayMomentsBetween should return 0 when beginning is more than end`` () =
    Calendar.Query.dayMomentsBetween
        (Shorthands.Winter 9<days> 2023<years>)
        (Shorthands.Winter 8<days> 2023<years>)
    |> should equal 0<dayMoments>

[<Test>]
let ``dayMomentsBetween returns the correct amount of day moments`` () =
    let beginning =
        Shorthands.Winter 9<days> 2023<years>
        |> Calendar.Transform.changeDayMoment EarlyMorning

    let end' =
        Shorthands.Winter 9<days> 2023<years>
        |> Calendar.Transform.changeDayMoment Night

    Calendar.Query.dayMomentsBetween beginning end'
    |> should equal 5<dayMoments>

[<Test>]
let ``dayMomentsBetween handles dates that are days apart`` () =
    let beginning =
        Shorthands.Winter 9<days> 2023<years>
        |> Calendar.Transform.changeDayMoment EarlyMorning

    let end' =
        Shorthands.Winter 11<days> 2023<years>
        |> Calendar.Transform.changeDayMoment EarlyMorning

    Calendar.Query.dayMomentsBetween beginning end'
    |> should equal 14<dayMoments>

[<Test>]
let ``dayMomentsBetween handles midnight gracefully`` () =
    let beginning =
        Shorthands.Winter 9<days> 2023<years>
        |> Calendar.Transform.changeDayMoment Midnight

    let end' =
        Shorthands.Winter 9<days> 2023<years>
        |> Calendar.Transform.changeDayMoment Night

    Calendar.Query.dayMomentsBetween beginning end'
    |> should equal 6<dayMoments>

(* -- dayMomentsUntil -- *)
[<Test>]
let ``dayMomentsUntil correctly counts day moments`` () =
    let currentDate =
        Shorthands.Spring 1<days> 2025<years>
        |> Calendar.Transform.changeDayMoment Midday

    Calendar.Query.dayMomentsUntil Afternoon currentDate
    |> should equal 1<dayMoments>

[<Test>]
let ``dayMomentsUntil correctly counts over midnight`` () =
    let currentDate =
        Shorthands.Spring 1<days> 2025<years>
        |> Calendar.Transform.changeDayMoment Night

    Calendar.Query.dayMomentsUntil Midnight currentDate
    |> should equal 1<dayMoments>

[<Test>]
let ``dayMomentsUntil handles day moments that have passed`` () =
    let currentDate =
        Shorthands.Spring 1<days> 2025<years>
        |> Calendar.Transform.changeDayMoment Midday

    Calendar.Query.dayMomentsUntil EarlyMorning currentDate
    |> should equal 5<dayMoments>
