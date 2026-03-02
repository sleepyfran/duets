module Duets.UI.Scenes.MainMenu

open Duets.Agents
open Duets.UI
open Duets.UI.Common
open Duets.UI.SceneIndex

let private gameVersion =
    System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString()

let view (switchTo: Scene -> unit) =
    let hasSave =
        match Savegame.load () with
        | Savegame.Available _ -> true
        | _ -> false

    let navigate =
        function
        | Navigate.NewGame -> switchTo Scene.NewGame
        | Navigate.InGame -> switchTo Scene.InGame
        | Navigate.Exit -> System.Environment.Exit(0)
        | Navigate.MainMenu -> ()

    Renderer.run "MainMenu" (Duets.UI.Common.Scenes.MainMenu.scene navigate gameVersion hasSave)
