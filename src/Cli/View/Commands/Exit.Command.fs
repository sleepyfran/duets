namespace Cli.View.Commands

open Cli.View.Actions
open Cli.View.Text

[<RequireQualifiedAccess>]
module ExitCommand =
    /// Command which exits the app upon being called.
    let get =
        { Name = "exit"
          Description = I18n.translate (CommandText CommandExitDescription)
          Handler = HandlerWithNavigation(fun _ -> seq { Exit }) }
