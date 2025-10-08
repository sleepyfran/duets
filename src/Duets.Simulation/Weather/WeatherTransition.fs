module Duets.Simulation.Weather.Transition

open Aether
open Duets.Entities
open Duets.Simulation

module private CityTransitionMatrix =
    let private losAngelesMatrix =
        Map.ofList
            [ WeatherCondition.Sunny,
              [ WeatherCondition.Sunny, 0.90
                WeatherCondition.Cloudy, 0.08
                WeatherCondition.Rainy, 0.01
                WeatherCondition.Stormy, 0.01
                WeatherCondition.Snowy, 0.0 ]
              WeatherCondition.Cloudy,
              [ WeatherCondition.Sunny, 0.60
                WeatherCondition.Cloudy, 0.30
                WeatherCondition.Rainy, 0.05
                WeatherCondition.Stormy, 0.05
                WeatherCondition.Snowy, 0.0 ]
              WeatherCondition.Rainy,
              [ WeatherCondition.Sunny, 0.40
                WeatherCondition.Cloudy, 0.30
                WeatherCondition.Rainy, 0.15
                WeatherCondition.Stormy, 0.15
                WeatherCondition.Snowy, 0.0 ]
              WeatherCondition.Stormy,
              [ WeatherCondition.Sunny, 0.40
                WeatherCondition.Cloudy, 0.30
                WeatherCondition.Rainy, 0.15
                WeatherCondition.Stormy, 0.15
                WeatherCondition.Snowy, 0.0 ]
              WeatherCondition.Snowy,
              [ WeatherCondition.Sunny, 0.40
                WeatherCondition.Cloudy, 0.30
                WeatherCondition.Rainy, 0.15
                WeatherCondition.Stormy, 0.15
                WeatherCondition.Snowy, 0.0 ] ]

    let private madridMatrix =
        Map.ofList
            [ WeatherCondition.Sunny,

              [ WeatherCondition.Sunny, 0.80
                WeatherCondition.Cloudy, 0.15
                WeatherCondition.Rainy, 0.025
                WeatherCondition.Stormy, 0.025
                WeatherCondition.Snowy, 0.0 ]
              WeatherCondition.Cloudy,

              [ WeatherCondition.Sunny, 0.40
                WeatherCondition.Cloudy, 0.50
                WeatherCondition.Rainy, 0.05
                WeatherCondition.Stormy, 0.05
                WeatherCondition.Snowy, 0.0 ]
              WeatherCondition.Rainy,

              [ WeatherCondition.Sunny, 0.25
                WeatherCondition.Cloudy, 0.35
                WeatherCondition.Rainy, 0.20
                WeatherCondition.Stormy, 0.20
                WeatherCondition.Snowy, 0.0 ]
              WeatherCondition.Stormy,

              [ WeatherCondition.Sunny, 0.25
                WeatherCondition.Cloudy, 0.35
                WeatherCondition.Rainy, 0.20
                WeatherCondition.Stormy, 0.20
                WeatherCondition.Snowy, 0.0 ]
              WeatherCondition.Snowy,

              [ WeatherCondition.Sunny, 0.25
                WeatherCondition.Cloudy, 0.35
                WeatherCondition.Rainy, 0.20
                WeatherCondition.Stormy, 0.20
                WeatherCondition.Snowy, 0.0 ] ]

    let private londonMatrix =
        Map.ofList
            [ WeatherCondition.Sunny,
              [ WeatherCondition.Sunny, 0.30
                WeatherCondition.Cloudy, 0.45
                WeatherCondition.Rainy, 0.125
                WeatherCondition.Stormy, 0.125
                WeatherCondition.Snowy, 0.0 ]
              WeatherCondition.Cloudy,
              [ WeatherCondition.Sunny, 0.25
                WeatherCondition.Cloudy, 0.50
                WeatherCondition.Rainy, 0.125
                WeatherCondition.Stormy, 0.125
                WeatherCondition.Snowy, 0.0 ]
              WeatherCondition.Rainy,
              [ WeatherCondition.Sunny, 0.10
                WeatherCondition.Cloudy, 0.30
                WeatherCondition.Rainy, 0.30
                WeatherCondition.Stormy, 0.30
                WeatherCondition.Snowy, 0.0 ]
              WeatherCondition.Stormy,
              [ WeatherCondition.Sunny, 0.10
                WeatherCondition.Cloudy, 0.30
                WeatherCondition.Rainy, 0.30
                WeatherCondition.Stormy, 0.30
                WeatherCondition.Snowy, 0.0 ]
              WeatherCondition.Snowy,
              [ WeatherCondition.Sunny, 0.10
                WeatherCondition.Cloudy, 0.30
                WeatherCondition.Rainy, 0.30
                WeatherCondition.Stormy, 0.30
                WeatherCondition.Snowy, 0.0 ] ]

    let private pragueWinterMatrix =
        Map.ofList
            [ WeatherCondition.Sunny,
              [ WeatherCondition.Sunny, 0.20
                WeatherCondition.Cloudy, 0.40
                WeatherCondition.Rainy, 0.0
                WeatherCondition.Stormy, 0.0
                WeatherCondition.Snowy, 0.40 ]
              WeatherCondition.Cloudy,
              [ WeatherCondition.Sunny, 0.10
                WeatherCondition.Cloudy, 0.55
                WeatherCondition.Rainy, 0.0
                WeatherCondition.Stormy, 0.0
                WeatherCondition.Snowy, 0.35 ]
              WeatherCondition.Rainy,
              [ WeatherCondition.Sunny, 0.05
                WeatherCondition.Cloudy, 0.20
                WeatherCondition.Rainy, 0.0
                WeatherCondition.Stormy, 0.0
                WeatherCondition.Snowy, 0.75 ]
              WeatherCondition.Stormy,
              [ WeatherCondition.Sunny, 0.05
                WeatherCondition.Cloudy, 0.20
                WeatherCondition.Rainy, 0.0
                WeatherCondition.Stormy, 0.0
                WeatherCondition.Snowy, 0.75 ]
              WeatherCondition.Snowy,
              [ WeatherCondition.Sunny, 0.05
                WeatherCondition.Cloudy, 0.20
                WeatherCondition.Rainy, 0.0
                WeatherCondition.Stormy, 0.0
                WeatherCondition.Snowy, 0.75 ] ]

    let private pragueSummerMatrix =
        Map.ofList
            [ WeatherCondition.Sunny,
              [ WeatherCondition.Sunny, 0.55
                WeatherCondition.Cloudy, 0.30
                WeatherCondition.Rainy, 0.075
                WeatherCondition.Stormy, 0.075
                WeatherCondition.Snowy, 0.0 ]
              WeatherCondition.Cloudy,
              [ WeatherCondition.Sunny, 0.25
                WeatherCondition.Cloudy, 0.50
                WeatherCondition.Rainy, 0.125
                WeatherCondition.Stormy, 0.125
                WeatherCondition.Snowy, 0.0 ]
              WeatherCondition.Rainy,
              [ WeatherCondition.Sunny, 0.10
                WeatherCondition.Cloudy, 0.30
                WeatherCondition.Rainy, 0.30
                WeatherCondition.Stormy, 0.30
                WeatherCondition.Snowy, 0.0 ]
              WeatherCondition.Stormy,
              [ WeatherCondition.Sunny, 0.10
                WeatherCondition.Cloudy, 0.30
                WeatherCondition.Rainy, 0.30
                WeatherCondition.Stormy, 0.30
                WeatherCondition.Snowy, 0.0 ]
              WeatherCondition.Snowy,
              [ WeatherCondition.Sunny, 0.10
                WeatherCondition.Cloudy, 0.30
                WeatherCondition.Rainy, 0.30
                WeatherCondition.Stormy, 0.30
                WeatherCondition.Snowy, 0.0 ] ]

    let private newYorkWinterMatrix =
        Map.ofList
            [ WeatherCondition.Sunny,
              [ WeatherCondition.Sunny, 0.50
                WeatherCondition.Cloudy, 0.30
                WeatherCondition.Rainy, 0.0
                WeatherCondition.Stormy, 0.0
                WeatherCondition.Snowy, 0.20 ]
              WeatherCondition.Cloudy,
              [ WeatherCondition.Sunny, 0.15
                WeatherCondition.Cloudy, 0.45
                WeatherCondition.Rainy, 0.0
                WeatherCondition.Stormy, 0.0
                WeatherCondition.Snowy, 0.40 ]
              WeatherCondition.Rainy,
              [ WeatherCondition.Sunny, 0.10
                WeatherCondition.Cloudy, 0.20
                WeatherCondition.Rainy, 0.0
                WeatherCondition.Stormy, 0.0
                WeatherCondition.Snowy, 0.70 ]
              WeatherCondition.Stormy,
              [ WeatherCondition.Sunny, 0.10
                WeatherCondition.Cloudy, 0.20
                WeatherCondition.Rainy, 0.0
                WeatherCondition.Stormy, 0.0
                WeatherCondition.Snowy, 0.70 ]
              WeatherCondition.Snowy,
              [ WeatherCondition.Sunny, 0.10
                WeatherCondition.Cloudy, 0.20
                WeatherCondition.Rainy, 0.0
                WeatherCondition.Stormy, 0.0
                WeatherCondition.Snowy, 0.70 ] ]

    let private newYorkSummerMatrix =
        Map.ofList
            [ WeatherCondition.Sunny,
              [ WeatherCondition.Sunny, 0.65
                WeatherCondition.Cloudy, 0.25
                WeatherCondition.Rainy, 0.05
                WeatherCondition.Stormy, 0.05
                WeatherCondition.Snowy, 0.0 ]
              WeatherCondition.Cloudy,
              [ WeatherCondition.Sunny, 0.30
                WeatherCondition.Cloudy, 0.45
                WeatherCondition.Rainy, 0.125
                WeatherCondition.Stormy, 0.125
                WeatherCondition.Snowy, 0.0 ]
              WeatherCondition.Rainy,
              [ WeatherCondition.Sunny, 0.10
                WeatherCondition.Cloudy, 0.20
                WeatherCondition.Rainy, 0.35
                WeatherCondition.Stormy, 0.35
                WeatherCondition.Snowy, 0.0 ]
              WeatherCondition.Stormy,
              [ WeatherCondition.Sunny, 0.10
                WeatherCondition.Cloudy, 0.20
                WeatherCondition.Rainy, 0.35
                WeatherCondition.Stormy, 0.35
                WeatherCondition.Snowy, 0.0 ]
              WeatherCondition.Snowy,
              [ WeatherCondition.Sunny, 0.10
                WeatherCondition.Cloudy, 0.20
                WeatherCondition.Rainy, 0.35
                WeatherCondition.Stormy, 0.35
                WeatherCondition.Snowy, 0.0 ] ]

    let transitionMatrix cityId : CityWeatherTransitionMatrix =
        match cityId with
        | London ->
            { AutumnWinter = londonMatrix
              SpringSummer = londonMatrix }
        | LosAngeles ->
            { AutumnWinter = losAngelesMatrix
              SpringSummer = losAngelesMatrix }
        | Madrid ->
            { AutumnWinter = madridMatrix
              SpringSummer = madridMatrix }
        | NewYork ->
            { AutumnWinter = newYorkWinterMatrix
              SpringSummer = newYorkSummerMatrix }
        | Prague ->
            { AutumnWinter = pragueWinterMatrix
              SpringSummer = pragueSummerMatrix }

let private updateWeatherIn cityId state =
    let currentWeather =
        state
        |> Optic.get (Lenses.FromState.Weather.forCity_ cityId)
        |> Option.defaultValue WeatherCondition.Sunny

    let currentDate = Queries.Calendar.today state
    let currentSeason = currentDate.Season

    let transitionMatrix = CityTransitionMatrix.transitionMatrix cityId

    let seasonMatrix =
        match currentSeason with
        | Autumn
        | Winter -> transitionMatrix.AutumnWinter
        | Spring
        | Summer -> transitionMatrix.SpringSummer

    let weatherProbabilities =
        seasonMatrix
        |> Map.tryFind currentWeather
        |> Option.defaultValue List.empty

    let nextWeather =
        RandomGen.weightedRandomChoice weatherProbabilities
        |> Option.defaultValue currentWeather

    if nextWeather <> currentWeather then
        [ WeatherChanged(cityId, Diff(currentWeather, nextWeather)) ]
    else
        []

/// Determines the weather for the next day based on the current weather,
/// the current city, and the current season.
let dailyWeatherUpdate state =
    Queries.World.allCities
    |> List.collect (fun city -> updateWeatherIn city.Id state)
