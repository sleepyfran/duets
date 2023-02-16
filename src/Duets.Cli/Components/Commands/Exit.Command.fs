namespace Duets.Cli.Components.Commands

open Duets.Cli.SceneIndex
open Duets.Cli.Text

[<RequireQualifiedAccess>]
module ExitCommand =
    /// Command which exits the app upon being called.
    let get =
        { Name = "exit"
          Description = Command.exitDescription
          Handler = fun _ -> Scene.Exit ExitMode.SaveGame }
