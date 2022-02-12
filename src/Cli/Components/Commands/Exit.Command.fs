namespace Cli.Components.Commands

open Cli.Actions
open Cli.Text

[<RequireQualifiedAccess>]
module ExitCommand =
    /// Command which exits the app upon being called.
    let get =
        { Name = "exit"
          Description = I18n.translate (CommandText CommandExitDescription)
          Handler =
              fun _ ->
                  System.Environment.Exit(0)
                  None }
