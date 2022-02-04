module Entities.Calendar

open Fugit.Months
open System

let allDayMoments =
    [ Midnight
      Dawn
      Morning
      Midday
      Sunset
      Dusk ]

[<AutoOpen>]
module Ops =
    /// Adds the given number of days to the date.
    let addDays n (date: Date) = date.AddDays(float n)

[<RequireQualifiedAccess>]
module Query =
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

    /// Returns the associated time in a day of the given day moment.
    let timeOfDayMoment dayMoment =
        match dayMoment with
        | Dawn -> 6
        | Morning -> 10
        | Midday -> 14
        | Sunset -> 18
        | Dusk -> 20
        | Night -> 22
        | Midnight -> 0

    /// Determines whether the given date is the first day of the year or not.
    let isFirstMomentOfYear (date: Date) =
        date.Day = 1
        && date.Month = 1
        && dayMomentOf date = Dawn

    /// Returns the first date of the next month of the given date.
    let firstDayOfNextMonth (date: Date) =
        let dateWithAddedMonth = date.AddMonths(1)
        DateTime(dateWithAddedMonth.Year, dateWithAddedMonth.Month, 1)

    /// Retrieves all dates from today until the end of the month.
    let monthDaysFrom (date: Date) =
        [ date.Day .. DateTime.DaysInMonth(date.Year, date.Month) ]
        |> Seq.map (fun day -> DateTime(date.Year, date.Month, day))

[<RequireQualifiedAccess>]
module Transform =
    /// Returns the given date with the hour set to the specified day moment.
    let changeDayMoment dayMoment (date: Date) =
        Query.timeOfDayMoment dayMoment
        |> fun hour -> DateTime(date.Year, date.Month, date.Day, hour, 0, 0)

    /// Returns the given date with the hour set to the specified day moment.
    let changeDayMoment' (date: Date) dayMoment = changeDayMoment dayMoment date

[<RequireQualifiedAccess>]
module Parse =
    /// Attempts to parse a given string into a date.
    let date (strDate: string) =
        let couldParse, parsedDate = DateTime.TryParse strDate

        if couldParse then
            Some parsedDate
        else
            None

    /// Attempts to parse a given string into a day moment. Returns dawn if
    /// no compatible day moment is given.
    let dayMoment (strDayMoment: string) =
        match strDayMoment with
        | "Dawn" -> Dawn
        | "Morning" -> Morning
        | "Midday" -> Midday
        | "Sunset" -> Sunset
        | "Dusk" -> Dusk
        | "Night" -> Night
        | "Midnight" -> Midnight
        | _ -> Dawn

/// Returns the date in which the game starts.
let gameBeginning =
    January 1 2015 |> Transform.changeDayMoment Dawn
