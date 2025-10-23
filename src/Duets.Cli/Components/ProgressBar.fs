[<AutoOpen>]
module Duets.Cli.Components.ProgressBar

open FSharp.Data.UnitSystems.SI.UnitNames
open Spectre.Console

let private sleepForProgressBar (stepDuration: int<second>) =
    async { do! Async.Sleep(stepDuration * 1000 / 4 |> int) }
    |> Async.RunSynchronously

/// <summary>
/// Renders a progress bar that shows each of the given step names one at a time
/// waiting for the specified `stepDuration` between each step.
/// </summary>
/// <param name="stepNames">Text of the steps to show</param>
/// <param name="stepDuration">Duration of each of the steps</param>
let showProgressBarSync stepNames (stepDuration: int<second>) =
    AnsiConsole
        .Progress()
        .Start(fun ctx ->
            stepNames
            |> List.iter (fun stepName ->
                let task = ctx.AddTask(stepName)

                for _ in 0..4 do
                    task.Increment 25.0
                    sleepForProgressBar stepDuration))

/// <summary>
/// Renders a progress bar that shows each of the given step names all at the
/// same time, increasing the steps randomly and waiting for the specified
/// `stepDuration` between each step.
/// </summary>
/// <param name="stepNames">Text of the steps to show</param>
/// <param name="stepDuration">Duration of each of the steps</param>
let showProgressBarAsync stepNames (stepDuration: int<second>) =
    AnsiConsole
        .Progress()
        .Start(fun ctx ->
            let tasks =
                stepNames
                |> List.map (fun name -> ctx.AddTask(name))
                |> ResizeArray

            let random = System.Random()

            for _ in 0 .. 4 * tasks.Count - 1 do
                let randomIndex = random.Next(0, tasks.Count)

                let taskToIncrement = tasks.[randomIndex]

                taskToIncrement.Increment 25.0

                if taskToIncrement.IsFinished then
                    tasks.RemoveAt(randomIndex)

                sleepForProgressBar stepDuration)

/// <summary>
/// Renders a progress bar that shows an indeterminate step with the given name
/// until the function completes.
/// </summary>
/// <param name="stepName">Text to display</param>
/// <param name="func">Function that will be immediately executed synchronously and used to determine when to hide the progress</param>
let showProgressForFunc stepName (func: unit -> unit) =
    AnsiConsole
        .Progress()
        .Start(fun ctx ->
            let task = ctx.AddTask(stepName).IsIndeterminate()
            func ()
            task.StopTask())
