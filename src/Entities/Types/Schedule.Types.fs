namespace Entities

open Entities

[<AutoOpen>]
module ScheduleTypes =
    /// Represents all different types of events that can be scheduled in
    /// the game.
    type ScheduledEvent = Concert of ConcertId

    /// Represents all the scheduled events by date.
    type ScheduledEvents = Map<Date, ScheduledEvent list>
