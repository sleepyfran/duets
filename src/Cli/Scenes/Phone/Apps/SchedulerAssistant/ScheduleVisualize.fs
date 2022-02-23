module Cli.Scenes.Phone.Apps.SchedulerAssistant.Agenda


open Agents
open Cli.Components
open Cli.Text
open Entities
open Simulation.Queries

type private ScheduleAgendaMenuOption = | MoreDates

let rec private textFromOption opt =
    match opt with
    | MoreDates -> PhoneText SchedulerAssistantCommonMoreDates
    |> I18n.translate

let rec showAgenda app =
    State.get () |> Calendar.today |> showAgenda' app

and private showAgenda' app firstDay =
    let state = State.get ()

    let currentBand = Bands.currentBand state

    let nextMonthDate = Calendar.Query.firstDayOfNextMonth firstDay

    let concertsInMonth =
        Schedule.concertScheduleForMonth state currentBand.Id firstDay
        |> List.ofSeq

    let calendarEvents =
        concertsInMonth
        |> List.map (fun concert -> concert.Date)

    lineBreak ()

    showCalendar firstDay.Year firstDay.Month calendarEvents

    if List.isEmpty concertsInMonth then
        PhoneText SchedulerAssistantAppVisualizeNoConcerts
        |> I18n.translate
        |> showMessage
    else
        showConcertList app concertsInMonth

    lineBreak ()

    let selectedOption =
        showOptionalChoicePrompt
            (PhoneText SchedulerAssistantAppVisualizeMoreDatesPrompt
             |> I18n.translate)
            (CommonText CommonBack |> I18n.translate)
            textFromOption
            [ MoreDates ]

    match selectedOption with
    | Some _ -> showAgenda' app nextMonthDate
    | None -> app ()

and showConcertList app =
    let state = State.get ()

    List.iter (fun concert ->
        let resolvedConcert = Concerts.info state concert

        CommonDateWithDay resolvedConcert.Date
        |> CommonText
        |> I18n.translate
        |> Some
        |> showSeparator

        SchedulerAssistantAppVisualizeConcertInfo(
            resolvedConcert.DayMoment,
            resolvedConcert.Venue,
            resolvedConcert.City,
            resolvedConcert.TicketsSold
        )
        |> PhoneText
        |> I18n.translate
        |> showMessage)
