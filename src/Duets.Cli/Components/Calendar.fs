[<AutoOpen>]
module Duets.Cli.Components.Calendar

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

/// <summary>
/// Renders a calendar for the given month of the given year highlighting the
/// dates provided.
/// </summary>
/// <param name="year">Year to display</param>
/// <param name="season">Season to display</param>
/// <param name="events">List of dates to highlight</param>
let showCalendar (year: int<years>) season (events: Date list) =
    let today = Queries.Calendar.today (State.get ())

    (*
    Get a set of dates for quickly contains checking and make sure that their
    day moments are set to the start of the day to be able to match by equality.
    *)
    let eventSet =
        events |> List.map Calendar.Transform.resetDayMoment |> Set.ofList

    let tableColumns =
        [ "Mon"; "Tue"; "Wed"; "Thu"; "Fri"; "Sat"; "Sun" ]
        |> List.map Styles.header

    let firstDayOfSeason = Calendar.Date.fromSeasonAndYear season year

    let tableRows =
        Calendar.Query.seasonDaysFrom firstDayOfSeason
        |> List.ofSeq
        |> List.map (fun date ->
            let hasEvent =
                Set.contains (Calendar.Transform.resetDayMoment date) eventSet

            let style =
                if date = today then Styles.faded
                elif hasEvent then Styles.highlight
                else Styles.number

            style date.Day)
        |> List.splitInto 3

    let title = Calendar.Date.fromSeasonAndYear season year |> Date.seasonYear
    showTableWithTitle title tableColumns tableRows
