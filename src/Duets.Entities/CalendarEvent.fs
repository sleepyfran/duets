module Duets.Entities.CalendarEvent

/// Returns the date and day moment of the event
let date eventType =
    let date, dayMoment =
        match eventType with
        | CalendarEventType.Flight flight -> flight.Date, flight.DayMoment
        | CalendarEventType.Concert concert -> concert.Date, concert.DayMoment

    Calendar.Transform.resetDayMoment date, dayMoment
