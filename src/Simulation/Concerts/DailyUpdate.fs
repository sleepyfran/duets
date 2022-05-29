module Simulation.Concerts.DailyUpdate

open Aether
open Common
open Entities
open Simulation

let private ticketPriceModifier (band: Band) concert =
    let ticketPrice =
        concert.TicketPrice / 1<dd> |> float

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

let private lastVisitModifier state (band: Band) concert =
    let lastConcertInCity =
        Queries.Concerts.lastConcertInCity state band.Id concert.CityId

    match lastConcertInCity with
    | Some lastConcert ->
        let lastConcert =
            match lastConcert with
            | PerformedConcert (concert, _) -> concert
            | FailedConcert concert -> concert

        concert.Date - lastConcert.Date
        |> fun span ->
            match span.Days with
            | days when days < 30 -> 0.2
            | days when days < 180 -> 0.7
            | _ -> 1.0
    | None -> 1.0

let private dailyTicketSell state concert attendanceCap =
    let today = Queries.Calendar.today state

    let daysUntilConcert =
        (concert.Date - today).Days |> max 1

    attendanceCap / float daysUntilConcert

let private concertDailyUpdate state concert =
    let currentBand =
        Queries.Bands.currentBand state

    let innerConcert =
        Concert.fromScheduled concert

    let (_, venue) =
        Queries.World.ConcertSpace.byId innerConcert.CityId innerConcert.VenueId
        |> Option.get

    let ticketPriceModifier =
        ticketPriceModifier currentBand innerConcert

    let lastVisitModifier =
        lastVisitModifier state currentBand innerConcert

    let attendanceCap =
        (float currentBand.Fame / 100.0)
        * (float venue.Capacity)
        * lastVisitModifier
        * ticketPriceModifier

    let dailyTicketsSold =
        dailyTicketSell state innerConcert attendanceCap
        |> Math.roundToNearest

    Optic.map
        Lenses.Concerts.Scheduled.ticketsSold_
        (fun soldTickets -> min venue.Capacity (soldTickets + dailyTicketsSold))
        concert
    |> Tuple.two currentBand
    |> ConcertUpdated

let dailyUpdate state =
    let currentBand =
        Queries.Bands.currentBand state

    Queries.Concerts.allScheduled state currentBand.Id
    |> Set.fold
        (fun acc concert -> acc @ [ concertDailyUpdate state concert ])
        []
