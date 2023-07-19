namespace Duets.Entities

[<AutoOpen>]
module HotelTypes =
    /// Defines a hotel with the price of a room per night.
    type Hotel = { PricePerNight: Amount }
