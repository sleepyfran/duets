namespace Duets.Entities

open Duets.Entities

[<AutoOpen>]
module ConcertTypes =
    /// Unique identifier of a concert.
    type ConcertId = Identity

    /// Defines the type of participation of a band in a concert.
    type ParticipationType =
        | Headliner
        | OpeningAct of headliner: BandId * earningPercentage: int<percent>

    /// Represents a single concert in a venue.
    [<CustomEquality; CustomComparison>]
    type Concert =
        { Id: ConcertId
          CityId: CityId
          VenueId: PlaceId
          Date: Date
          DayMoment: DayMoment
          TicketPrice: Amount
          TicketsSold: int
          ParticipationType: ParticipationType }

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
    type ScheduledConcert = ScheduledConcert of Concert * scheduledOn: Date

    /// Specifies a reason why a concert failed.
    type FailedConcertReason =
        | BandDidNotMakeIt
        | CharacterPassedOut

    /// Represents a concert that has either been successfully performed, with
    /// the quality of the concert associated, or a concert that failed for
    /// one reason or another.
    type PastConcert =
        | PerformedConcert of Concert * Quality
        | FailedConcert of Concert * FailedConcertReason

    /// Defines a timeline of concerts as two lists: one for the events that
    /// have already happened and another for the ones that will happen in the
    /// future.
    type ConcertTimeline =
        {
            /// List of upcoming concerts. Guaranteed to be sorted by the concert's
            /// date, in ascending order from the closest concert to the furthest.
            ScheduledEvents: ScheduledConcert list

            /// List of concerts that have already happened. Guaranteed to be sorted
            /// by the concert's date, in descending order from the most recent
            /// concert to the oldest.
            PastEvents: PastConcert list
        }

    /// Holds all concerts scheduled by all bands in the game.
    type ConcertsByBand = Map<BandId, ConcertTimeline>
