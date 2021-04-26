open Renderer
open Cli.View.Actions
open Storage.Savegame

[<EntryPoint>]
let main _ =
  clear ()

  savegameState ()
  |> fun savegameState ->
       match savegameState with
       | Available -> Scene MainMenu
       | NotAvailable -> Scene MainMenu
  |> fun action ->
       Orchestrator.runWith renderPrompt renderMessage
       <| seq { action }

  0
