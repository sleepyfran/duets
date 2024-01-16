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

/// Shows a list of tips (hints) to the player as a list with a header.
let showTips tips =
    "Tips" |> Styles.title |> showMessage
    tips |> List.iter (fun tip -> $"- {tip}" |> Styles.hint |> showMessage)
