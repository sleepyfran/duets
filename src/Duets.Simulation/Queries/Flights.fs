module Duets.Simulation.Queries.Flights

open Aether
open Duets.Entities
open Duets.Simulation
open FSharp.Data.UnitSystems.SI.UnitNames

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

/// Calculates the total amount of minutes that it'd take to complete the given
/// flight.
let flightTime flight =
    Queries.World.distanceBetween flight.Origin flight.Destination
    |> (*) 8<second / km>

/// Calculates the total amount of day moments that it'd take to complete the
/// given flight.
let flightDayMoments flight =
    let flightTime = flightTime flight
    let dayMomentPerHourInSeconds = 3600<second>

    flightTime / dayMomentPerHourInSeconds |> (*) 1<dayMoments>
