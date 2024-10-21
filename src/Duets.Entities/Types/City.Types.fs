namespace Duets.Entities

[<AutoOpen>]
module CityTypes =
    /// ID for a country in the game world, which declared every possible country
    /// available in the game.
    type CountryId =
        | Australia
        | CzechRepublic
        | England
        | Japan
        | Mexico
        | Spain
        | UnitedStates

    /// ID for a city in the game world, which declared every possible city
    /// available in the game.
    type CityId =
        | London
        | LosAngeles
        | Madrid
        | MexicoCity
        | NewYork
        | Prague
        | Sydney
        | Tokyo
