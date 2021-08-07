module Duets.Scenes.MainMenu

open System

open Duets.Scenes.Base
open Duets.Text.Constants
open Duets.Types

type MainMenuScene(navigator: INavigator) =
    inherit UiScene()

    override this.Initialize() = base.Initialize()

    override this.OnStart() =
        this.UiRoot.AddText(TextConstant GameName) TextSize.Title centered

        this.UiRoot.AddButton
            (TextConstant MainMenuNewGame)
            (fun () -> navigator.NavigateWithTransition CharacterCreator)
            centered

        (this.UiRoot.AddButton
            (TextConstant MainMenuExit)
            (fun () -> Environment.Exit 0))
            centered

        ()
