module Duets.Simulation.Flights.Booking

open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Bank.Operations

let private generatePayment state bill =
    let characterAccount = Queries.Bank.playableCharacterAccount state

    expense state characterAccount bill

/// Books a flight for the current character if they have enough money on their
/// account.
let bookFlight state (flight: Flight) =
    generatePayment state flight.Price
    |> Result.map (fun effects -> [ FlightBooked flight ] @ effects)
