module Entities.ConcertContext

type ConcertCreationError = InvalidTicketPrice of price: Amount

/// Validates and creates a concert from the given parameters.
let createConcert date dayMoment cityId venueId ticketPrice =
    let ticketAmount = ticketPrice * 1<dd>

    if ticketAmount <= 0<dd> then
        InvalidTicketPrice ticketAmount |> Error
    else
        Ok
            { Id = Identity.create ()
              CityId = cityId
              VenueId = venueId
              Date = date
              DayMoment = dayMoment
              TicketPrice = ticketAmount
              TicketsSold = 0 }
