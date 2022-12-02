module rec Simulation.Flights.TicketGeneration

open Entities
open Simulation

/// Generates the tickets available for a given date between two cities.
let ticketsAvailable origin destination date =
    (* Day moments in which they can fly. *)
    [ Morning; Midday; Afternoon ]
    |> List.map (createTicket origin destination date)

let private createTicket origin destination date dayMoment =
    let ticketPrice =
        Queries.World.distanceBetween origin destination
        |> (*) Config.Travel.pricePerKm
        |> (*) 1<dd / km>

    Flight.create origin destination ticketPrice date dayMoment
