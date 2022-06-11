namespace Cli.Components.Commands

open Cli.Components
open Cli.Components.Commands
open Cli.Text
open Simulation
open Simulation.Concerts.Live.Encore

[<RequireQualifiedAccess>]
module DoEncoreCommand =
    /// Returns the artist back to the stage to perform an encore. Assumes that
    /// an encore is possible and that the audience will still be there for it.
    let create ongoingConcert =
        Concert.createCommand
            "do encore"
            Command.doEncoreDescription
            doEncore
            (fun _ _ -> Concert.encoreComingBackToStage |> showMessage)
            ongoingConcert
