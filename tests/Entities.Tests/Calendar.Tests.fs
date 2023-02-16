module Duets.Entities.Tests.Calendar

open FsUnit
open Fugit.Months
open NUnit.Framework

open Duets.Entities

let private baseDate =
    January 9 2023

(* ------ QUERY ------ *)

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
