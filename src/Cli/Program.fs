open Cli.View.Actions
open Cli.View.Renderer
open Storage

[<EntryPoint>]
let main _ =
    clear ()

    Savegame.load ()
    |> fun savegameState -> MainMenu savegameState |> Scene
    |> fun action -> seq { action } |> Orchestrator.runWith

    0
