namespace Duets.Cli.Text.Prompts

open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module Flight =
    let createInFlightExperiencePrompt state (flight: Flight) =
        let originCity = flight.Origin |> Generic.cityName
        let destinationCity = flight.Destination |> Generic.cityName
        let character = state |> Queries.Characters.playableCharacter
        let currentDate = state |> Queries.Calendar.today

        Common.createPrompt
            $"""
You are narrating a flight experience for {character.Name}, who is flying from {originCity} to {destinationCity}.

Rules:
- Generate a brief, immersive observation (2-3 sentences) about what they notice during the flight and landing.
- Never use first person in descriptions, describe everything in third person.
- Consider the destination culture for the possible things the character might hear around them.
- This could be about: fellow passengers (their language, appearance, behavior), in-flight service, turbulence, views from the window, sounds, announcements, entertainment, or their own thoughts.
- Match the tone to the time of day: {currentDate.DayMoment} in {currentDate.Season}.
- Keep it realistic and grounded - this is a text adventure, so make it evocative but not overly dramatic.
- **Do not** use quotation marks, asterisks, or any formatting.
- **Do not** include labels or headers - just the observation itself.
- Make it feel like a natural moment that adds flavor to the journey.

Context:
- Origin: {originCity}
- Destination: {destinationCity}
- Time: {currentDate.DayMoment} in {currentDate.Season}

Generate the in-flight and landing moment:
"""

    let createAirportExperiencePrompt state (flight: Flight) =
        let destinationCity = flight.Destination |> Generic.cityName
        let originCity = flight.Origin |> Generic.cityName
        let character = state |> Queries.Characters.playableCharacter

        Common.createPrompt
            $"""
You are narrating the passport control experience for {character.Name} at {destinationCity} airport, arriving from {originCity}.

Rules:
- Generate a brief, immersive observation (1-2 sentences) about going through passport control and going out of the airport.
- Never use first person in descriptions, describe everything in third person.
- This could be about: the queue, immigration officers, security checks, customs declarations, other travelers, signage in local language, airport atmosphere.
- Consider the destination culture and language.
- Keep it realistic and slightly bureaucratic in tone.
- **Do not** use quotation marks, asterisks, or any formatting.
- **Do not** include labels or headers - just the observation itself.

Context:
- Origin: {originCity}
- Destination: {destinationCity}

Generate the passport control and stroll through the airport on the way out moment:
"""
