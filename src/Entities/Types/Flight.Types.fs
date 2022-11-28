namespace Entities

[<AutoOpen>]
module FlightTypes =
    /// Defines a flight ticket that the character holds to travel between
    /// two cities in the game.
    type Flight =
        { Origin: CityId
          Destination: CityId
          Price: int<dd>
          Date: Date
          DayMoment: DayMoment }
