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
            CommandMakeCrowdSingDescription
            spinDrumsticks
            (fun result points ->
                match result with
                | LowPerformance -> ConcertDrumstickSpinningBadResult points
                | AveragePerformance
                | GoodPerformance
                | GreatPerformance -> ConcertDrumstickSpinningGoodResult points
                | _ -> ConcertTooManyDrumstickSpins
                |> ConcertText
                |> I18n.translate
                |> showMessage)
            ongoingConcert
