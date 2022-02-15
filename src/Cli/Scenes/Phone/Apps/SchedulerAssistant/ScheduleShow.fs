module Cli.Scenes.Phone.Apps.SchedulerAssistant.Show

open Agents
open Cli
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Common
open Entities
open Simulation
open Simulation.Queries

let private textFromDate date =
    CommonDateWithDay date
    |> CommonText
    |> I18n.translate

let private textFromDayMoment dayMoment =
    CommonDayMomentWithTime dayMoment
    |> CommonText
    |> I18n.translate

let private showDateScheduledError date error =
    match error with
    | Scheduler.DateAlreadyScheduled ->
        SchedulerAssistantAppDateAlreadyBooked date
        |> PhoneText
    |> I18n.translate
    |> showMessage

let private showTicketPriceError ticketPrice error =
    match error with
    | Concert.PriceBelowZero ->
        SchedulerAssistantAppTicketPriceBelowZero ticketPrice
        |> PhoneText
    | Concert.PriceTooHigh ->
        SchedulerAssistantAppTicketPriceTooHigh ticketPrice
        |> PhoneText
    |> I18n.translate
    |> showMessage

let rec scheduleShow app =
    // Skip 5 days to give enough time for the scheduler to compute some ticket
    // purchases, otherwise the concert would be empty.
    let firstAvailableDay =
        Calendar.today (State.get ())
        |> Calendar.Ops.addDays 5

    promptForDate app firstAvailableDay

and private promptForDate app firstDate =
    let monthDays = Calendar.Query.monthDaysFrom firstDate

    let nextMonthDate =
        Calendar.Query.firstDayOfNextMonth firstDate

    let selectedDate =
        showOptionalChoicePrompt
            (PhoneText SchedulerAssistantAppShowDatePrompt
             |> I18n.translate)
            (PhoneText SchedulerAssistantCommonMoreDates
             |> I18n.translate)
            textFromDate
            monthDays

    match selectedDate with
    | Some date ->
        Scheduler.validateNoOtherConcertsInDate (State.get ()) date
        |> Result.switch
            (promptForDayMoment app)
            (fun error ->
                showDateScheduledError date error
                promptForDate app firstDate)
    | None -> promptForDate app nextMonthDate

and private promptForDayMoment app date =
    // Drop the last element (Midnight) since we don't want to allow scheduling
    // on the day after the selection.
    let availableDayMoments = Calendar.allDayMoments |> List.tail

    let selectedDayMoment =
        showOptionalChoicePrompt
            (PhoneText SchedulerAssistantAppShowTimePrompt
             |> I18n.translate)
            (CommonText CommonCancel |> I18n.translate)
            textFromDayMoment
            availableDayMoments

    match selectedDayMoment with
    | Some dayMoment -> promptForCity app date dayMoment
    | None -> app ()

and private promptForCity app date dayMoment =
    let cities = World.allCities (State.get ())

    let selectedCity =
        showOptionalChoicePrompt
            (PhoneText SchedulerAssistantAppShowCityPrompt
             |> I18n.translate)
            (CommonText CommonCancel |> I18n.translate)
            (fun (city: City) -> I18n.constant city.Name)
            cities

    match selectedCity with
    | Some city -> promptForVenue app date dayMoment city
    | None -> app ()

and private promptForVenue app date dayMoment city =
    let state = State.get ()

    let venues =
        World.allConcertSpacesOfCity state city.Id

    let selectedVenue =
        showOptionalChoicePrompt
            (PhoneText SchedulerAssistantAppShowVenuePrompt
             |> I18n.translate)
            (CommonText CommonCancel |> I18n.translate)
            (fun (_, venue: ConcertSpace) -> I18n.constant venue.Name)
            venues

    match selectedVenue with
    | Some (nodeId, _) -> promptForTicketPrice app date dayMoment city nodeId
    | None -> app ()

and private promptForTicketPrice app date dayMoment city venueId =
    let ticketPrice =
        showNumberPrompt (
            PhoneText SchedulerAssistantAppTicketPricePrompt
            |> I18n.translate
        )

    Concert.validatePrice ticketPrice
    |> Result.switch
        (scheduleConcert app date dayMoment city venueId)
        (fun error ->
            showTicketPriceError ticketPrice error
            promptForTicketPrice app date dayMoment city venueId)

and private scheduleConcert app date dayMoment city venueId ticketPrice =
    Scheduler.scheduleConcert
        (State.get ())
        date
        dayMoment
        city.Id
        venueId
        ticketPrice
    |> Effect.apply

    Scene.Phone
