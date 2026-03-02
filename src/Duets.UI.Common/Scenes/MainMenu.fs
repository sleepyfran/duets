module Duets.UI.Common.Scenes.MainMenu

open Duets.UI.Common

type private MenuOption =
    | NewGame
    | LoadGame
    | Exit

let scene (navigate: Navigate -> unit) (gameVersion: string) (hasSave: bool) : Scene<unit> =
    scene {
        do! showFiglet "duets"
        do! showGameInfo gameVersion

        let options =
            [ Some NewGame
              if hasSave then Some LoadGame
              Some Exit ]
            |> List.choose id

        let! option =
            askChoice options (function
                | NewGame -> "New game"
                | LoadGame -> "Load game"
                | Exit -> "Exit")

        match option with
        | NewGame -> navigate Navigate.NewGame
        | LoadGame -> navigate Navigate.InGame
        | Exit -> navigate Navigate.Exit
    }
