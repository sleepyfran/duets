[<AutoOpen>]
module Duets.Cli.Components.Message

open Spectre.Console

/// Renders a message into the screen.
let showMessage text = text |> AnsiConsole.MarkupLine

/// Renders a path into the screen.
let showPath path =
    let mutable path = TextPath path

    path.RootStyle <- Style(foreground = Color.Green)
    path.SeparatorStyle <- Style(foreground = Color.Green)
    path.StemStyle <- Style(foreground = Color.Blue)
    path.LeafStyle <- Style(foreground = Color.Yellow)

    AnsiConsole.Write path
    AnsiConsole.WriteLine()

/// Renders an exception into the screen.
let showException = AnsiConsole.WriteException
