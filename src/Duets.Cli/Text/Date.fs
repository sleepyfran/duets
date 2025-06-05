module Duets.Cli.Text.Date

open Duets.Entities

let seasonName (season: Season) =
    match season with
    | Spring -> "Spring"
    | Summer -> "Summer"
    | Autumn -> "Autumn"
    | Winter -> "Winter"

/// Formats a date to `Season, Year` format.
let seasonYear (date: Date) =
    $"{seasonName date.Season}, {date.Year}"

/// Formats a date to `Day d of Season, Year` format.
let simple (date: Date) = $"Day {date.Day} of {seasonYear date}"

/// Formats a date to the dd/mm/yyyy format and adds the name of the day in
/// the beginning.
let withDayName (date: Date) =
    $"{Calendar.Query.dayOfWeek date}, {simple date}"
