[<AutoOpen>]
module Duets.Cli.Components.Notification

open Duets.Cli.Text
open Spectre.Console

/// Shows a notification inside of a panel with a bell with the given title
/// and the given body text.
let showNotification (title: string) (text: string) =
    let header = PanelHeader(Styles.header $"{Emoji.notification} {title}")

    Panel(
        Markup(text),
        Header = header,
        Border = BoxBorder.Double,
        Expand = true,
        Padding = Padding(2, 4)
    )
    |> AnsiConsole.Write
