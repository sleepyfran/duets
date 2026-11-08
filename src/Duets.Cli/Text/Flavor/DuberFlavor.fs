namespace Duets.Cli.Text.Flavor

open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module Duber =
    let private weatherTalk weather =
        match weather with
        | WeatherCondition.Sunny ->
            [ "Beautiful day, huh? Days like this I almost don't mind sitting in traffic."
              "With this sun I should've been a convertible driver, not a Duber one." ]
        | WeatherCondition.Cloudy ->
            [ "Looks like it might rain later. Hope you've got an umbrella."
              "Grey skies again. At least it keeps the tourists indoors." ]
        | WeatherCondition.Rainy ->
            [ "Rain is great for business, everybody wants a ride. Terrible for my wipers though."
              "Careful when you get out, the puddles around here are deeper than they look." ]
        | WeatherCondition.Stormy ->
            [ "Wild weather today! I almost called it a day before your ride came in."
              "Hang on, the wind on this stretch likes to push the car around a little." ]
        | WeatherCondition.Snowy ->
            [ "Snow! The whole city forgets how to drive the second a flake hits the ground."
              "Don't worry, I've got winter tires. Not everybody on the road does, though." ]

    let private timeTalk phase =
        match phase with
        | SceneCaptions.Dawn ->
            [ "You're up early! I'm on my third coffee already."
              "Early ride, huh? Best time to drive, the streets are empty." ]
        | SceneCaptions.Day ->
            [ "Traffic's not too bad right now, we should be quick."
              "Mind if I take a shortcut? The main road is a nightmare at this hour." ]
        | SceneCaptions.Dusk ->
            [ "Rush hour, sorry. Everybody is trying to get home at the same time."
              "Heading out for the evening? Best time of the day in this city, if you ask me." ]
        | SceneCaptions.Dark ->
            [ "Late night, huh? You wouldn't believe the stories I collect on this shift."
              "Night shifts are the best. Quiet roads, good music, interesting passengers." ]

    let private destinationTalk destinationName cityName =
        [ $"{destinationName}, huh? I've dropped a lot of people there this week."
          $"I know {destinationName}, great spot. I'll get you as close to the door as I can."
          $"Been driving in {cityName} for years and I still find streets I've never seen."
          $"You're not from around here? {cityName} is great once you figure it out." ]

    let private smallTalk =
        [ "Feel free to pick the music, the aux cable's right there."
          "You look familiar... you're not in a band, are you?"
          "Five stars if you liked the ride, I've got a perfect rating to protect!"
          "My cousin plays the drums, says it's the hardest job in the world. Not sure I believe him." ]

    /// Returns a line of small talk said by a Duber driver while driving to the
    /// given destination.
    let driverConversation state destinationName =
        let currentDate = state |> Queries.Calendar.today
        let currentCity = state |> Queries.World.currentCity
        let weather = state |> Queries.World.currentWeather
        let phase = SceneCaptions.phaseOf currentDate.DayMoment

        Common.pickFrom
            [ weatherTalk weather
              timeTalk phase
              destinationTalk destinationName (Generic.cityName currentCity.Id)
              smallTalk ]
