[<AutoOpen>]
module Cli.Components.Figlet

open Cli.Localization
open Spectre.Console

/// Renders a figlet (see: http://www.figlet.org).
let showFiglet text =
    let figlet = FigletText(toString text).Centered()
    figlet.Color <- Color.Blue
    AnsiConsole.Write(figlet)
