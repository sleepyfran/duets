[<AutoOpen>]
module Cli.Components.ProgressBar

open Cli.Localization
open FSharp.Data.UnitSystems.SI.UnitNames
open Spectre.Console

/// <summary>
/// Renders a progress bar that shows each of the given step names for a given
/// duration in either sync or async way.
/// </summary>
/// <param name="stepNames">Text of the steps to show</param>
/// <param name="stepDuration">Duration of each of the steps</param>
/// <param name="async">
/// Whether the steps can be shown at the same time or only one at a time.
/// </param>
let rec showProgressBar stepNames (stepDuration: int<second>) async =
    if async then
        showProgressBarAsync stepNames stepDuration
    else
        showProgressBarSync stepNames stepDuration

and private showProgressBarSync stepNames stepDuration =
    AnsiConsole
        .Progress()
        .Start(fun ctx ->
            stepNames
            |> List.iter (fun stepName ->
                let task = ctx.AddTask(toString stepName)

                for i in 0..4 do
                    task.Increment 25.0
                    sleepForProgressBar stepDuration))

and private showProgressBarAsync stepNames stepDuration =
    AnsiConsole
        .Progress()
        .Start(fun ctx ->
            let tasks =
                stepNames
                |> List.map (fun name -> ctx.AddTask(toString name))
                |> ResizeArray

            let random = System.Random()

            for i in 0 .. 4 * tasks.Count - 1 do
                let randomIndex = random.Next(0, tasks.Count)
                let taskToIncrement = tasks.[randomIndex]

                taskToIncrement.Increment 25.0

                if taskToIncrement.IsFinished then
                    tasks.RemoveAt(randomIndex)

                sleepForProgressBar stepDuration)

and private sleepForProgressBar stepDuration =
    async { do! Async.Sleep(stepDuration * 1000 / 4 |> int) }
    |> Async.RunSynchronously
