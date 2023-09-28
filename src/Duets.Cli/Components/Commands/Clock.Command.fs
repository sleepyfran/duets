namespace Duets.Cli.Components.Commands

open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities

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

                Scene.World) }
