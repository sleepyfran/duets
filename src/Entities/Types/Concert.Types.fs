namespace Entities

[<AutoOpen>]
module ConcertTypes =
    /// Unique identifier of a concert.
    type ConcertId = Identity

    /// Represents a single concert in a venue.
    type Concert =
        { Id: ConcertId
          City: CityId
          Venue: ConcertSpaceId
          Date: Date
          DayMoment: DayMoment
          TicketPrice: Amount }

    /// Map that holds all the actual concerts.
    type AllConcerts = Map<ConcertId, Concert>

    /// Map that holds the amount of tickets sold for a given concert.
    type ConcertSoldTickets = Map<ConcertId, int>

    type ConcertContext =
        { All: AllConcerts
          SoldTickets: ConcertSoldTickets }
