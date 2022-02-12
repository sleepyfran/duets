[<AutoOpen>]
module Cli.Components.Message

open Cli.Localization
open Spectre.Console

/// Renders a message into the screen.
let showMessage text = toString text |> AnsiConsole.MarkupLine
