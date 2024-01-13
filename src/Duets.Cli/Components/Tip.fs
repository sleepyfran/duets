[<AutoOpen>]
module Duets.Cli.Components.Tip

open Duets.Cli.Text
open Spectre.Console

/// Shows a tip (hint) to the player inside a panel.
let showTip title text =
    let header = PanelHeader(Styles.header $"{Emoji.tip} {title}")

    Panel(
        Markup(text |> Styles.highlight),
        Header = header,
        Border = BoxBorder.Rounded,
        Expand = false
    )
    |> AnsiConsole.Write
