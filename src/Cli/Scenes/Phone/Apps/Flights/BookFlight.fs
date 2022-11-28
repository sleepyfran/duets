module rec Cli.Scenes.Phone.Apps.Flights.BookFlight

open Agents
open Cli
open Cli.Components
open Cli.Text
open Common
open Entities
open Simulation
open Simulation.Bank.Operations
open Simulation.Flights

let bookFlight flightsApp =
    let origin =
        showChoicePrompt
            Phone.bookFlightOriginPrompt
            (fun (city: City) -> Generic.cityName city.Id)
            Queries.World.allCities

    let allCitiesExceptOrigin =
        Queries.World.allCities
        |> List.filter (fun city -> city.Id <> origin.Id)

    let destination =
        showChoicePrompt
            Phone.bookFlightDestinationPrompt
            (fun (city: City) -> Generic.cityName city.Id)
            allCitiesExceptOrigin

    let initialDate =
        Queries.Calendar.today (State.get ())

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
    let bookingResult =
        Booking.bookFlight (State.get ()) ticket

    match bookingResult with
    | Ok effects -> Effect.applyMultiple effects
    | Error (NotEnoughFunds amount) ->
        Phone.flightsNotEnoughFunds amount |> showMessage

    app ()
