namespace Duets.Entities

[<AutoOpen>]
module CityTypes =
    /// ID for a country in the game world, which declared every possible country
    /// available in the game.
    type CountryId =
        | CzechRepublic
        | England
        | Spain
        | UnitedStates

    /// ID for a city in the game world, which declared every possible city
    /// available in the game.
    type CityId =
        | London
        | LosAngeles
        | Madrid
        | NewYork
        | Prague
