module Duets.Scenes.Game

open Duets.Scenes.Base
open Duets.Text.Constants
open Duets.Types

type GameScene(_navigator: INavigator) =
    inherit UiScene()

    override this.OnStart() =
        this.UiRoot.AddText(Literal "TBD") TextSize.Title centered

        ()
