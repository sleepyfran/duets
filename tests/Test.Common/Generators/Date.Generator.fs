module Test.Common.Generators.Date

open Duets.Entities
open FsCheck

let dateGenerator (fromDate: Date) (toDate: Date) =
    if fromDate = toDate then
        Gen.constant fromDate
    else
        [ 0 .. 1 + toDate.Subtract(fromDate).Days ]
        |> List.map (fun offset -> fromDate.AddDays(float offset))
        |> Gen.elements
