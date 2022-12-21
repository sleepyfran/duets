namespace Entities

[<AutoOpen>]
module CalendarEventTypes =
    /// Represents a calendar event.
    [<RequireQualifiedAccess>]
    type CalendarEventType =
        | Flight of Flight
        | Concert of Concert
