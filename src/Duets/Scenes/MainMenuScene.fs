module Duets.Scenes.MainMenu

open Nez
open System

open Duets.Scenes.Base
open Duets.Scenes.CharacterCreatorScene
open Duets.Text.Constants

type MainMenuScene() =
    inherit UiScene()

    override this.Initialize() = base.Initialize()

    override this.SetupView() =
        this.UiRoot.AddText(TextConstant GameName) TextSize.Title centered

        this.UiRoot.AddButton
            (TextConstant MainMenuNewGame)
            (fun () ->
                Core.StartSceneTransition(
                    FadeTransition(fun () -> CharacterCreatorScene() :> Scene)
                )
                |> ignore)
            centered

        (this.UiRoot.AddButton
            (TextConstant MainMenuExit)
            (fun () -> Environment.Exit 0))
            centered

        ()
