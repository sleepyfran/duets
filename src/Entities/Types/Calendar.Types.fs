namespace Entities

[<AutoOpen>]
module CalendarTypes =
    /// Represents a date in the game.
    type Date = System.DateTime

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

    /// Defines a period of time with a start and an optional end.
    type Period = Date * Date

    /// Measure for a minute of time.
    [<Measure>]
    type minute
    
    /// Measure for a number of day moments.
    [<Measure>]
    type dayMoments
