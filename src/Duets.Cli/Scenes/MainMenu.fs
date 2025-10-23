module Duets.Cli.Scenes.MainMenu

open Duets.Agents
open Duets.Agents.Savegame
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Data.Savegame.Types
open System.Reflection

let gameVersion =
    let assembly = Assembly.GetEntryAssembly()

    let infoVersion =
        assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()

    match infoVersion with
    | null -> assembly.GetName().Version.ToString()
    | attr ->
        let version = attr.InformationalVersion
        let isNonFinalVersion = version.Contains("-")
        if isNonFinalVersion then version else version.Split("+")[0]


type private MainMenuOption =
    | NewGame
    | LoadGame
    | Settings

let private textFromOption opt =
    match opt with
    | NewGame -> MainMenu.newGame
    | LoadGame -> MainMenu.loadGame
    | Settings -> MainMenu.settings

let private textFromMigrationError err =
    match err with
    | MigrationError.InvalidStructure message -> message
    | MigrationError.InvalidVersion version ->
        $"Unknown savegame version {version}"

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

    let hasSavegameAvailable =
        match savegameState with
        | Available _ -> true
        | NotAvailable -> false
        | Incompatible reason ->
            textFromMigrationError reason
            |> MainMenu.incompatibleSavegame
            |> showMessage

            false

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
