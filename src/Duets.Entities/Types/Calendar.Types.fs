namespace Duets.Entities

[<AutoOpen>]
module CalendarTypes =
    /// Measure for a minute of time.
    [<Measure>]
    type minute

    /// Measure for days.
    [<Measure>]
    type days

    /// Measure for years.
    [<Measure>]
    type years

    /// Measure for a number of day moments.
    [<Measure>]
    type dayMoments

    /// Season of the year.
    type Season =
        | Spring
        | Summer
        | Autumn
        | Winter

    /// Since the game does not feature a 24-hour clock to not make days
    /// unbearably long in-game, these are the moments that of the day that
    /// we will show the user depending on the real time from the date.
    type DayMoment =
        | EarlyMorning
        | Morning
        | Midday
        | Afternoon
        | Evening
        | Night
        | Midnight

    /// Represents a date in the game.
    [<CustomComparison; CustomEquality>]
    type Date =
        { Day: int<days>
          Season: Season
          Year: int<years>
          DayMoment: DayMoment }

        override this.Equals(obj) =
            match obj with
            | :? Date as other ->
                this.Year = other.Year
                && this.Season = other.Season
                && this.Day = other.Day
                && this.DayMoment = other.DayMoment
            | _ -> false

        override this.GetHashCode() =
            hash (this.Year, this.Season, this.Day, this.DayMoment)

        static member private SeasonToInt =
            function
            | Spring -> 0
            | Summer -> 1
            | Autumn -> 2
            | Winter -> 3

        static member private DayMomentToInt =
            function
            | Midnight -> 0
            | EarlyMorning -> 1
            | Morning -> 2
            | Midday -> 3
            | Afternoon -> 4
            | Evening -> 5
            | Night -> 6

        interface System.IComparable with
            member this.CompareTo other =
                match other with
                | :? Date as otherDate ->
                    match compare this.Year otherDate.Year with
                    | 0 ->
                        match
                            compare
                                (Date.SeasonToInt this.Season)
                                (Date.SeasonToInt otherDate.Season)
                        with
                        | 0 ->
                            match compare this.Day otherDate.Day with
                            | 0 ->
                                compare
                                    (Date.DayMomentToInt this.DayMoment)
                                    (Date.DayMomentToInt otherDate.DayMoment)
                            | result -> result
                        | result -> result
                    | result -> result
                | _ -> -1

        static member op_LessThan(date1: Date, date2: Date) =
            (date1 :> System.IComparable).CompareTo(date2) < 0

        static member op_LessThanOrEqual(date1: Date, date2: Date) =
            (date1 :> System.IComparable).CompareTo(date2) <= 0

        static member op_GreaterThan(date1: Date, date2: Date) =
            (date1 :> System.IComparable).CompareTo(date2) > 0

        static member op_GreaterThanOrEqual(date1: Date, date2: Date) =
            (date1 :> System.IComparable).CompareTo(date2) >= 0

    /// Defines a period of time with a start and an optional end.
    type Period = Date * Date
