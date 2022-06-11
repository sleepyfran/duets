[<AutoOpen>]
module Cli.Components.Separator

open Spectre.Console

/// Renders a line into the screen with an optional text that, if given, shows
/// in the center of the screen.
let showSeparator text =
    let rule = Rule().Centered()
    rule.Style <- Style.Parse("blue dim")

    match text with
    | Some text -> rule.Title <- text
    | None -> ()

    AnsiConsole.Write(rule)

/// Renders an empty line in the screen.
let lineBreak () = AnsiConsole.WriteLine()
