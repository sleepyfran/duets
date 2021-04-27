open Cli.View.Actions
open Cli.View.Renderer
open Storage.Savegame

[<EntryPoint>]
let main _ =
  clear ()

  savegameState ()
  |> fun savegameState ->
       match savegameState with
       | Available -> Scene MainMenu
       | NotAvailable -> Scene MainMenu
  |> fun action -> seq { action } |> Orchestrator.runWith

  0
