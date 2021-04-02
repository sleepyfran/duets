open Renderer
open View.Actions
open Mediator.Query
open Mediator.Queries.Storage

[<EntryPoint>]
let main argv =
  clear ()

  query SavegameStateQuery
  |> fun (savegameState) ->
      match savegameState with
      | NotAvailable -> (None, Scene MainMenu)
      | Available -> (None, Scene MainMenu)
  |> fun (state, action) ->
       Orchestrator.runWith renderPrompt renderMessage state
       <| seq { action }
  |> ignore

  0
