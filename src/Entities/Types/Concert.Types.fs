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

    /// Holds all concerts scheduled by all bands in the game.
    type ConcertsByBand = Map<BandId, Schedule<Concert>>
