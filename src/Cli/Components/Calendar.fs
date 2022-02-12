[<AutoOpen>]
module Cli.Components.Calendar

open Entities
open Spectre.Console

/// <summary>
/// Renders a calendar for the given month of the given year highlighting the
/// dates provided.
/// </summary>
/// <param name="year">Year to display</param>
/// <param name="month">Month to display</param>
/// <param name="events">List of dates to highlight</param>
let showCalendar year month (events: Date list) =
    let mutable calendar = Calendar(year, month)
    calendar.Culture <- System.Threading.Thread.CurrentThread.CurrentCulture
    calendar.HightlightStyle <- Style.Parse("deepskyblue3 bold")

    for event in events do
        calendar <-
            calendar.AddCalendarEvent(event.Year, event.Month, event.Day)

    AnsiConsole.Write(calendar)
