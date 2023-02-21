namespace Duets.Entities

[<AutoOpen>]
module RentalTypes =
    /// Defines when the rental has to be paid. Monthly is for rentals like a
    /// flat, where a fee has to be paid repeatedly each month. OneTime is for
    /// rentals like a hotel room, where there's just one payment.
    type RentalType =
        | Monthly of nextPaymentDate: Date
        | OneTime of until: Date

    /// Defines a rental that the character holds over some place.
    type Rental =
        { Amount: Amount
          Coords: WorldCoordinates
          RentalType: RentalType }

    /// Associates the rentals that the character currently holds, grouped by
    /// their location in the world.
    type CharacterRentals = Map<WorldCoordinates, Rental>
