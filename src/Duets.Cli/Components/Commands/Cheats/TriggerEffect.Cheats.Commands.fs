namespace Duets.Cli.Components.Commands.Cheats

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components.Commands
open Duets.Cli.SceneIndex
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module TriggerEffectCommands =
    /// Command which triggers the specified effect. Since this requires manually
    /// pattern matching against all possible effects, it does not support
    /// every effect.
    let triggerEffect =
        { Name = "trigger_effect"
          Description = ""
          Handler =
            (fun args ->
                match args with
                | [ "band_fans"; oldFans; newFans ] ->
                    let currentBand = Queries.Bands.currentBand (State.get ())

                    BandFansChanged(
                        currentBand,
                        Diff(int oldFans, int newFans)
                    )
                    |> Effect.apply
                | _ -> ()

                Scene.World) }
