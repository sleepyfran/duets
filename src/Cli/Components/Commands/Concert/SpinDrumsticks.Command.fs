namespace Cli.Components.Commands

open Cli.Components
open Cli.Components.Commands
open Cli.Text
open Simulation
open Simulation.Concerts.Live

[<RequireQualifiedAccess>]
module SpinDrumsticksCommand =
    /// Command which performs the action of spinning the drumsticks for drummers.
    let rec create ongoingConcert =
        Concert.createCommand
            "spin drumstick"
            Command.makeCrowdSingDescription
            spinDrumsticks
            (fun result points ->
                match result with
                | LowPerformance -> Concert.drumstickSpinningBadResult points
                | AveragePerformance
                | GoodPerformance
                | GreatPerformance -> Concert.drumstickSpinningGoodResult points
                | _ -> Concert.tooManyDrumstickSpins
                |> showMessage)
            ongoingConcert
