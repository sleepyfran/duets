module Duets.Entrypoint

open Duets.Scenes.Navigator
open Nez

open Duets.Scenes.MainMenu

type Game() =
    inherit Core()

    let navigator = SceneNavigator()

    override this.Initialize() =
        base.Initialize()

        Screen.SetSize(1280, 720)
        this.Window.AllowUserResizing <- true
        this.Window.IsBorderless <- true

        // Load the main menu as the initial scene.
        Core.Scene <- MainMenuScene(navigator)

[<EntryPoint>]
let main _ =
    let game = new Game()
    game.Run()
    0
