namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Common
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module ClockCommand =
    let private endOfDay = "End of day" |> Styles.faded

    /// Command which shows the current day moment and the number of day moments
    /// until the end of the day.
    let create
        (dayMomentsWithEvents: (DayMoment * CalendarEventType list) list)
        =
        { Name = "clock"
          Description =
            "Shows the current day moment and the number of day moments until the end of the day"
          Handler =
            (fun _ ->
                let turnInfo =
                    Queries.Calendar.currentTurnInformation (State.get ())

                dayMomentsWithEvents
                |> List.indexed
                |> List.fold
                    (fun acc (index, (dayMoment, events)) ->
                        let style =
                            match events with
                            | [] when index = 0 -> Styles.faded
                            | [] -> id
                            | _ -> Styles.highlight

                        let styledDayMoment =
                            $"{dayMoment |> Generic.dayMomentName |> style}"

                        let isLastItem =
                            index = List.length dayMomentsWithEvents - 1

                        match index with
                        | 0 when isLastItem ->
                            $"""{styledDayMoment} → {endOfDay}"""
                        | _ when isLastItem ->
                            $"""{acc} → {styledDayMoment} → {endOfDay}"""
                        | 0 -> styledDayMoment
                        | _ -> $"{acc} → {styledDayMoment}")
                    ""
                |> showMessage

                let hasAnyEvent =
                    dayMomentsWithEvents
                    |> List.collect snd
                    |> List.isEmpty
                    |> not

                if hasAnyEvent then
                    lineBreak ()

                    "* An event is scheduled at this time, check calendar for more details"
                    |> Styles.highlight
                    |> showMessage

                let formatDayMoment =
                    Generic.dayMomentName >> String.lowercase >> Styles.time

                $"Spent {turnInfo.TimeSpent} minutes on {turnInfo.CurrentDayMoment |> formatDayMoment}. {turnInfo.TimeLeft} minutes until {turnInfo.NextDayMoment |> formatDayMoment}"
                |> Styles.faded
                |> showMessage

                Scene.World) }
