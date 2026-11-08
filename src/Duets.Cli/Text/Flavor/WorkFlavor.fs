namespace Duets.Cli.Text.Flavor

open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module Work =
    let private arrival weather phase placeName =
        match weather, phase with
        | WeatherCondition.Rainy, _
        | WeatherCondition.Stormy, _ ->
            [ $"Shaking the rain off your jacket, you slip in through the back door of {placeName}."
              $"You arrive at {placeName} soaked from the rain and hang your coat to dry." ]
        | WeatherCondition.Snowy, _ ->
            [ $"You stomp the snow off your boots at the entrance of {placeName}."
              $"Cheeks red from the cold, you step into the warmth of {placeName}." ]
        | _, SceneCaptions.Dawn ->
            [ $"You unlock the door of {placeName} while the rest of the city is still yawning."
              $"Still half asleep, you put on your apron at {placeName}." ]
        | _, SceneCaptions.Day ->
            [ $"You clock in at {placeName}, where the day is already in full swing."
              $"A quick nod to your coworkers and you're ready to start your shift at {placeName}." ]
        | _, SceneCaptions.Dusk ->
            [ $"The evening crowd is starting to arrive at {placeName} as you clock in."
              $"You arrive at {placeName} just as the lights are being dimmed for the evening." ]
        | _, SceneCaptions.Dark ->
            [ $"The night shift at {placeName} begins, and you already know it's going to be a long one."
              $"You clock in at {placeName}, ready for whatever the night brings." ]

    /// Returns a description of the player arriving at their workplace to
    /// start a shift.
    let shiftArrival state =
        let currentDate = state |> Queries.Calendar.today
        let currentPlace = state |> Queries.World.currentPlace
        let weather = state |> Queries.World.currentWeather
        let phase = SceneCaptions.phaseOf currentDate.DayMoment

        Common.pickFrom [ arrival weather phase currentPlace.Name ]
