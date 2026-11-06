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

/// Opening clauses of the room caption, describing how well kept the place
/// is. They have no punctuation at the end since they're followed by an
/// activity.
let roomCondition (quality: Quality) =
    match quality with
    | q when q < 20<quality> ->
        [ "The paint peels off the walls"
          "A damp smell hangs in the air"
          "The floor creaks with every step" ]
    | q when q < 40<quality> ->
        [ "The furniture has seen better days"
          "Scuff marks cover the floor"
          "A flickering bulb buzzes overhead" ]
    | q when q < 60<quality> ->
        [ "The place looks lived in"
          "Nothing here stands out much"
          "The room is plain but tidy" ]
    | q when q < 80<quality> ->
        [ "Everything looks clean and in its place"
          "The room feels warm and well kept"
          "Fresh paint brightens the walls" ]
    | _ ->
        [ "Everything shines like it's brand new"
          "Polished surfaces gleam all around"
          "Not a speck of dust in sight" ]

/// Closing clauses of the first sentence of the room caption, describing how
/// busy the room is.
let roomActivity peopleCount =
    match peopleCount with
    | 0 ->
        [ ", and the room is quiet."
          " and nobody else is around."
          ", and you have the place to yourself." ]
    | 1 ->
        [ ", and someone else is around."
          " and one other person keeps to themselves." ]
    | count when count <= 3 ->
        [ ", and a few people chat quietly."
          " and a couple of people hang around." ]
    | _ ->
        [ ", and the room is buzzing with people."
          " and it's hard to find a free spot."
          ", and voices fill the room." ]

/// Replaces the room caption while the band is on stage, describing how full
/// the venue is (from 0 to 1).
let crowd (fill: float) =
    match fill with
    | fill when fill < 0.2 ->
        [ "Only a handful of people stand in front of the stage."
          "Your voice echoes across a mostly empty room." ]
    | fill when fill < 0.6 ->
        [ "A decent crowd gathers in front of the stage."
          "Heads bob along across half of the room." ]
    | fill when fill < 0.9 ->
        [ "The crowd fills the room, hands in the air."
          "A sea of faces looks up at the stage." ]
    | _ ->
        [ "The venue is packed wall to wall."
          "The crowd roars from the front row to the back." ]

/// Optional second sentence of the room caption, about the weather outside.
/// Only used in rooms with windows.
let throughWindow weather phase =
    match weather, phase with
    | WeatherCondition.Rainy, _ ->
        [ "Rain taps against the windows."
          "Raindrops race down the glass." ]
    | WeatherCondition.Stormy, _ ->
        [ "Thunder rattles the windows."
          "Lightning flashes through the glass." ]
    | WeatherCondition.Snowy, _ ->
        [ "Snowflakes drift past the windows."
          "Frost creeps along the edges of the glass." ]
    | _, Dawn -> [ "The first light of the day comes through the windows." ]
    | WeatherCondition.Sunny, Day -> [ "Sunlight pours through the windows." ]
    | _, Dusk -> [ "The windows glow orange with the sunset." ]
    | _, Dark -> [ "City lights twinkle outside the windows." ]
    | _ -> []
