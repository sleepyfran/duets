namespace Duets.Cli.Components.Commands

open Duets.Cli.Components
open Duets.Cli.Components.Commands
open Duets.Cli.Text
open Duets.Simulation.Concerts.Live.Encore

[<RequireQualifiedAccess>]
module DoEncoreCommand =
    /// Returns the artist back to the stage to perform an encore. Assumes that
    /// an encore is possible and that the audience will still be there for it.
    let create ongoingConcert =
        Concert.createCommand
            "do encore"
            Command.doEncoreDescription
            (fun _ -> doEncore)
            (fun _ _ -> Concert.encoreComingBackToStage |> showMessage)
            ongoingConcert
