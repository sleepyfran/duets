module Duets.Simulation.Concerts.DailyUpdate

open Aether
open Duets.Common
open Duets.Entities
open Duets.Simulation

let private ticketPriceModifier bandFame concert =
    let ticketPrice =
        concert.TicketPrice / 1m<dd> |> float

    let ticketPriceCap =
        match bandFame with
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
            Concert.fromPast lastConcert

        concert.Date - lastConcert.Date
        |> fun span ->
            match span.Days with
            | days when days < 30 -> 0.2
            | days when days < 180 -> 0.7
            | _ -> 1.0
    | None -> 1.0

let private dailyTicketSell concert attendanceCap =
    let (ScheduledConcert (concert, dateScheduled)) =
        concert

    let daysUntilConcert =
        (concert.Date - dateScheduled).Days |> max 1

    attendanceCap / float daysUntilConcert

let private concertDailyUpdate state scheduledConcert =
    let currentBand =
        Queries.Bands.currentBand state

    let concert =
        Concert.fromScheduled scheduledConcert

    let venue =
        Queries.World.placeInCityById concert.CityId concert.VenueId
        |> fun place ->
            match place.Type with
            | ConcertSpace concertSpace -> concertSpace
            | _ -> failwith "How did we get here?"

    let bandFame =
        Queries.Bands.estimatedFameLevel state currentBand

    let ticketPriceModifier =
        ticketPriceModifier bandFame concert

    let lastVisitModifier =
        lastVisitModifier state currentBand concert

    let fanAttendanceCap =
        float currentBand.Fans
        * 0.15
        * ticketPriceModifier
        * lastVisitModifier

    let nonFansAttendanceCap =
        (float bandFame / 100.0)
        * (float venue.Capacity)
        * lastVisitModifier
        * ticketPriceModifier

    let dailyTicketsSold =
        dailyTicketSell scheduledConcert nonFansAttendanceCap
        |> (+) (dailyTicketSell scheduledConcert fanAttendanceCap)
        |> Math.roundToNearest

    Optic.map
        Lenses.Concerts.Scheduled.ticketsSold_
        (fun soldTickets -> min venue.Capacity (soldTickets + dailyTicketsSold))
        scheduledConcert
    |> Tuple.two currentBand
    |> ConcertUpdated

let dailyUpdate state =
    let currentBand =
        Queries.Bands.currentBand state

    Queries.Concerts.allScheduled state currentBand.Id
    |> Set.fold
        (fun acc concert -> acc @ [ concertDailyUpdate state concert ])
        []