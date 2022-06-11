[<AutoOpen>]
module Cli.Components.Message

open Spectre.Console

/// Renders a message into the screen.
let showMessage text = text |> AnsiConsole.MarkupLine
