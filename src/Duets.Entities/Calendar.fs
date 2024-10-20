module rec Duets.Entities.Calendar

open FSharp.Data.UnitSystems.SI.UnitNames
open Fugit.Shorthand
open Duets.Common
open System
open System.Globalization

let allDayMoments =
    [ Midnight; EarlyMorning; Morning; Midday; Afternoon; Evening; Night ]

let weekday =
    [ DayOfWeek.Monday
      DayOfWeek.Tuesday
      DayOfWeek.Wednesday
      DayOfWeek.Thursday
      DayOfWeek.Wednesday
      DayOfWeek.Friday ]

let everyDay =
    [ DayOfWeek.Monday
      DayOfWeek.Tuesday
      DayOfWeek.Wednesday
      DayOfWeek.Thursday
      DayOfWeek.Wednesday
      DayOfWeek.Friday
      DayOfWeek.Saturday
      DayOfWeek.Sunday ]

module DayMoments =
    /// Contains all the possible day moments in a week.
    let oneWeek = Calendar.allDayMoments |> List.length |> (*) 7<dayMoments>

    /// Transforms the given number of day moments into minutes.
    let toMinutes dayMoments =
        dayMoments / 1<dayMoments> * 180<minute>

[<RequireQualifiedAccess>]
module Ops =
    /// Adds the given number of days to the date.
    let addDays (n: int<_>) (date: Date) = date.AddDays(float n)

    /// Adds the given number of months to the date.
    let addMonths (n: int<_>) (date: Date) = date.AddMonths(int n)

    /// Adds the given number of years to the date.
    let addYears (n: int<_>) (date: Date) = date.AddYears(int n)

[<RequireQualifiedAccess>]
module Query =
    /// Returns the day moment of the given date. Defaults to dawn if the time does
    /// not have an equivalent.
    let dayMomentOf (date: Date) =
        match date.Hour with
        | 6 -> EarlyMorning
        | 10 -> Morning
        | 14 -> Midday
        | 18 -> Afternoon
        | 20 -> Evening
        | 22 -> Night
        | 0 -> Midnight
        | _ -> EarlyMorning

    /// Returns the associated time in a day of the given day moment.
    let timeOfDayMoment dayMoment =
        match dayMoment with
        | EarlyMorning -> 6
        | Morning -> 10
        | Midday -> 14
        | Afternoon -> 18
        | Evening -> 20
        | Night -> 22
        | Midnight -> 0

    /// Returns the next day moment from the given one.
    let nextDayMoment dayMoment =
        match dayMoment with
        | EarlyMorning -> Morning
        | Morning -> Midday
        | Midday -> Afternoon
        | Afternoon -> Evening
        | Evening -> Night
        | Night -> Midnight
        | Midnight -> EarlyMorning

    /// Returns the previous day moment from the given one.
    let previousDayMoment dayMoment =
        match dayMoment with
        | EarlyMorning -> Midnight
        | Morning -> EarlyMorning
        | Midday -> Morning
        | Afternoon -> Midday
        | Evening -> Afternoon
        | Night -> Evening
        | Midnight -> Night

    /// Returns the resulting date after advancing the day moment of the given
    /// one.
    let next (date: Date) =
        let nextDayMoment =
            Calendar.Query.dayMomentOf date |> Calendar.Query.nextDayMoment

        if nextDayMoment = Midnight then
            (*
            The next day moment is not in the current date anymore, so advance
            the current date as well.
            *)
            date + oneDay |> Transform.changeDayMoment nextDayMoment
        else
            (*
            The next day moment is still within the current day.
            *)
            nextDayMoment |> Calendar.Transform.changeDayMoment' date

    /// Returns the resulting date after advancing the day moment of the given
    /// n times.
    let nextN n (date: Date) =
        [ 1..n ] |> List.fold (fun date _ -> next date) date

    /// Returns all the dates between the two given dates.
    let datesBetween (beginningDate: Date) (endDate: Date) =
        [ 0 .. endDate.Subtract(beginningDate).Days ]
        |> List.map (fun daysToAdd -> beginningDate.AddDays(float daysToAdd))

    /// Returns the number of day moments between two dates.
    let rec dayMomentsBetween (beginningDate: Date) (endDate: Date) =
        if beginningDate >= endDate then
            0<dayMoments>
        else
            let nextDayMoment = next beginningDate
            1<dayMoments> + dayMomentsBetween nextDayMoment endDate

    /// Determines whether the given date is the first day of the year or not.
    let isFirstMomentOfYear (date: Date) =
        date.Day = 1 && date.Month = 1 && dayMomentOf date = EarlyMorning

    /// Returns the first date of the month from the given date.
    let firstDayOfMonth (date: Date) = DateTime(date.Year, date.Month, 1)

    /// Returns the last date of the month from the given date.
    let lastDayOfMonth (date: Date) =
        DateTime(
            date.Year,
            date.Month,
            DateTime.DaysInMonth(date.Year, date.Month)
        )

    /// Returns the first date of the next month from the given date.
    let firstDayOfNextMonth (date: Date) = date.AddMonths(1) |> firstDayOfMonth

    /// Returns the first date of the previous month from the given date.
    let firstDayOfPreviousMonth (date: Date) =
        let dateWithSubtractedMonth = date.AddMonths(-1)
        DateTime(dateWithSubtractedMonth.Year, dateWithSubtractedMonth.Month, 1)

    /// Retrieves all dates from today until the end of the month.
    let monthDaysFrom (date: Date) =
        [ date.Day .. DateTime.DaysInMonth(date.Year, date.Month) ]
        |> Seq.map (fun day -> DateTime(date.Year, date.Month, day))

    /// Returns the number of days between the two given dates.
    let daysBetween (fromDate: Date) (toDate: Date) =
        (fromDate - toDate).Days |> (*) 1<days>

    /// Returns the number of years between to dates.
    let yearsBetween (fromDate: Date) (toDate: Date) =
        (*
        SOMEHOW there's no way of getting the number of years between two dates
        by default on .NET, so estimate it by diving the number of days between
        the days in a year with a little twist added to account for leap years.
        Number taken from: https://en.wikipedia.org/wiki/Leap_year
        *)
        (toDate - fromDate).TotalDays / 365.2425 |> Math.roundToNearest

[<RequireQualifiedAccess>]
module Transform =
    /// Returns the given date with the hour set to the specified day moment.
    let changeDayMoment dayMoment (date: Date) =
        Query.timeOfDayMoment dayMoment
        |> fun hour -> DateTime(date.Year, date.Month, date.Day, hour, 0, 0)

    /// Returns the given date with the hour set to 00:00.
    let resetDayMoment = changeDayMoment Midnight

    /// Returns the given date with the hour set to the specified day moment.
    let changeDayMoment' (date: Date) dayMoment = changeDayMoment dayMoment date

[<RequireQualifiedAccess>]
module Compare =
    /// Determines whether the two given dates are the same day or not.
    let areSameDay (date1: Date) (date2: Date) =
        date1.Day = date2.Day
        && date1.Month = date2.Month
        && date1.Year = date2.Year

[<RequireQualifiedAccess>]
module Parse =
    /// Attempts to parse a given string in the format dd/mm/yyyy into a date.
    let date (strDate: string) =
        let couldParse, parsedDate =
            DateTime.TryParseExact(
                strDate,
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None
            )

        if couldParse then Some parsedDate else None

    /// Attempts to parse a given string into a day moment. Returns dawn if
    /// no compatible day moment is given.
    let dayMoment (strDayMoment: string) =
        match strDayMoment with
        | "EarlyMorning" -> EarlyMorning
        | "Morning" -> Morning
        | "Midday" -> Midday
        | "Afternoon" -> Afternoon
        | "Evening" -> Evening
        | "Night" -> Night
        | "Midnight" -> Midnight
        | _ -> EarlyMorning


[<RequireQualifiedAccess>]
module Seconds =
    /// Transforms the given number of seconds into minutes.
    let toMinutes seconds = (seconds / 60<second>) * 1<minute>

/// Returns the date in which the game starts.
let gameBeginning = Date.Now |> Transform.changeDayMoment EarlyMorning
