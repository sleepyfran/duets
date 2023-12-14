namespace Duets.Entities

[<AutoOpen>]
module CityTypes =
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
