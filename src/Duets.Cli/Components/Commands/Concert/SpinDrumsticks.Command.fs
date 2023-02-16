namespace Duets.Cli.Components.Commands

open Duets.Cli.Components
open Duets.Cli.Components.Commands
open Duets.Cli.Text
open Duets.Simulation.Concerts.Live

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
                | LowPerformance _ -> Concert.drumstickSpinningBadResult points
                | AveragePerformance _
                | GoodPerformance _
                | GreatPerformance _ ->
                    Concert.drumstickSpinningGoodResult points
                | _ -> Concert.tooManyDrumstickSpins
                |> showMessage)
            ongoingConcert
