namespace Duets.Cli.Text.Prompts

open Duets.Cli.Text
open Duets.Simulation

[<RequireQualifiedAccess>]
module Driving =
    let createDrivingMomentPrompt state destinationName car =
        let currentCity = state |> Queries.World.currentCity
        let currentDate = state |> Queries.Calendar.today
        let currentWeather = state |> Queries.World.currentWeather
        let character = state |> Queries.Characters.playableCharacter

        Common.createPrompt
            $"""
You are narrating a driving moment for {character.Name}, driving through {currentCity.Id |> Generic.cityName} to {destinationName} on their {Common.itemNameForPrompt car}.

Rules:
- Generate a brief, immersive observation (1-2 sentences) about what they notice while driving in third person.
- This could be about: the car they're driving, the scenery, weather effects, other drivers, pedestrians, billboards, radio snippets, road conditions, street life, city atmosphere, or their own thoughts.
- Match the tone to the current time and weather and car type.
- Keep it realistic and grounded - this is a text adventure, so make it evocative but not overly dramatic.
- **Do not** use quotation marks, asterisks, or any formatting.
- **Do not** include labels or headers - just the observation itself.
- Make it feel like a natural moment that adds flavor to the journey.

Context:
- Time: {currentDate.DayMoment} in {currentDate.Season}
- Weather: {currentWeather}
- City: {currentCity.Id |> Generic.cityName}
- Destination: {destinationName}

Generate the driving moment:
"""
