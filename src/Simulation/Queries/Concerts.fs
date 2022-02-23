module Simulation.Queries.Concerts

open Entities

/// Retrieves the complete information of a concert, which basically resolves
/// the ID that are given inside of the `CityId` and `VenueId` fields.
let info state concert =
    let concertCity = World.cityById state concert.CityId |> Option.get

    let concertVenue =
        World.concertSpaceById state concert.CityId concert.VenueId
        |> Option.get

    {| Id = concert.Id
       Date = concert.Date
       DayMoment = concert.DayMoment
       City = concertCity
       Venue = concertVenue
       TicketPrice = concert.TicketPrice
       TicketsSold = concert.TicketsSold |}
