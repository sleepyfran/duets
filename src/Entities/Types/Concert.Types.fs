namespace Entities

open Entities

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

    /// Defines how much energy the player will devote to an event in a concert.
    type PerformEnergy =
        | Energetic
        | Normal
        | Limited

    /// Defines all events that can happen in a concert.
    type ConcertEvent =
        | PlaySong of song: Song * energy: PerformEnergy
        | DedicateSong
        | GreetAudience
        | GiveSpeech
        | FaceBand
        | GotOffStage
        | PerformedEncore
        | TuneInstrument
        | GuitarSolo
        | BassSolo
        | TakeMic
        | PutMicOnStand
        | AdjustDrums
        | SpinDrumsticks
        | DrumSolo
        | MakeCrowdSing

    /// Represents a concert that is currently ongoing, where we keep all the
    /// events that the player have done during the concert and the total amount
    /// of points gathered.
    type OngoingConcert =
        { Events: ConcertEvent list
          Points: Quality
          Concert: Concert }

    /// Represents a concert that hasn't happened yet.
    type ScheduledConcert = ScheduledConcert of Concert

    /// Represents a concert that has either been successfully performed, with
    /// the quality of the concert associated, or a concert that failed for
    /// one reason or another.
    type PastConcert =
        | PerformedConcert of Concert * Quality
        | FailedConcert of Concert

    /// Defines a timeline of concerts as two lists: one for the events that
    /// have already happened and another for the ones that will happen in the
    /// future.
    type ConcertTimeline =
        // TODO: Consider moving into simple lists since we're not really using any nicety from the Set module.
        { ScheduledEvents: Set<ScheduledConcert>
          PastEvents: Set<PastConcert> }

    /// Holds all concerts scheduled by all bands in the game.
    type ConcertsByBand = Map<BandId, ConcertTimeline>
