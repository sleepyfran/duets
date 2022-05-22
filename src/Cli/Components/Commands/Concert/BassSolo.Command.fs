namespace Cli.Components.Commands

open Cli.Text
open Simulation
open Simulation.Concerts.Live

[<RequireQualifiedAccess>]
module BassSoloCommand =
    /// Command to perform a bass solo.
    let create ongoingConcert =
        Concert.createSoloCommand
            "bass solo"
            CommandBassSoloDescription
            [ ConcertText ConcertBassSoloMovingFingersQuickly
              |> I18n.translate
              ConcertText ConcertBassSoloSlappingThatBass
              |> I18n.translate
              ConcertText ConcertBassSoloGrooving
              |> I18n.translate ]
            bassSolo
            ongoingConcert
