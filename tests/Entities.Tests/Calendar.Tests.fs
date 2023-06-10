module Duets.Entities.Tests.Calendar

open FsUnit
open Fugit.Months
open NUnit.Framework

open Duets.Entities

let private baseDate = January 9 2023

(* ------ QUERY ------ *)

[<Test>]
let ``datesBetween should return all dates between two dates`` () =
    Calendar.Query.datesBetween (January 1 2023) (January 5 2023)
    |> should
        equal
        [ January 1 2023
          January 2 2023
          January 3 2023
          January 4 2023
          January 5 2023 ]

[<Test>]
let ``datesBetween handles next month correctly`` () =
    Calendar.Query.datesBetween (January 28 2023) (February 2 2023)
    |> should
        equal
        [ January 28 2023
          January 29 2023
          January 30 2023
          January 31 2023
          February 1 2023
          February 2 2023 ]

[<Test>]
let ``datesBetween can handle a lot of dates`` () =
    Calendar.Query.datesBetween (January 1 2023) (January 1 2024)
    |> should haveLength 366

[<Test>]
let ``next should return next day moment`` () =
    let next =
        January 9 2023
        |> Calendar.Transform.changeDayMoment EarlyMorning
        |> Calendar.Query.next

    next.Day |> should equal 9
    next.Month |> should equal 1
    next.Year |> should equal 2023
    next |> Calendar.Query.dayMomentOf |> should equal Morning

[<Test>]
let ``next should return next day when next day moment is midnight`` () =
    let next =
        January 9 2023
        |> Calendar.Transform.changeDayMoment Night
        |> Calendar.Query.next

    next.Day |> should equal 10
    next.Month |> should equal 1
    next.Year |> should equal 2023
    next |> Calendar.Query.dayMomentOf |> should equal Midnight
