module Duets.Cli.Scenes.Phone.Apps.Weather.Root

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

let private weatherIcon weatherCondition =
    match weatherCondition with
    | WeatherCondition.Sunny -> "☀️"
    | WeatherCondition.Cloudy -> "☁️"
    | WeatherCondition.Rainy -> "🌧️"
    | WeatherCondition.Stormy -> "⛈️"
    | WeatherCondition.Snowy -> "❄️"

let private weatherDescription weatherCondition =
    match weatherCondition with
    | WeatherCondition.Sunny -> "Sunny"
    | WeatherCondition.Cloudy -> "Cloudy"
    | WeatherCondition.Rainy -> "Rainy"
    | WeatherCondition.Stormy -> "Stormy"
    | WeatherCondition.Snowy -> "Snowy"

let weatherApp () =
    let state = State.get ()
    let currentCity = Queries.World.currentCity state
    let currentWeather = Queries.World.currentWeather state

    let icon = weatherIcon currentWeather
    let description = weatherDescription currentWeather

    Phone.weatherAppTitle |> Styles.header |> showMessage
    Phone.weatherAppContent currentCity.Id icon description |> showMessage

    showContinuationPrompt ()

    Scene.Phone
