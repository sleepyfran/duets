module Duets.Cli.Scenes.Cheats

open Duets.Cli.Components
open Duets.Cli.Components.Commands
open Duets.Cli.SceneIndex
open Duets.Cli.Text

let private leaveCommand =
    { Name = "leave"
      Description = "Makes you come back to reality"
      Handler = fun _ -> Scene.World }

/// Shows a scene that allows the player to input cheats and debug commands.
let cheatsScene () =
    "You're now in cheat mode. Proceed with caution... or not, who cares."
    |> Styles.warning
    |> showMessage

    let prompt =
        $"""{"[[Cheat mode enabled]]" |> Styles.error} What magic trickery are you doing today?"""
        |> Styles.prompt

    let commands =
        leaveCommand :: ExitCommand.get :: MapCommand.get :: Cheats.Index.all

    commands
    |> (@)
        [ HelpCommand.createForApp
              "cheat mode"
              (fun () -> Scene.Cheats)
              commands ]
    |> showCommandPrompt prompt
