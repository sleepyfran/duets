namespace Duets.Entities

[<AutoOpen>]
module RentalTypes =
    /// Defines when the rental has to be paid. Seasonal is for rentals like a
    /// flat, where a fee has to be paid repeatedly each season. OneTime is for
    /// rentals like a hotel room, where there's just one payment.
    type RentalType =
        | Seasonal of nextPaymentDate: Date
        | OneTime of from: Date * until: Date

    /// Defines a rental that the character holds over some place.
    type Rental =
        { Amount: Amount
          Coords: PlaceCoordinates
          RentalType: RentalType }

    /// Associates the rentals that the character currently holds, grouped by
    /// their location in the world.
    type CharacterRentals = Map<PlaceCoordinates, Rental>
