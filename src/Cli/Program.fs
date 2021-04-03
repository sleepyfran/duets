open Renderer
open View.Actions
open Mediator.Query
open Mediator.Queries.Storage

[<EntryPoint>]
let main _ =
  clear ()

  query SavegameStateQuery
  |> fun savegameState ->
       match savegameState with
       | NotAvailable -> Scene MainMenu
       | Available -> Scene MainMenu
  |> fun action ->
       Orchestrator.runWith renderPrompt renderMessage
       <| seq { action }

  0
