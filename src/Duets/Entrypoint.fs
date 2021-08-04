module Duets.Entrypoint

open Microsoft.Xna.Framework
open Nez

open Duets.Scenes.MainMenu

type Game() =
    inherit Core()

    override this.Initialize() =
        base.Initialize()

        Screen.SetSize(1280, 720)
        this.Window.AllowUserResizing <- true
        this.Window.IsBorderless <- true

        // Load the main menu as the initial scene.
        Core.Scene <- MainMenuScene()

[<EntryPoint>]
let main argv =
    let game = new Game()
    game.Run()
    0
