namespace Duets.Entities

[<AutoOpen>]
module CityTypes =
    /// ID for a country in the game world, which declares every possible country
    /// available in the game.
    type CountryId = UnitedStates

    /// ID for a city in the game world, which declares every possible city
    /// available in the game.
    type CityId = LosAngeles

    /// A descriptor that can be used to create descriptions for a specific
    /// zone.
    type ZoneDescriptor =
        | Bohemian
        | BusinessDistrict
        | Creative
        | Coastal
        | EntertainmentHeart
        | Glitz
        | Luxurious
        | Nature
