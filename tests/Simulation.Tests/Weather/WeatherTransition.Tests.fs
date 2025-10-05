module Duets.Simulation.Tests.Weather.Transition

open Test.Common
open Test.Common.Generators
open NUnit.Framework
open FsUnit

open Aether
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Weather.Transition

let private staticDoubleRandom value =
    { new System.Random() with
        override this.NextDouble() = value }
    |> RandomGen.change

    new RandomGenDisposable()

let createStateWith cityId weather season =
    let home =
        Queries.World.placesByTypeInCity cityId PlaceTypeIndex.Home |> List.head

    State.generateOne State.defaultOptions
    |> Optic.set Lenses.State.currentPosition_ (cityId, home.Id, "")
    |> Optic.set (Lenses.FromState.Weather.forCity_ cityId) weather
    |> Optic.set
        Lenses.State.today_
        { Season = season
          Day = 1<days>
          Year = 2025<years>
          DayMoment = EarlyMorning }

[<Test>]
let ``Los Angeles - Sunny to Cloudy transition`` () =
    use _ = staticDoubleRandom 0.95

    let state = createStateWith LosAngeles WeatherCondition.Sunny Summer

    let effects = dailyWeatherUpdate state

    effects
    |> should
        contain
        (WeatherChanged(
            LosAngeles,
            Diff(WeatherCondition.Sunny, WeatherCondition.Cloudy)
        ))

[<Test>]
let ``Madrid - Stormy to Cloudy in winter`` () =
    use _ = staticDoubleRandom 0.4

    let state = createStateWith Madrid WeatherCondition.Stormy Winter

    let effects = dailyWeatherUpdate state

    effects
    |> should
        contain
        (WeatherChanged(
            Madrid,
            Diff(WeatherCondition.Stormy, WeatherCondition.Cloudy)
        ))

[<Test>]
let ``London - Rainy stays Rainy in autumn`` () =
    use _ = staticDoubleRandom 0.5

    let state = createStateWith London WeatherCondition.Rainy Autumn

    let effects = dailyWeatherUpdate state

    effects
    |> List.filter (function
        | WeatherChanged(cityId, _) -> cityId = London
        | _ -> false)
    |> should haveLength 0

[<Test>]
let ``Prague - Sunny to Snowy in winter`` () =
    use _ = staticDoubleRandom 0.7

    let state = createStateWith Prague WeatherCondition.Sunny Winter

    let effects = dailyWeatherUpdate state

    effects
    |> should
        contain
        (WeatherChanged(
            Prague,
            Diff(WeatherCondition.Sunny, WeatherCondition.Snowy)
        ))

[<Test>]
let ``New York - Rainy to Stormy in spring`` () =
    use _ = staticDoubleRandom 0.7

    let state = createStateWith NewYork WeatherCondition.Rainy Spring

    let effects = dailyWeatherUpdate state

    effects
    |> should
        contain
        (WeatherChanged(
            NewYork,
            Diff(WeatherCondition.Rainy, WeatherCondition.Stormy)
        ))
