open Renderer.Render

[<EntryPoint>]
let main argv =
  init ()

  Savegame.load ()
  |> Startup.fromSavegame
  |> fun (state, action) ->
       Orchestrator.runWith renderMessage renderPrompt state action
  |> ignore

  0
