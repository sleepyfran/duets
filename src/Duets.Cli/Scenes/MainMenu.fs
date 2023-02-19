module Duets.Cli.Scenes.MainMenu

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text

let gameVersion =
    System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString()

type private MainMenuOption =
    | NewGame
    | LoadGame

let private textFromOption opt =
    match opt with
    | NewGame -> MainMenu.newGame
    | LoadGame -> MainMenu.loadGame

/// Main menu of the game where the user can choose to either start a new game
/// or load a previous one.
let rec mainMenu savegameState =
    clearScreen ()

    Generic.gameName |> showFiglet
    showGameInfo gameVersion

    if savegameState = Savegame.Incompatible then
        MainMenu.incompatibleSavegame |> showMessage

    let hasSavegameAvailable =
        savegameState = Savegame.Available

    let selectedChoice =
        showOptionalChoicePrompt
            MainMenu.prompt
            MainMenu.exit
            textFromOption
            [ NewGame
              if hasSavegameAvailable then
                  LoadGame ]

    match selectedChoice with
    | Some NewGame -> createNewGame savegameState hasSavegameAvailable
    | Some LoadGame -> Scene.WorldAfterMovement
    | None -> Scene.Exit ExitMode.SkipSave

and private createNewGame savegameState hasSavegameAvailable =
    if hasSavegameAvailable then
        let confirmed =
            showConfirmationPrompt MainMenu.newGameReplacePrompt

        if confirmed then
            Scene.CharacterCreator
        else
            mainMenu savegameState
    else
        Scene.CharacterCreator