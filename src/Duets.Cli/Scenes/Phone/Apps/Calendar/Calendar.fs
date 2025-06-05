module rec Duets.Cli.Scenes.Phone.Apps.Calendar.Root

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

type private CalendarMenuOption =
    | NextSeason
    | PreviousSeason

let rec private textFromOption opt =
    match opt with
    | NextSeason -> "Next season"
    | PreviousSeason -> "Previous season"
    |> Styles.faded

let rec calendarApp () =
    State.get () |> Queries.Calendar.today |> calendarApp'

let private calendarApp' firstDay =
    let nextSeasonDate = Calendar.Query.firstDayOfNextSeason firstDay
    let previousSeasonDate = Calendar.Query.firstDayOfPreviousSeason firstDay

    let calendarEvents =
        Queries.CalendarEvents.allOfDateSeason (State.get ()) firstDay

    let calendarEventDates = calendarEvents |> List.map fst

    lineBreak ()

    calendarEventDates
    |> List.map fst
    |> showCalendar firstDay.Year firstDay.Season

    if List.isEmpty calendarEvents then
        "No events this season" |> Styles.danger |> showMessage
    else
        showEventList calendarEvents

    lineBreak ()

    let selectedOption =
        showOptionalChoicePrompt
            Phone.concertAssistantAppVisualizeMoreDatesPrompt
            Generic.back
            textFromOption
            [ NextSeason; PreviousSeason ]

    match selectedOption with
    | Some NextSeason -> calendarApp' nextSeasonDate
    | Some PreviousSeason -> calendarApp' previousSeasonDate
    | None -> Scene.Phone

let private showEventList dateGroupedEvents =
    let today = Queries.Calendar.today (State.get ())
    let tomorrow = today |> Calendar.Ops.addDays 1<days>

    dateGroupedEvents
    |> List.iter (fun ((date, _), events) ->
        if Calendar.Compare.areSameDay date today then
            "Today"
        else if Calendar.Compare.areSameDay date tomorrow then
            "Tomorrow"
        else
            Generic.dateWithDay date
        |> Some
        |> showSeparator

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

    let concertInformation =
        match concert.ParticipationType with
        | Headliner ->
            $"Concert at {Styles.place place.Name}, {Styles.place (Generic.cityName city.Id)}. Sold {Styles.information concert.TicketsSold} tickets"
        | OpeningAct(bandId, _) ->
            let headliner = Queries.Bands.byId (State.get ()) bandId

            $"Opening for {Styles.band headliner.Name} at {Styles.place place.Name}, {Styles.place (Generic.cityName city.Id)}. Sold {Styles.information concert.TicketsSold} tickets"


    $"""{Styles.highlight $"*{Generic.dayMomentName concert.DayMoment}"}: {concertInformation}"""
    |> showMessage
