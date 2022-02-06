module Cli.View.Scenes.Phone.Apps.SchedulerAssistant.Visualize

open Agents
open Cli.View.Actions
open Cli.View.Common
open Cli.View.Text
open Entities
open Simulation.Queries

let rec visualizeSchedule app =
    State.get ()
    |> Calendar.today
    |> visualizeSchedule' app

and private visualizeSchedule' app firstDay =
    let state = State.get ()

    let currentBand = Bands.currentBand state

    let nextMonthDate =
        Calendar.Query.firstDayOfNextMonth firstDay

    let concertsInMonth =
        Schedule.concertScheduleForMonth state currentBand.Id firstDay
        |> List.ofSeq

    let calendarEvents =
        concertsInMonth
        |> List.map (fun concert -> concert.Date)

    seq {
        yield NewLine

        yield
            (firstDay.Year, firstDay.Month, calendarEvents)
            |> Calendar

        yield!
            match concertsInMonth with
            | [] ->
                seq {
                    PhoneText SchedulerAssistantAppVisualizeNoConcerts
                    |> I18n.translate
                    |> Message
                }
            | concerts ->
                concerts
                |> Seq.map
                    (fun concert ->
                        seq {
                            let resolvedConcert = Concerts.info state concert

                            yield
                                CommonDateWithDay resolvedConcert.Date
                                |> CommonText
                                |> I18n.translate
                                |> Rule

                            yield
                                SchedulerAssistantAppVisualizeConcertInfo(
                                    resolvedConcert.DayMoment,
                                    resolvedConcert.Venue,
                                    resolvedConcert.City,
                                    resolvedConcert.TicketsSold
                                )
                                |> PhoneText
                                |> I18n.translate
                                |> Message
                        })
                |> Seq.concat

        yield Separator

        yield
            Prompt
                { Title =
                      I18n.translate (
                          PhoneText
                              SchedulerAssistantAppVisualizeMoreDatesPrompt
                      )
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices =
                                [ { Id = "next"
                                    Text =
                                        PhoneText
                                            SchedulerAssistantCommonMoreDates
                                        |> I18n.translate } ]
                            Handler =
                                basicOptionalChoiceHandler
                                    (app ())
                                    (fun _ ->
                                        visualizeSchedule' app nextMonthDate)
                            BackText = CommonText CommonBack |> I18n.translate } }
    }
