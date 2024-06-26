namespace Duets.Cli.Components.Commands

open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Entities
open FSharp.Data.UnitSystems.SI.UnitNames

[<RequireQualifiedAccess>]
module Concert =
    /// Creates a command that performs a specific action in the concert, without
    /// showing any intermediate output.
    let eventCommand name description event ongoingConcert =
        Command.action
            name
            description
            (ConcertPerformAction
                {| Action = event
                   Concert = ongoingConcert |})

    /// Creates a command to perform a solo in a concert, which shows a progress bar
    /// with the given steps before applying the effect.
    let soloCommand
        name
        description
        stepNames
        (instrument: InstrumentType)
        ongoingConcert
        =
        Command.create name description (fun _ ->
            showProgressBarAsync stepNames 2<second>

            let action =
                match instrument with
                | Guitar -> GuitarSolo
                | Drums -> DrumSolo
                | Bass -> BassSolo
                | Vocals -> MakeCrowdSing

            ConcertPerformAction
                {| Action = action
                   Concert = ongoingConcert |}
            |> Effect.applyAction

            Scene.World)
