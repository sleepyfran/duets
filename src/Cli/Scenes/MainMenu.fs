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
    | Some NewGame -> Scene.CharacterCreator
    | Some LoadGame -> Scene.World
    | None -> Scene.Exit
