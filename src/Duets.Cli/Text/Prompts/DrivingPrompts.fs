namespace Duets.Cli.Text.Prompts

open Duets.Cli.Text
open Duets.Simulation

[<RequireQualifiedAccess>]
module Driving =
    let createWithinCityDrivingMomentPrompt state destinationName car =
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

    let createIntercityDrivingMomentPrompt
        state
        originCityId
        destinationCityId
        car
        =
        let currentDate = state |> Queries.Calendar.today
        let character = state |> Queries.Characters.playableCharacter
        let originCityName = originCityId |> Generic.cityName
        let destinationCityName = destinationCityId |> Generic.cityName

        let distance =
            Duets.Data.World.World.distanceBetween
                originCityId
                destinationCityId

        Common.createPrompt
            $"""
You are narrating a long-distance road trip moment for {character.Name}, driving from {originCityName} to {destinationCityName} (approximately {distance} km) on their {Common.itemNameForPrompt car}.

Rules:
- Generate a brief, immersive observation (1-2 sentences) about what they experience during this intercity journey in third person.
- **MUST explicitly mention BOTH {originCityName} and {destinationCityName}** in the narration (e.g., "leaving {originCityName} behind", "approaching {destinationCityName}", "the road from {originCityName} to {destinationCityName}", etc.)
- Focus on the SPECIFIC geographic route between these two cities: consider the real-world landscape, terrain, culture, and landmarks between {originCityName} and {destinationCityName}.
- This could reference: highway scenery specific to this route, regional landscapes (mountains, plains, coastlines, forests), crossing borders if applicable, notable regions or areas passed through, changing architecture or culture, road signs mentioning these cities, rest stops along this specific route, weather changes across the regions, or reflective thoughts about the journey between these two specific places.
- This is a LONG TRIP between major cities on highways/motorways, not city streets.
- Match the tone to the current time, season, and the geographic/cultural characteristics of the route from {originCityName} to {destinationCityName}.
- Keep it realistic and grounded - this is a text adventure, so make it evocative but not overly dramatic.
- **Do not** use quotation marks, asterisks, or any formatting.
- **Do not** include labels or headers - just the observation itself.

Context:
- Time: {currentDate.DayMoment} in {currentDate.Season}
- Origin City: {originCityName}
- Destination City: {destinationCityName}
- Distance: {distance} km
- Character: {character.Name}
- Route: From {originCityName} to {destinationCityName}

Generate the intercity driving moment (MUST mention both cities):
"""
