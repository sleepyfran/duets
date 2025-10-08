module rec Duets.Entities.Calendar

open FSharp.Data.UnitSystems.SI.UnitNames

let daysInSeason = 21<days>

let allDayMoments =
    [ Midnight; EarlyMorning; Morning; Midday; Afternoon; Evening; Night ]

let weekday = [ Monday; Tuesday; Wednesday; Thursday; Wednesday; Friday ]

let everyDay =
    [ Monday
      Tuesday
      Wednesday
      Thursday
      Wednesday
      Friday
      Saturday
      Sunday ]

module DayMoments =
    /// Contains all the possible day moments in a week.
    let oneWeek = Calendar.allDayMoments |> List.length |> (*) 7<dayMoments>

    /// Transforms the given number of day moments into minutes.
    let toMinutes dayMoments =
        dayMoments / 1<dayMoments> * 180<minute>

[<RequireQualifiedAccess>]
module Ops =
    /// Adds the given number of years to the date.
    let addYears (n: int<_>) (date: Date) = { date with Year = date.Year + n }

    /// Adds a season to the date, rolling the year as needed.
    let addSeason (date: Date) =
        let nextSeason =
            match date.Season with
            | Spring -> Summer
            | Summer -> Autumn
            | Autumn -> Winter
            | Winter -> Spring

        let updatedDate = { date with Season = nextSeason }

        if nextSeason = Spring then
            addYears 1<years> updatedDate
        else
            updatedDate

    /// Subtracts a season to the date, rolling the year as needed.
    let subtractSeason (date: Date) =
        let previousSeason =
            match date.Season with
            | Spring -> Winter
            | Summer -> Spring
            | Autumn -> Summer
            | Winter -> Autumn

        let updatedDate = { date with Season = previousSeason }

        if previousSeason = Winter then
            addYears -1<years> updatedDate
        else
            updatedDate

    /// Adds the given number of seasons to the date, rolling the year as needed.
    /// Supports both positive and negative numbers.
    let addSeasons (n: int) (date: Date) =
        let mapDate = if n > 0 then addSeason else subtractSeason
        [ 0 .. abs n ] |> List.fold (fun date _ -> mapDate date) date

    /// Adds a day to the given date, rolling seasons and years as needed.
    let addDay (date: Date) =
        let nextDay = date.Day + 1<days>

        if nextDay > daysInSeason then
            { addSeason date with Day = 1<days> }
        else
            { date with Day = nextDay }

    /// Subtracts one day from the given date, rolling seasons and years as
    /// necessary.
    let subtractDay (date: Date) =
        let previousDay = date.Day - 1<days>

        if previousDay < 1<days> then
            { subtractSeason date with
                Day = daysInSeason }
        else
            { date with Day = previousDay }

    /// Adds the given number of days to the date, rolling the season and year
    /// as needed. Supports both positive and negative numbers.
    let addDays (n: int<days>) (date: Date) =
        let mapDate = if n > 0<days> then addDay else subtractDay

        [ 1 .. abs (n / 1<days>) ]
        |> List.fold (fun date _ -> mapDate date) date

[<RequireQualifiedAccess>]
module Query =
    /// Returns the day moment of the given date. Defaults to dawn if the time does
    /// not have an equivalent.
    let dayMomentOf (date: Date) = date.DayMoment

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
        let updatedDayMoment = dayMomentOf date |> nextDayMoment

        if updatedDayMoment = Midnight then
            (*
            The next day moment is not in the current date anymore, so advance
            the current date as well.
            *)
            Ops.addDays 1<days> date
            |> Transform.changeDayMoment updatedDayMoment
        else
            (*
            The next day moment is still within the current day.
            *)
            updatedDayMoment |> Transform.changeDayMoment' date

    /// Returns the resulting date after advancing the day moment of the given
    /// n times.
    let nextN n (date: Date) =
        [ 1..n ] |> List.fold (fun date _ -> next date) date

    /// Returns the day of the week that a given date is on.
    let dayOfWeek (date: Date) : DayOfWeek =
        let dayOfWeekNumber = date.Day % 7<days>

        match dayOfWeekNumber with
        | 1<days> -> Monday
        | 2<days> -> Tuesday
        | 3<days> -> Wednesday
        | 4<days> -> Thursday
        | 5<days> -> Friday
        | 6<days> -> Saturday
        | _ -> Sunday

    /// Returns the number of years between to dates.
    let yearsBetween (fromDate: Date) (toDate: Date) =
        fromDate.Year - toDate.Year

    /// Returns all the dates between the two given dates.
    let datesBetween (beginningDate: Date) (endDate: Date) =
        let rec advanceDay currentDate targetDate dates =
            let currentWithoutDayMoment = Transform.resetDayMoment currentDate
            let targetWithoutDayMoment = Transform.resetDayMoment targetDate

            if currentWithoutDayMoment = targetWithoutDayMoment then
                dates @ [ currentDate ]
            else
                let nextDate = Ops.addDay currentDate
                advanceDay nextDate targetDate (dates @ [ currentDate ])

        if beginningDate <= endDate then
            advanceDay beginningDate endDate []
        else
            advanceDay endDate beginningDate []

    /// Returns the number of days between the two given dates.
    let daysBetween (fromDate: Date) (toDate: Date) : int<days> =
        let dates = datesBetween fromDate toDate
        (*
        Before switching to our custom Date structure we were using DateTime,
        which used the hours as well to compute the difference between two dates,
        and which we're not doing right now to simplify computations. Due to this,
        this function used to always return the number of days between two days
        without including one of the edges, thus why to preserve that logic I'm
        subtracting one day from the total.
        *)
        (dates.Length * 1<days>) - 1<days>

    /// Returns the number of day moments between two dates.
    let rec dayMomentsBetween (beginningDate: Date) (endDate: Date) =
        if beginningDate >= endDate then
            0<dayMoments>
        else
            let nextDayMoment = next beginningDate
            1<dayMoments> + dayMomentsBetween nextDayMoment endDate

    /// Counts the number of day moments between the current date and the given
    /// day moment.
    let dayMomentsUntil (dayMoment: DayMoment) (currentDate: Date) =
        let rec count n (targetDayMoment: DayMoment) (date: Date) =
            if date.DayMoment = targetDayMoment then
                n
            else
                date |> next |> count (n + 1<dayMoments>) targetDayMoment

        count 0<dayMoments> dayMoment currentDate

    /// Determines whether the given date is the first day of the year or not.
    let isFirstMomentOfYear (date: Date) =
        date.Day = 1<days>
        && date.Season = Spring
        && dayMomentOf date = EarlyMorning

    /// Returns the first date of the season from the given date.
    let firstDayOfSeason (date: Date) = { date with Day = 1<days> }

    /// Returns the last date of the season from the given date.
    let lastDayOfSeason (date: Date) = { date with Day = daysInSeason }

    /// Returns the first date of the next season from the given date.
    let firstDayOfNextSeason (date: Date) =
        Ops.addSeason date |> firstDayOfSeason

    /// Returns the first date of the previous season from the given date.
    let firstDayOfPreviousSeason (date: Date) =
        let dateWithSubtractedSeason = Ops.subtractSeason date

        { dateWithSubtractedSeason with
            Day = 1<days> }

    /// Retrieves all dates from today until the end of the season.
    let seasonDaysFrom (date: Date) =
        [ date.Day / 1<days> .. daysInSeason / 1<days> ]
        |> List.map (fun day -> { date with Day = day * 1<days> })

[<RequireQualifiedAccess>]
module Transform =
    /// Returns the given date with the hour set to the specified day moment.
    let changeDayMoment dayMoment (date: Date) =
        { date with DayMoment = dayMoment }

    /// Returns the given date with the hour set to 00:00.
    let resetDayMoment = changeDayMoment Midnight

    /// Returns the given date with the hour set to the specified day moment.
    let changeDayMoment' (date: Date) dayMoment = changeDayMoment dayMoment date

[<RequireQualifiedAccess>]
module Compare =
    /// Determines whether the two given dates are the same day or not.
    let areSameDay (date1: Date) (date2: Date) =
        date1.Day = date2.Day
        && date1.Season = date2.Season
        && date1.Year = date2.Year

[<RequireQualifiedAccess>]
module Parse =
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

[<RequireQualifiedAccess>]
module Date =
    /// Creates a date with the year set to the given year and the rest of
    /// the fields to their initial value, which is 1st of Spring in the early
    /// morning.
    let fromYear year : Date =
        { DayMoment = EarlyMorning
          Season = Spring
          Year = year
          Day = 1<days> }

    /// Creates a date with the year and season set to the given values, with
    /// the rest of the fields set to their initial value, which is 1st day
    /// in the early morning.
    let fromSeasonAndYear season year : Date =
        { DayMoment = EarlyMorning
          Season = season
          Year = year
          Day = 1<days> }

module Shorthands =
    /// Creates a new date in the given day and year, during spring.
    let Spring day year : Date =
        { DayMoment = EarlyMorning
          Day = day
          Year = year
          Season = Season.Spring }

    /// Creates a new date in the given day and year, during summer.
    let Summer day year : Date =
        { DayMoment = EarlyMorning
          Day = day
          Year = year
          Season = Season.Summer }

    /// Creates a new date in the given day and year, during autumn.
    let Autumn day year : Date =
        { DayMoment = EarlyMorning
          Day = day
          Year = year
          Season = Season.Autumn }

    /// Creates a new date in the given day and year, during winter.
    let Winter day year : Date =
        { DayMoment = EarlyMorning
          Day = day
          Year = year
          Season = Season.Winter }

/// Returns the date in which the game starts.
let gameBeginning: Date =
    { Season = Spring
      Year = 2025<years>
      Day = 1<days>
      DayMoment = EarlyMorning }
