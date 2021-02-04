open Renderer.Render

[<EntryPoint>]
let main argv =
  init ()
  Savegame.load ()
  |> Startup.fromSavegame
  |> fun (state, action) -> Orchestrator.runWith render state action
  |> ignore

  0
