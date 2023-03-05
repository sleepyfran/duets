module Duets.Cli.Text.Date

open Duets.Entities

/// Formats a date to the dd/mm/yyyy format.
let simple (date: Date) = date.ToString("dd/MM/yyyy")

/// Formats a date to the dd/mm/yyyy format and adds the name of the day in
/// the beginning.
let withDayName (date: Date) = $"{date.DayOfWeek.ToString()}, {simple date}"
