module Entities.CalendarEvent

/// Returns the date of the event
let date eventType =
    match eventType with
    | CalendarEventType.Flight flight -> flight.Date
    | CalendarEventType.Concert concert -> concert.Date
