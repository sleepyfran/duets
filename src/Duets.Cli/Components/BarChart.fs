[<AutoOpen>]
module Duets.Cli.Components.BarChart

open Spectre.Console

/// Returns the associated color given the level of a skill or the quality
/// of a song.
let private colorForLevel level =
    match level with
    | level when level < 30 -> Color.Red
    | level when level < 60 -> Color.Orange1
    | level when level < 80 -> Color.Green
    | _ -> Color.Blue

/// <summary>
/// Shows a bar chart with a max value of 100.
/// </summary>
/// <param name="items">List of tuples of value and text to display</param>
let showBarChart items =
    let mutable barChart = BarChart()
    barChart.MaxValue <- 100.0

    barChart <-
        barChart.AddItems(
            items,
            fun (progress, label) ->
                BarChartItem(label, float progress, colorForLevel progress)
        )

    AnsiConsole.Write(barChart)
