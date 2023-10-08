namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Interactions

[<RequireQualifiedAccess>]
module SleepCommand =
    let private eventName (event: CalendarEventType) =
        match event with
        | CalendarEventType.Concert _ -> "concert"
        | CalendarEventType.Flight _ -> "flight"

    let private showSleepResult effects =
        Interaction.sleeping |> showMessage
        wait 8000<millisecond>
        Interaction.sleepResult |> showMessage
        effects |> Duets.Cli.Effect.applyMultiple

    /// Command to sleep until a given day moment.
    let get =
        { Name = "sleep"
          Description = Command.sleepDescription
          Handler =
            fun _ ->
                let state = State.get ()

                let eventsInNextDayMoments =
                    Queries.Calendar.nextDates state
                    |> Seq.take 5
                    |> Seq.collect (fun date ->
                        let events =
                            date
                            |> Calendar.Query.dayMomentOf
                            |> Queries.CalendarEvents.forDayMoment state date

                        match events with
                        | [] ->
                            let dayMoment = Calendar.Query.dayMomentOf date
                            [ (date, dayMoment), [] ]
                        | _ -> events)
                    |> List.ofSeq

                let selectedOption =
                    showOptionalChoicePrompt
                        "Until when do you want to sleep?"
                        Generic.cancel
                        (fun ((_, dayMoment), events) ->
                            let eventList = Generic.listOf events eventName

                            match events with
                            | [] -> $"{Generic.dayMomentName dayMoment}"
                            | _ ->
                                $"""{Generic.dayMomentName dayMoment} {$"({eventList} scheduled)" |> Styles.warning}""")
                        eventsInNextDayMoments

                match selectedOption with
                | Some((date, dayMoment), _) ->
                    Sleep.sleep state date dayMoment |> showSleepResult
                | None -> ()

                Scene.World }
