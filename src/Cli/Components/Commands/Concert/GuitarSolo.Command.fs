namespace Cli.Components.Commands

open Cli.Text
open Simulation
open Simulation.Concerts.Live

[<RequireQualifiedAccess>]
module GuitarSoloCommand =
    /// Command to perform a guitar solo.
    let create ongoingConcert =
        Concert.createSoloCommand
            "guitar solo"
            CommandGuitarSoloDescription
            [ ConcertText ConcertGuitarSoloPlayingReallyFast
              |> I18n.translate
              ConcertText ConcertGuitarSoloPlayingWithTeeth
              |> I18n.translate
              ConcertText ConcertGuitarSoloDoingSomeTapping
              |> I18n.translate ]
            guitarSolo
            ongoingConcert
