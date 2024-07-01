namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.Components.Commands
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Simulation.Concerts
open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames

[<RequireQualifiedAccess>]
module PerformSoundcheckCommand =
    /// Returns a command that marks the soundcheck as done.
    let create checklist =
        { Name = "soundcheck"
          Description =
            "Allows you to check the sound of your band before the concert starts, which improves the quality of the concert"
          Handler =
            fun _ ->
                showProgressBarSync
                    [ "Plugin cables..." |> Styles.progress
                      "Mic check, mic check..." |> Styles.progress
                      "Staring into the abyss while the rest of the band checks..."
                      |> Styles.progress ]
                    1<second>

                Live.Actions.soundcheck (State.get ()) checklist
                |> Effect.applyMultiple

                Scene.World }
