module rec Duets.Cli.Scenes.Phone.Apps.Calendar.Root

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

type private CalendarMenuOption =
    | NextMonth
    | PreviousMonth

let rec private textFromOption opt =
    match opt with
    | NextMonth -> "Next month"
    | PreviousMonth -> "Previous month"
    |> Styles.faded

let rec calendarApp () =
    State.get () |> Queries.Calendar.today |> calendarApp'

let private calendarApp' firstDay =
    let nextMonthDate = Calendar.Query.firstDayOfNextMonth firstDay
    let previousMonthDate = Calendar.Query.firstDayOfPreviousMonth firstDay

    let calendarEvents =
        Queries.CalendarEvents.allOfDateMonth (State.get ()) firstDay

    let calendarEventDates = calendarEvents |> List.map fst

    lineBreak ()

    calendarEventDates
    |> List.map fst
    |> showCalendar firstDay.Year firstDay.Month

    if List.isEmpty calendarEvents then
        "No events this month" |> showMessage
    else
        showEventList calendarEvents

    lineBreak ()

    let selectedOption =
        showOptionalChoicePrompt
            Phone.concertAssistantAppVisualizeMoreDatesPrompt
            Generic.back
            textFromOption
            [ NextMonth; PreviousMonth ]

    match selectedOption with
    | Some NextMonth -> calendarApp' nextMonthDate
    | Some PreviousMonth -> calendarApp' previousMonthDate
    | None -> Scene.Phone

let private showEventList dateGroupedEvents =
    dateGroupedEvents
    |> List.iter (fun ((date, _), events) ->
        Generic.dateWithDay date |> Some |> showSeparator

        events
        |> List.iter (fun event ->
            match event with
            | CalendarEventType.Flight flight -> showFlight flight
            | CalendarEventType.Concert concert -> showConcert concert))

let private showFlight flight =
    $"""{Styles.highlight $"*{Generic.dayMomentName flight.DayMoment}"}: Flight from {Generic.cityName flight.Origin |> Styles.place} to {Generic.cityName flight.Destination |> Styles.place}"""
    |> showMessage

let private showConcert concert =
    let city = Queries.World.cityById concert.CityId

    let place = Queries.World.placeInCityById concert.CityId concert.VenueId

    $"""{Styles.highlight $"*{Generic.dayMomentName concert.DayMoment}"}: Concert at {Styles.place place.Name}, {Styles.place (Generic.cityName city.Id)}. Sold {Styles.information concert.TicketsSold} tickets"""
    |> showMessage
