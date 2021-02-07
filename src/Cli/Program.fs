open Renderer.Render

[<EntryPoint>]
let main argv =
  let renderer = CliRenderer() :> Orchestrator.IRenderer
  renderer.Clear()

  Savegame.load ()
  |> Startup.fromSavegame
  |> fun (state, action) ->
       Orchestrator.runWith renderer state
       <| seq { action }
  |> ignore

  0
