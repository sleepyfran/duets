namespace Entities

[<AutoOpen>]
module ConcertTypes =
    /// Unique identifier of a concert.
    type ConcertId = Identity

    /// Represents a single concert in a venue.
    type Concert =
        { Id: ConcertId
          CityId: CityId
          VenueId: NodeId
          Date: Date
          DayMoment: DayMoment
          TicketPrice: Amount
          TicketsSold: int }

    /// Defines a timeline of concerts as two lists: one for the events that
    /// have already happened and another for the ones that will happen in the
    /// future.
    type ConcertTimeline =
      { PastEvents: Set<Concert>
        FutureEvents: Set<Concert> }

    /// Holds all concerts scheduled by all bands in the game.
    type ConcertsByBand = Map<BandId, ConcertTimeline>
