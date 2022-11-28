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
    |> List.partition (fun flight -> flight.Date < today)
