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
    | Settings

let private textFromOption opt =
    match opt with
    | NewGame -> MainMenu.newGame
    | LoadGame -> MainMenu.loadGame
    | Settings -> MainMenu.settings

/// Main menu of the game where the user can choose to either start a new game
/// or load a previous one.
let rec mainMenu skipSaving =
    clearScreen ()

    Generic.gameName |> showFiglet
    showGameInfo gameVersion

    let savegameState = Savegame.load ()

    if skipSaving then
        Styles.danger
            "--no-saving arg detected, all changes during gameplay won't be persisted!"
        |> showMessage

    if savegameState = Savegame.Incompatible then
        MainMenu.incompatibleSavegame |> showMessage

    let hasSavegameAvailable = savegameState = Savegame.Available

    let selectedChoice =
        showOptionalChoicePrompt
            MainMenu.prompt
            MainMenu.exit
            textFromOption
            [ NewGame
              if hasSavegameAvailable then
                  LoadGame
              Settings ]

    match selectedChoice with
    | Some NewGame -> createNewGame skipSaving hasSavegameAvailable
    | Some LoadGame -> Scene.WorldAfterMovement
    | Some Settings -> Scene.Settings
    | None -> Scene.Exit ExitMode.SkipSave

and private createNewGame skipSaving hasSavegameAvailable =
    if hasSavegameAvailable then
        let confirmed = showConfirmationPrompt MainMenu.newGameReplacePrompt

        if confirmed then
            Scene.CharacterCreator
        else
            mainMenu skipSaving
    else
        Scene.CharacterCreator
