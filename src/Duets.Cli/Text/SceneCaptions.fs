[<RequireQualifiedAccess>]
module Duets.Cli.Text.SceneCaptions

open Duets.Entities

/// Part of the day that changes how the street feels, coarser than a
/// `DayMoment`.
type DayPhase =
    | Dawn
    | Day
    | Dusk
    | Dark

/// Returns the phase of the day in which the given day moment falls.
let phaseOf dayMoment =
    match dayMoment with
    | EarlyMorning -> Dawn
    | Morning
    | Midday
    | Afternoon -> Day
    | Evening -> Dusk
    | Night
    | Midnight -> Dark

/// Opening clauses of the caption, describing the sky and the light. They
/// have no punctuation at the end since they're followed by an activity.
let atmosphere weather phase =
    match weather, phase with
    | WeatherCondition.Sunny, Dawn ->
        [ "The first light creeps over the rooftops"
          "A pale sunrise paints the windows orange"
          "The street wakes up under a clear sky" ]
    | WeatherCondition.Sunny, Day ->
        [ "Sunlight warms the façades"
          "The sun bounces off the shop windows"
          "A bright sky hangs over the rooftops" ]
    | WeatherCondition.Sunny, Dusk ->
        [ "The sunset stretches long shadows down the street"
          "Golden light fades from the rooftops"
          "The last of the sun lingers on the windows" ]
    | WeatherCondition.Sunny, Dark ->
        [ "Streetlights hum under a clear night sky"
          "The moon hangs above the rooftops"
          "Stars peek out between the buildings" ]
    | WeatherCondition.Cloudy, Dark ->
        [ "Streetlights glow against a starless sky"
          "Low clouds swallow the glow of the city" ]
    | WeatherCondition.Cloudy, _ ->
        [ "Grey clouds hang low over the street"
          "A flat, grey light settles over the façades"
          "The sky is a sheet of grey" ]
    | WeatherCondition.Rainy, Dark ->
        [ "Rain glistens under the streetlights"
          "Neon signs ripple in the wet pavement" ]
    | WeatherCondition.Rainy, _ ->
        [ "Rain drums on the awnings"
          "Puddles gather along the curb"
          "A steady drizzle slicks the pavement" ]
    | WeatherCondition.Stormy, _ ->
        [ "Thunder rolls over the rooftops"
          "Wind whips the rain down the street"
          "Lightning flashes behind the buildings" ]
    | WeatherCondition.Snowy, Dark ->
        [ "Snow drifts through the glow of the streetlights"
          "The snow muffles the night" ]
    | WeatherCondition.Snowy, _ ->
        [ "A thin layer of snow covers the sidewalk"
          "Snow muffles the sound of the city"
          "Snowflakes swirl between the buildings" ]

/// Closing clauses of the first sentence, describing how busy the street is.
let activity peopleCount =
    match peopleCount with
    | 0 ->
        [ " and nobody else is around."
          ", and the sidewalk is deserted."
          ", and the street is all yours." ]
    | 1 ->
        [ " while someone hurries past."
          " as a lone passer-by strolls along." ]
    | count when count <= 3 ->
        [ " as a few people walk by."
          " as a couple of passers-by stroll along." ]
    | _ ->
        [ " while people bustle along the sidewalk."
          " as a crowd fills the sidewalk."
          " and the street is packed with people." ]

/// Optional second sentence about one of the places in the street.
let nightlife placeName =
    [ $"Music drifts out of {placeName}."
      $"A muffled bassline leaks from {placeName}."
      $"Laughter spills out of {placeName}." ]

let food placeName =
    [ $"The smell of food wafts from {placeName}."
      $"Clinking cutlery echoes out of {placeName}." ]

let coffee placeName =
    [ $"The smell of coffee comes from {placeName}."
      $"A queue for coffee forms outside {placeName}." ]

let allClosed =
    [ "Every shutter on the street is down."
      "All the shops have called it a day." ]
