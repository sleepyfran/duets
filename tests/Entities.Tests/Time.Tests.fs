module Duets.Entities.Tests.Time

open FSharp.Data.UnitSystems.SI.UnitNames
open FsUnit
open NUnit.Framework

open Duets.Common
open Duets.Entities

[<Test>]
let ``from should succeed if the given minutes and seconds are valid`` () =
    [ (4<minute>, 35<second>)
      (0<minute>, 0<second>)
      (10<minute>, 0<second>) ]
    |> List.iter
        (fun (minutes, seconds) ->
            Time.Length.from minutes seconds
            |> Result.unwrap
            |> should equal { Minutes = minutes; Seconds = seconds })

[<Test>]
let ``from should error if given less than 0 minutes`` () =
    Time.Length.from -1<minute> 0<second>
    |> Result.unwrapError
    |> should equal Time.Length.LessThan0Minutes

[<Test>]
let ``from should error if given less than 0 seconds`` () =
    Time.Length.from 3<minute> -1<second>
    |> Result.unwrapError
    |> should equal Time.Length.LessThan0Seconds

[<Test>]
let ``from should error if given more than 60 seconds`` () =
    Time.Length.from 3<minute> 61<second>
    |> Result.unwrapError
    |> should equal Time.Length.MoreThan60Seconds

[<Test>]
let ``parse should error if input does not contain :`` () =
    Time.Length.parse "12"
    |> Result.unwrapError
    |> should equal Time.Length.InvalidFormat

[<Test>]
let ``parse should error if input does not contain numbers`` () =
    Time.Length.parse "as:ff"
    |> Result.unwrapError
    |> should equal Time.Length.InvalidFormat
