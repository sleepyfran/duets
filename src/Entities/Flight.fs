module Entities.Flight

let create origin destination price date dayMoment =
    { Id = Identity.create ()
      Origin = origin
      Destination = destination
      Price = price
      Date = date
      DayMoment = dayMoment
      AlreadyUsed = false }
