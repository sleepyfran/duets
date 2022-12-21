module Simulation.Queries.Flights

open Aether
open Entities
open Simulation

/// Retrieves a tuple containing all flights booked. First element is past bookings
/// (those that are passed) and the second is future bookings (any element whose
/// date is more or equal to today).
let all state =
    let today = Queries.Calendar.today state

    Optic.get Lenses.State.flights_ state
    |> List.partition (fun flight ->
        flight.Date < Calendar.Transform.resetDayMoment today
        || flight.AlreadyUsed)

/// Retrieves all the flights booked in the month of the given date.
let forMonth state date =
    let today =
        Queries.Calendar.today state |> Calendar.Transform.resetDayMoment

    let dateMonth = date |> Calendar.Query.firstDayOfNextMonth

    Optic.get Lenses.State.flights_ state
    |> List.filter (fun flight ->
        flight.Date >= today
        && flight.Date < dateMonth
        && not flight.AlreadyUsed)

/// Retrieves any flight that is currently possible for the character to board.
let availableForBoarding state =
    let currentDate = Queries.Calendar.today state

    Optic.get Lenses.State.flights_ state
    |> List.filter (fun flight -> not flight.AlreadyUsed)
    |> List.tryFind (fun flight ->
        let earliestDayMomentToBoard =
            flight.DayMoment |> Calendar.Query.previousDayMoment

        let earliestBoardingDate =
            flight.Date
            |> Calendar.Transform.changeDayMoment earliestDayMomentToBoard

        let latestBoardingDate =
            flight.Date |> Calendar.Transform.changeDayMoment flight.DayMoment

        earliestBoardingDate = currentDate || latestBoardingDate = currentDate)
