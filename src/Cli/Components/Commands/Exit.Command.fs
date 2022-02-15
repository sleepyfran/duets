namespace Cli.Components.Commands

open Cli.SceneIndex
open Cli.Text

[<RequireQualifiedAccess>]
module ExitCommand =
    /// Command which exits the app upon being called.
    let get =
        { Name = "exit"
          Description = I18n.translate (CommandText CommandExitDescription)
          Handler = fun _ -> Some Scene.Exit }
