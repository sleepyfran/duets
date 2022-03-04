module Entities.Concert

open Entities

type TicketPriceError =
    | PriceBelowZero
    | PriceTooHigh

module Timeline =
    let empty =
        { ScheduledEvents = Set.empty
          PastEvents = Set.empty }

/// Creates a concert from the given parameter.
let create date dayMoment cityId venueId ticketPrice =
    let ticketAmount = ticketPrice * 1<dd>

    { Id = Identity.create ()
      CityId = cityId
      VenueId = venueId
      Date = date
      DayMoment = dayMoment
      TicketPrice = ticketAmount
      TicketsSold = 0 }

/// Validates that the ticket price is not below zero or a too high number.
let validatePrice ticketPrice =
    if ticketPrice < 0 then
        Error PriceBelowZero
    else if ticketPrice > 10000 then
        Error PriceTooHigh
    else
        Ok ticketPrice

/// Returns the inner concert inside a past concert.
let fromPast (concert: PastConcert) =
    match concert with
    | PerformedConcert (concert, _) -> concert
    | FailedConcert concert -> concert

/// Returns the inner concert inside a scheduled concert.
let fromScheduled (ScheduledConcert concert) = concert
