module Cli.Scenes.Phone.Apps.SchedulerAssistant.Agenda


open Agents
open Cli.Components
open Cli.Text
open Entities
open Simulation

type private ScheduleAgendaMenuOption = | MoreDates

let rec private textFromOption opt =
    match opt with
    | MoreDates -> Phone.schedulerAssistantCommonMoreDates

let rec showAgenda app =
    State.get () |> Queries.Calendar.today |> showAgenda' app

and private showAgenda' app firstDay =
    let state = State.get ()

    let currentBand = Queries.Bands.currentBand state

    let nextMonthDate = Calendar.Query.firstDayOfNextMonth firstDay

    let concertsInMonth =
        Queries.Concerts.scheduleForMonth state currentBand.Id firstDay
        |> Seq.map Concert.fromScheduled
        |> List.ofSeq

    let calendarEvents =
        concertsInMonth |> List.map (fun concert -> concert.Date)

    lineBreak ()

    showCalendar firstDay.Year firstDay.Month calendarEvents

    if List.isEmpty concertsInMonth then
        Phone.schedulerAssistantAppVisualizeNoConcerts |> showMessage
    else
        showConcertList app concertsInMonth

    lineBreak ()

    let selectedOption =
        showOptionalChoicePrompt
            Phone.schedulerAssistantAppVisualizeMoreDatesPrompt
            Generic.back
            textFromOption
            [ MoreDates ]

    match selectedOption with
    | Some _ -> showAgenda' app nextMonthDate
    | None -> app ()

and showConcertList _ =
    List.iter (fun concert ->
        let city = Queries.World.cityById concert.CityId

        let place = Queries.World.placeInCityById concert.CityId concert.VenueId

        Generic.dateWithDay concert.Date |> Some |> showSeparator

        Phone.schedulerAssistantAppVisualizeConcertInfo
            concert.DayMoment
            place
            city
            concert.TicketsSold
        |> showMessage)
