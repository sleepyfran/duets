module Entities.Calendar

open System
open Fugit.Months

/// Returns the day moment of the given date. Defaults to dawn if the time does
/// not have an equivalent.
let dayMomentOf (date: Date) =
    match date.Hour with
    | 6 -> Dawn
    | 10 -> Morning
    | 14 -> Midday
    | 18 -> Sunset
    | 20 -> Dusk
    | 22 -> Night
    | 0 -> Midnight
    | _ -> Dawn

/// Determines whether the given date is the first day of the year or not.
let isFirstMomentOfYear (date: Date) =
    date.Day = 1
    && date.Month = 1
    && dayMomentOf date = Dawn

/// Returns the given date with the hour set to the specified day moment.
let withDayMoment dayMoment (date: Date) =
    match dayMoment with
    | Dawn -> 6
    | Morning -> 10
    | Midday -> 14
    | Sunset -> 18
    | Dusk -> 20
    | Night -> 22
    | Midnight -> 0
    |> fun hour -> DateTime(date.Year, date.Month, date.Day, hour, 0, 0)

/// Returns the given date with the hour set to the specified day moment.
let withDayMoment' (date: Date) dayMoment = withDayMoment dayMoment date

/// Returns the date in which the game starts.
let gameBeginning = January 1 2015 |> withDayMoment Dawn
