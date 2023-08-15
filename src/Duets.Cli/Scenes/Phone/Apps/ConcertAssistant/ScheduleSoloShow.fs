module Duets.Cli.Scenes.Phone.Apps.ConcertAssistant.SoloShow

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Concerts

let private textFromDayMoment dayMoment = Generic.dayMomentWithTime dayMoment

let private showDateScheduledError date error =
    match error with
    | Scheduler.DateAlreadyScheduled ->
        Phone.concertAssistantAppDateAlreadyBooked date
    |> showMessage

let private showTicketPriceError ticketPrice error =
    match error with
    | Concert.PriceBelowZero ->
        Phone.concertAssistantAppTicketPriceBelowZero ticketPrice
    | Concert.PriceTooHigh ->
        Phone.concertAssistantAppTicketPriceTooHigh ticketPrice
    |> showMessage

let rec scheduleShow app =
    // Skip 5 days to give enough time for the scheduler to compute some ticket
    // purchases, otherwise the concert would be empty.
    let firstAvailableDay =
        Queries.Calendar.today (State.get ()) |> Calendar.Ops.addDays 5

    promptForDate app firstAvailableDay

and private promptForDate app firstDate =
    let selectedDate =
        showInteractiveDatePrompt
            Phone.concertAssistantAppShowDatePrompt
            firstDate

    match selectedDate with
    | Some date ->
        Scheduler.validateNoOtherConcertsInDate (State.get ()) date
        |> Result.switch (promptForDayMoment app) (fun error ->
            showDateScheduledError date error
            promptForDate app firstDate)
    | None -> app ()

and private promptForDayMoment app date =
    // Skip the first four elements since the concert spaces are closed at those
    // times anyway: Midnight, early morning, morning and midday.
    let availableDayMoments = Calendar.allDayMoments |> List.skip 4

    let selectedDayMoment =
        showOptionalChoicePrompt
            Phone.concertAssistantAppShowTimePrompt
            Generic.cancel
            textFromDayMoment
            availableDayMoments

    match selectedDayMoment with
    | Some dayMoment -> promptForCity app date dayMoment
    | None -> app ()

and private promptForCity app date dayMoment =
    let selectedCity = showCityPrompt Phone.concertAssistantAppShowCityPrompt

    match selectedCity with
    | Some city -> promptForVenue app date dayMoment city
    | None -> app ()

and private promptForVenue app date dayMoment city =
    let venues =
        Queries.World.placeIdsByTypeInCity city.Id PlaceTypeIndex.ConcertSpace
        |> List.map (Queries.World.placeInCityById city.Id)

    let selectedVenue =
        showOptionalChoicePrompt
            Phone.concertAssistantAppShowVenuePrompt
            Generic.cancel
            (fun (place: Place) ->
                match place.PlaceType with
                | ConcertSpace concertSpace ->
                    let venueCut =
                        Queries.Concerts.concertSpaceTicketPercentage place
                        |> (*) 100.0
                        |> Math.ceilToNearest

                    $"{place.Name |> Styles.place} (Capacity: {concertSpace.Capacity}, takes {venueCut |> Styles.highlight}%% of ticket sales)"
                | _ -> place.Name)
            venues

    match selectedVenue with
    | Some place -> promptForTicketPrice app date dayMoment city place.Id
    | None -> app ()

and private promptForTicketPrice app date dayMoment city venueId =
    let ticketPrice =
        showDecimalPrompt Phone.concertAssistantAppTicketPricePrompt

    Concert.validatePrice ticketPrice
    |> Result.switch
        (scheduleConcert app date dayMoment city venueId)
        (fun error ->
            showTicketPriceError ticketPrice error
            promptForTicketPrice app date dayMoment city venueId)

and private scheduleConcert app date dayMoment city venueId ticketPrice =
    Scheduler.scheduleHeadlinerConcert
        (State.get ())
        date
        dayMoment
        city.Id
        venueId
        ticketPrice
    |> Effect.apply

    Scene.Phone
