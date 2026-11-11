namespace Duets.Cli.Components.Commands

open Duets.Cli.Text
open Duets.Common
open Duets.Entities
open Duets.Simulation.Concerts.Live

[<RequireQualifiedAccess>]
module SoloCommand =
    let private name instrument =
        match instrument with
        | Drums -> "drum solo"
        | _ -> $"{Generic.instrumentName instrument |> String.lowercase} solo"

    /// Command to perform a solo with the given instrument.
    let create ongoingConcert instrument =
        Concert.createSoloCommand
            (name instrument)
            (Command.soloDescription instrument)
            (Concert.soloProgress instrument)
            (fun state ongoingConcert -> solo state ongoingConcert instrument)
            ongoingConcert
