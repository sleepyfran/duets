[<RequireQualifiedAccess>]
module Duets.Cli.Components.Scene.Sky

open Duets.Cli.Components.Scene.Common
open Duets.Cli.Text
open Duets.Entities

module Styles = SceneStyles.Sky
module Weather = SceneStyles.Weather

/// Draws the sun or the moon depending on the time of the day, as well as the
/// clouds if the weather calls for them.
let draw (grid: SceneGrid.Grid) (lighting: Lighting) =
    let position fraction = int (float (grid.Width - 1) * fraction)

    let glyph, fraction, row, style =
        match lighting.DayMoment with
        | EarlyMorning -> Styles.sun, 0.1, 1, Styles.sunriseColor
        | Morning -> Styles.sun, 0.3, 0, Styles.morningSunColor
        | Midday -> Styles.sun, 0.5, 0, Styles.middaySunColor
        | Afternoon -> Styles.sun, 0.7, 0, Styles.morningSunColor
        | Evening -> Styles.sun, 0.9, 1, Styles.sunsetColor
        | Night -> Styles.moon, 0.3, 0, Styles.moonColor
        | Midnight -> Styles.moon, 0.6, 0, Styles.moonColor

    match lighting.Weather with
    | WeatherCondition.Sunny
    | WeatherCondition.Cloudy ->
        SceneGrid.set grid (position fraction) row glyph (Some style)
    | _ -> ()

    match lighting.Weather with
    | WeatherCondition.Cloudy ->
        for x in 3 .. 22 .. grid.Width - 6 do
            SceneGrid.stamp grid x 0 Styles.cloud (Some Styles.cloudColor)
    | WeatherCondition.Rainy
    | WeatherCondition.Stormy
    | WeatherCondition.Snowy ->
        let overcast = String.replicate grid.Width (string Styles.overcast)
        SceneGrid.stamp grid 0 0 overcast (Some Styles.overcastColor)
    | WeatherCondition.Sunny -> ()

/// Fills the empty cells above the given row with rain, snow or stars,
/// depending on the weather and time of the day.
let drawParticles
    (grid: SceneGrid.Grid)
    (lighting: Lighting)
    groundRow
    seed
    =
    let random = System.Random(seed)

    let particles =
        match lighting.Weather with
        | WeatherCondition.Rainy -> Some(Weather.rain, 0.12, Weather.rainColor)
        | WeatherCondition.Stormy ->
            Some(Weather.storm, 0.15, Weather.stormColor)
        | WeatherCondition.Snowy -> Some(Weather.snow, 0.1, Weather.snowColor)
        | WeatherCondition.Sunny when isNight lighting ->
            Some(Weather.stars, 0.03, Weather.starsColor)
        | _ -> None

    particles
    |> Option.iter (fun (glyphs, density, style) ->
        for y in 0 .. groundRow - 1 do
            for x in 0 .. grid.Width - 1 do
                if
                    SceneGrid.isEmpty grid x y
                    && random.NextDouble() < density
                then
                    let glyph = glyphs[random.Next(glyphs.Length)]
                    SceneGrid.set grid x y glyph (Some style))
