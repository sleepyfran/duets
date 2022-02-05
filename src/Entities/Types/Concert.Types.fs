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

    /// Map that holds all the concerts from one specific band.
    type Concerts = Map<ConcertId, Concert>

    /// Map that holds all concerts by all bands in the game.
    type ConcertsByBand = Map<BandId, Concerts>
