namespace Duets.Entities

[<AutoOpen>]
module FlightTypes =
    /// Defines a flight ticket that the character holds to travel between
    /// two cities in the game.
    type Flight =
        { Id: Identity
          Origin: CityId
          Destination: CityId
          Price: Amount
          Date: Date
          DayMoment: DayMoment
          AlreadyUsed: bool }
