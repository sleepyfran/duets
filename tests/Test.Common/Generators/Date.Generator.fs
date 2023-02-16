module Test.Common.Generators.Date

open System
open Duets.Entities
open FsCheck

let dateGenerator (fromDate: Date) (toDate: Date) =
    gen {
        let! day = Gen.choose (fromDate.Day, toDate.Day)
        let! month = Gen.choose (fromDate.Month, toDate.Month)
        let! year = Gen.choose (fromDate.Year, toDate.Year)

        return DateTime(year, month, day)
    }
