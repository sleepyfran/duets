namespace Cli.Components.Commands

open Cli.Text
open Simulation
open Simulation.Concerts.Live

[<RequireQualifiedAccess>]
module DrumSoloCommand =
    /// Command to perform a drum solo.
    let create ongoingConcert =
        Concert.createSoloCommand
            "drum solo"
            CommandDrumSoloDescription
            [ ConcertText ConcertDrumSoloDoingDrumstickTricks
              |> I18n.translate
              ConcertText ConcertDrumSoloPlayingWeirdRhythms
              |> I18n.translate
              ConcertText ConcertDrumSoloPlayingReallyFast
              |> I18n.translate ]
            drumSolo
            ongoingConcert
