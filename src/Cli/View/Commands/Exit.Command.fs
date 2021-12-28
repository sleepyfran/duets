namespace Cli.View.Commands

open Cli.View.Actions
open Cli.View.TextConstants

[<RequireQualifiedAccess>]
module ExitCommand =
    /// Command which exits the app upon being called.
    let get =
        { Name = "exit"
          Description = TextConstant CommandExitDescription
          Handler = HandlerWithNavigation(fun _ -> seq { Exit }) }
