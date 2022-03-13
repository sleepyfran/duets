module Cli.Scenes.MainMenu

open Agents
open Cli.Components
open Cli.SceneIndex
open Cli.Text

let gameVersion =
    System
        .Reflection
        .Assembly
        .GetEntryAssembly()
        .GetName()
        .Version.ToString()

type private MainMenuOption =
    | NewGame
    | LoadGame

let private textFromOption opt =
    match opt with
    | NewGame -> MainMenuText MainMenuNewGame
    | LoadGame -> MainMenuText MainMenuLoadGame
    |> I18n.translate

/// Main menu of the game where the user can choose to either start a new game
/// or load a previous one.
let rec mainMenu savegameState =
    clearScreen ()

    I18n.translate (CommonText GameName) |> showFiglet
    showGameInfo gameVersion

    if savegameState = Savegame.Incompatible then
        I18n.translate (MainMenuText MainMenuIncompatibleSavegame)
        |> showMessage

    let hasSavegameAvailable = savegameState = Savegame.Available

    let selectedChoice =
        showOptionalChoicePrompt
            (MainMenuText MainMenuPrompt |> I18n.translate)
            (MainMenuText MainMenuExit |> I18n.translate)
            textFromOption
            [ NewGame
              if hasSavegameAvailable then LoadGame ]

    match selectedChoice with
    | Some NewGame -> createNewGame savegameState hasSavegameAvailable
    | Some LoadGame -> Scene.World
    | None -> Scene.Exit

and private createNewGame savegameState hasSavegameAvailable =
    if hasSavegameAvailable then
        let confirmed =
            showConfirmationPrompt (
                MainMenuText MainMenuNewGameReplacePrompt
                |> I18n.translate
            )

        if confirmed then
            Scene.CharacterCreator
        else
            mainMenu savegameState
    else
        Scene.CharacterCreator
