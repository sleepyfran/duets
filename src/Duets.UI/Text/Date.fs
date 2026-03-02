module Duets.UI.Text.Date

open Duets.Entities

let seasonName =
    function
    | Spring -> "Spring"
    | Summer -> "Summer"
    | Autumn -> "Autumn"
    | Winter -> "Winter"

let format (date: Date) =
    $"Day {date.Day} of {seasonName date.Season}, {date.Year}"

let dayMomentName =
    function
    | EarlyMorning -> "Early Morning"
    | Morning -> "Morning"
    | Midday -> "Midday"
    | Afternoon -> "Afternoon"
    | Evening -> "Evening"
    | Night -> "Night"
    | Midnight -> "Midnight"
