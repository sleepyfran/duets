module Duets.UI.Common.Scenes.MainMenu

open Duets.Agents
open Duets.UI.Common

type private MenuOption =
    | NewGame
    | LoadGame
    | Exit

let scene () : Scene<Navigate> =
    scene {
        let gameVersion =
            System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString()

        let hasSave =
            match Savegame.load () with
            | Savegame.Available _ -> true
            | _ -> false

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

        return
            match option with
            | NewGame -> Navigate.NewGame
            | LoadGame -> Navigate.InGame
            | Exit -> Navigate.Exit
    }
