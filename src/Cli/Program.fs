open System.Globalization
open System.Threading

open Agents

open Cli.View.Actions
open Cli.View.Renderer

[<EntryPoint>]
let main _ =
    clear ()

    // Set default culture to UK for sane defaults :)
    Thread.CurrentThread.CurrentCulture <- CultureInfo("en-UK")

    Savegame.load ()
    |> fun savegameState -> MainMenu savegameState |> Scene
    |> fun action -> seq { action } |> Orchestrator.runWith

    0
