namespace Cli.Components.Commands

open Cli.Actions
open Cli.Text

/// Defines a command that can be executed by the user.
type Command =
    { Name: string
      Description: Text
      Handler: string list -> Scene option }
