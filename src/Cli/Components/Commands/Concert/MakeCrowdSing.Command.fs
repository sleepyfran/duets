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
            CommandMakeCrowdSingDescription
            makeCrowdSing
            (fun result points ->
                match result with
                | LowPerformance -> ConcertMakeCrowdSingLowPerformance points
                | AveragePerformance ->
                    ConcertMakeCrowdSingLowPerformance points
                | GoodPerformance
                | GreatPerformance ->
                    ConcertMakeCrowdSingGreatPerformance points
                | _ -> ConcertTooMuchSingAlong
                |> ConcertText
                |> I18n.translate
                |> showMessage)
            ongoingConcert
