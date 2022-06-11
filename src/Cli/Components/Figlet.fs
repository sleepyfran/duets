[<AutoOpen>]
module Cli.Components.Figlet

open Spectre.Console

/// Renders a figlet (see: http://www.figlet.org).
let showFiglet text =
    let figlet = FigletText(text).Centered()
    figlet.Color <- Color.Blue
    AnsiConsole.Write(figlet)
