namespace Cli.Components.Commands

open Cli.Components
open Cli.Components.Commands
open Cli.Text
open Simulation
open Simulation.Concerts.Live

[<RequireQualifiedAccess>]
module MakeCrowdSingCommand =
    /// Command which allows the user make the crowd sing.
    let rec create ongoingConcert =
        Concert.createCommand
            "make crowd sing"
            Command.makeCrowdSingDescription
            makeCrowdSing
            (fun result points ->
                match result with
                | LowPerformance -> Concert.makeCrowdSingLowPerformance points
                | AveragePerformance ->
                    Concert.makeCrowdSingLowPerformance points
                | GoodPerformance
                | GreatPerformance ->
                    Concert.makeCrowdSingGreatPerformance points
                | _ -> Concert.tooMuchSingAlong
                |> showMessage)
            ongoingConcert
