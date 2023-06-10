module Test.Common.Generators.Date

open Duets.Entities
open FsCheck

let dateGenerator (fromDate: Date) (toDate: Date) =
    if fromDate = toDate then
        Gen.constant fromDate
    else
        Calendar.Query.datesBetween fromDate toDate |> Gen.elements
