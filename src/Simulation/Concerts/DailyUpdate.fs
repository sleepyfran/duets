module Simulation.Concerts.DailyUpdate

open Aether
open Common
open Entities
open Simulation

let private ticketPriceModifier band concert =
    let ticketPrice = concert.TicketPrice / 1<dd> |> float

    let ticketPriceCap =
        match band.Fame with
        | fame when fame < 15 -> 10.0
        | fame when fame < 30 -> 25.0
        | fame when fame < 60 -> 75.0
        | fame when fame < 80 -> 100.0
        | _ -> 150.0

    if (ticketPriceCap + 3.0) < ticketPrice then
        // If the price is only slightly outside of the price cap it means
        // that the user put a higher ticket price that people are willing to pay.
        // Try to reduce the modifier as close to zero as possible the bigger
        // the cap becomes.
        // Example: Cap 10, price 12 -> 0.48225309
        (ticketPriceCap / ticketPrice) ** 7.0
    else if ticketPriceCap > ticketPrice then
        // If the price is still inside the price cap, then the user put a fair
        // price for the ticket. Reduce the modifier at a normal pace until it
        // gets too close to the cap.
        // Example: Cap 25, price 22 -> 0.92243721
        1.0 - (ticketPrice / ticketPriceCap) ** 20.0
    else
        // If the price is the same as the price cap or just slightly up, reduce
        // until right before the cap was surpassed.
        let adaptedPrice =
            ticketPrice - (ticketPrice - ticketPriceCap) - 1.0

        1.0 - (adaptedPrice / ticketPriceCap) ** 20.0

let private dailyTicketSell state concert attendanceCap =
    let today = Queries.Calendar.today state
    let daysUntilConcert = concert.Date - today

    attendanceCap / float daysUntilConcert.Days

let private concertDailyUpdate state concert =
    let currentBand = Queries.Bands.currentBand state
    let concertInfo = Queries.Concerts.info state concert
    let ticketPriceModifier = ticketPriceModifier currentBand concert
    let lastVisitModifier = 1.0 // TODO: Implement.

    let attendanceCap =
        (float currentBand.Fame / 100.0)
        * (float concertInfo.Venue.Capacity)
        * lastVisitModifier
        * ticketPriceModifier

    let dailyTicketsSold =
        dailyTicketSell state concert attendanceCap
        |> Math.roundToNearest

    Optic.map
        Lenses.Concerts.ticketsSold_
        (fun soldTickets ->
            min (concertInfo.Venue.Capacity) (soldTickets + dailyTicketsSold))
        concert
    |> Tuple.two currentBand
    |> ConcertUpdated

let dailyUpdate state =
    let currentBand = Queries.Bands.currentBand state

    Queries.Concerts.allScheduled state currentBand.Id
    |> Set.fold
        (fun acc concert -> acc @ [ concertDailyUpdate state concert ])
        []
