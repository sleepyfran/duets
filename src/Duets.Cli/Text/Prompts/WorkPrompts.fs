namespace Duets.Cli.Text.Prompts

open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module Work =
    let createWorkShiftDescriptionPrompt state (job: Job) =
        let currentPlace = state |> Queries.World.currentPlace
        let currentCity = state |> Queries.World.currentCity
        let currentDate = state |> Queries.Calendar.today
        let currentWeather = state |> Queries.World.currentWeather
        let careerName = Career.careerName job.Id
        let stageName = Career.name job

        Common.createPrompt
            $"""
    You are describing a work shift in a text-based music and life simulation game.
    
    Rules:
    - Generate a single, engaging sentence (1-2 sentences max) describing the work shift the player is about to start.
    - Keep it immersive and relevant to the job type and career stage.
    - Reflect the time of day, weather, and location context when appropriate.
    - Make it feel natural and varied - sometimes humorous, sometimes serious, always contextual.
    - **Do not** use quotation marks, asterisks, or any formatting.
    - **Do not** include labels or prefixes - just the description itself.
    - Keep the tone casual and conversational, as if narrating a game event.
    
    Context:
    - Career: {careerName}
    - Current Position: {stageName}
    - Workplace: {currentPlace.Name} in {currentCity.Id |> Generic.cityName}
    - Time: {currentDate.DayMoment} in {currentDate.Season}
    - Weather: {currentWeather}
    
    Generate the work shift description:
    """
