module rec Duets.Cli.Scenes.Phone.Apps.Flights.BookFlight

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.Text
open Duets.Common
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Bank.Operations

let bookFlight flightsApp =
    let currentCity = Queries.World.currentCity (State.get ())

    let origin =
        showOptionalChoicePrompt
            Phone.bookFlightOriginPrompt
            Generic.cancel
            (fun (city: City) ->
                if city.Id = currentCity.Id then
                    $"{Generic.cityName city.Id} (Current)" |> Styles.highlight
                else
                    Generic.cityName city.Id)
            (originCities currentCity)

    match origin with
    | Some origin -> destinationPrompt flightsApp origin
    | None -> flightsApp ()

/// Lists all available cities with the current one at the top.
let private originCities currentCity =
    let allButCurrentCity =
        Queries.World.allCities
        |> List.filter (fun city -> city.Id <> currentCity.Id)
        |> List.sortBy (fun city -> Generic.cityName city.Id)

    currentCity :: allButCurrentCity

let private destinationPrompt flightsApp origin =
    let allCitiesExceptOrigin =
        Queries.World.allCities
        |> List.filter (fun city -> city.Id <> origin.Id)

    let destination =
        showChoicePrompt
            Phone.bookFlightDestinationPrompt
            (fun (city: City) -> Generic.cityName city.Id)
            allCitiesExceptOrigin

    let initialDate = Queries.Calendar.tomorrow (State.get ())

    let date =
        showInteractiveDatePrompt
            (Phone.bookFlightDatePrompt destination.Id)
            initialDate

    match date with
    | Some date -> ticketPrompt flightsApp origin.Id destination.Id date
    | None -> flightsApp ()

let private ticketPrompt flightsApp origin destination selectedDate =
    let availableTickets =
        Flights.TicketGeneration.ticketsAvailable
            origin
            destination
            selectedDate

    let selectedTicket =
        showOptionalChoicePrompt
            Phone.chooseFlightPrompt
            Generic.back
            Phone.flightInformation
            availableTickets

    match selectedTicket with
    | Some ticket -> confirmPurchase flightsApp ticket
    | None -> flightsApp ()

let private confirmPurchase app ticket =
    let confirm =
        showConfirmationPrompt (ticket |> Phone.flightPurchaseConfirmation)

    if confirm then
        purchaseTicket app ticket
    else
        ticketPrompt app ticket.Origin ticket.Destination ticket.Date

let private purchaseTicket app ticket =
    let bookingResult = Flights.Booking.bookFlight (State.get ()) ticket

    match bookingResult with
    | Ok effects -> Effect.applyMultiple effects
    | Error(NotEnoughFunds amount) ->
        Phone.flightsNotEnoughFunds amount |> showMessage

    app ()
