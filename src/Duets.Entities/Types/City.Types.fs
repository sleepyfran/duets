namespace Duets.Entities

[<AutoOpen>]
module CityTypes =
    /// ID for a country in the game world, which declared every possible country
    /// available in the game.
    type CountryId =
        | Canada
        | Chile
        | CzechRepublic
        | England
        | France
        | Germany
        | Japan
        | SouthKorea
        | Spain
        | UnitedStates

    /// ID for a city in the game world, which declared every possible city
    /// available in the game.
    type CityId =
        | Berlin
        | London
        | LosAngeles
        | Madrid
        | NewYork
        | Paris
        | Prague
        | Santiago
        | Seoul
        | Tokyo
        | Toronto
