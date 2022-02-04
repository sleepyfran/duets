module Entities.ConcertContext

let empty =
    { All = Map.empty
      SoldTickets = Map.empty }

let createConcert date cityId venueId ticketPrice =
    { Id = Identity.create ()
      City = cityId
      Venue = venueId
      Date = date
      DayMoment = Night
      TicketPrice = ticketPrice }
