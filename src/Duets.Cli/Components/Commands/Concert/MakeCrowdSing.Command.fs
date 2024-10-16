namespace Duets.Cli.Components.Commands

open Duets.Cli.Components
open Duets.Cli.Components.Commands
open Duets.Cli.Text
open Duets.Simulation.Concerts.Live

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
                | LowPerformance _ ->
                    Concert.makeCrowdSingLowPerformance points
                | AveragePerformance _ ->
                    Concert.makeCrowdSingAveragePerformance points
                | GoodPerformance _
                | GreatPerformance ->
                    Concert.makeCrowdSingGreatPerformance points
                | _ -> Concert.tooMuchSingAlong
                |> showMessage)
            ongoingConcert
