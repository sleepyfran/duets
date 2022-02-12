[<AutoOpen>]
module Cli.Components.GameInfo

open Spectre.Console

/// <summary>
/// Shows the version of the game stylized.
/// </summary>
/// <param name="version">Current game version</param>
let showGameInfo version =
    let gameInfo = $"v{version}"
    let styledGameInfo = $"[bold blue dim]{gameInfo}[/]"

    System.Console.SetCursorPosition(
        (System.Console.WindowWidth - gameInfo.Length) / 2,
        System.Console.CursorTop
    )

    AnsiConsole.MarkupLine(styledGameInfo)
