namespace Duets.Entities


[<AutoOpen>]
module WeatherTypes =
    /// Represents difference weather conditions that can happen in the game.
    [<RequireQualifiedAccess>]
    type WeatherCondition =
        | Sunny
        | Cloudy
        | Rainy
        | Stormy
        | Snowy

    /// Maps a city ID to its current weather condition.
    type WeatherConditionPerCity = Map<CityId, WeatherCondition>

    /// Defines the transition probabilities between different weather conditions
    /// in a specific city, depending on the season.
    type CityWeatherTransitionMatrix =
        { AutumnWinter: Map<WeatherCondition, (WeatherCondition * float) list>
          SpringSummer: Map<WeatherCondition, (WeatherCondition * float) list> }
