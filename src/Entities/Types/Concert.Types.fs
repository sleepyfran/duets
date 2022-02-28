namespace Entities

[<AutoOpen>]
module ConcertTypes =
    /// Unique identifier of a concert.
    type ConcertId = Identity

    /// Represents a single concert in a venue.
    [<CustomEquality; CustomComparison>]
    type Concert =
        { Id: ConcertId
          CityId: CityId
          VenueId: NodeId
          Date: Date
          DayMoment: DayMoment
          TicketPrice: Amount
          TicketsSold: int }
        override x.Equals(obj) =
            match obj with
            | :? Concert as c -> (x.Id = c.Id)
            | _ -> false

        override x.GetHashCode() = hash x.Id

        interface System.IComparable with
            member x.CompareTo(o) =
                match o with
                | :? Concert as c -> compare x.Id c.Id
                | _ -> -1

    /// Defines a timeline of concerts as two lists: one for the events that
    /// have already happened and another for the ones that will happen in the
    /// future.
    type ConcertTimeline =
        { PastEvents: Set<Concert>
          FutureEvents: Set<Concert> }

    /// Holds all concerts scheduled by all bands in the game.
    type ConcertsByBand = Map<BandId, ConcertTimeline>
