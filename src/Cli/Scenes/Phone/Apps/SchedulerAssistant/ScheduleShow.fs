module Cli.Scenes.Phone.Apps.SchedulerAssistant.Show

open Agents
open Cli
open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Common
open Entities
open Simulation
open Simulation.Concerts

let private textFromDate date = Generic.dateWithDay date

let private textFromDayMoment dayMoment = Generic.dayMomentWithTime dayMoment

let private showDateScheduledError date error =
    match error with
    | Scheduler.DateAlreadyScheduled ->
        Phone.schedulerAssistantAppDateAlreadyBooked date
    |> showMessage

let private showTicketPriceError ticketPrice error =
    match error with
    | Concert.PriceBelowZero ->
        Phone.schedulerAssistantAppTicketPriceBelowZero ticketPrice
    | Concert.PriceTooHigh ->
        Phone.schedulerAssistantAppTicketPriceTooHigh ticketPrice
    |> showMessage

let rec scheduleShow app =
    // Skip 5 days to give enough time for the scheduler to compute some ticket
    // purchases, otherwise the concert would be empty.
    let firstAvailableDay =
        Queries.Calendar.today (State.get ()) |> Calendar.Ops.addDays 5

    promptForDate app firstAvailableDay

and private promptForDate app firstDate =
    let monthDays = Calendar.Query.monthDaysFrom firstDate

    let nextMonthDate = Calendar.Query.firstDayOfNextMonth firstDate

    let selectedDate =
        showOptionalChoicePrompt
            Phone.schedulerAssistantAppShowDatePrompt
            Phone.schedulerAssistantCommonMoreDates
            textFromDate
            monthDays

    match selectedDate with
    | Some date ->
        Scheduler.validateNoOtherConcertsInDate (State.get ()) date
        |> Result.switch (promptForDayMoment app) (fun error ->
            showDateScheduledError date error
            promptForDate app firstDate)
    | None -> promptForDate app nextMonthDate

and private promptForDayMoment app date =
    // Drop the last element (Midnight) since we don't want to allow scheduling
    // on the day after the selection.
    let availableDayMoments = Calendar.allDayMoments |> List.tail

    let selectedDayMoment =
        showOptionalChoicePrompt
            Phone.schedulerAssistantAppShowTimePrompt
            Generic.cancel
            textFromDayMoment
            availableDayMoments

    match selectedDayMoment with
    | Some dayMoment -> promptForCity app date dayMoment
    | None -> app ()

and private promptForCity app date dayMoment =
    let cities = Queries.World.allCities

    let selectedCity =
        showOptionalChoicePrompt
            Phone.schedulerAssistantAppShowCityPrompt
            Generic.cancel
            (fun (city: City) -> Generic.cityName city.Id)
            cities

    match selectedCity with
    | Some city -> promptForVenue app date dayMoment city
    | None -> app ()

and private promptForVenue app date dayMoment city =
    let venues =
        Queries.World.placeIdsOf city.Id PlaceTypeIndex.ConcertSpace
        |> List.map (Queries.World.placeInCityById city.Id)

    let selectedVenue =
        showOptionalChoicePrompt
            Phone.schedulerAssistantAppShowVenuePrompt
            Generic.cancel
            (fun (place: Place) -> place.Name)
            venues

    match selectedVenue with
    | Some place -> promptForTicketPrice app date dayMoment city place.Id
    | None -> app ()

and private promptForTicketPrice app date dayMoment city venueId =
    let ticketPrice =
        showNumberPrompt Phone.schedulerAssistantAppTicketPricePrompt

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
