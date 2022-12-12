namespace Cli.Components.Commands

open Cli.SceneIndex
open Cli.Text

[<RequireQualifiedAccess>]
module ExitCommand =
    /// Command which exits the app upon being called.
    let get =
        { Name = "exit"
          Description = Command.exitDescription
          Handler = fun _ -> Scene.Exit ExitMode.SaveGame }
