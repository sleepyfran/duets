module Duets.UI.Scenes.InGame

open Duets.UI
open Duets.UI.Common
open Duets.UI.SceneIndex

let view (switchTo: Scene -> unit) =
    let navigate =
        function
        | Navigate.InGame -> switchTo Scene.InGame
        | Navigate.NewGame -> switchTo Scene.NewGame
        | Navigate.MainMenu -> switchTo Scene.MainMenu
        | Navigate.Exit -> System.Environment.Exit(0)

    Renderer.run "InGame" (Duets.UI.Common.Scenes.InGame.scene navigate)
