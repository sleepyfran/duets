module rec Duets.Cli.Scenes.Settings

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Common
open System.IO
open System.Diagnostics

type private SettingOption =
    | SavegamePath
    | OpenSourceCode

let settings () =
    let selectedOption =
        showOptionalChoicePrompt
            ""
            Generic.back
            (function
            | SavegamePath -> "Change savegame path"
            | OpenSourceCode -> "Open game source code")
            [ SavegamePath; OpenSourceCode ]

    match selectedOption with
    | Some SavegamePath ->
        changeSaveGamePath ()

        Scene.Settings
    | Some OpenSourceCode ->
        ProcessStartInfo(
            "https://github.com/sleepyfran/duets",
            UseShellExecute = true
        )
        |> Process.Start
        |> ignore

        Scene.Settings
    | None -> Scene.MainMenu

let private changeSaveGamePath () =
    let customSettings = Savegame.settings ()

    match customSettings with
    | Some settings ->
        "Your current savegame lives in:" |> showMessage

        settings.SavegamePath |> showPath
    | None ->
        "You have not set a custom savegame path. The default path is:"
        |> showMessage

        Files.duetsFolder () |> showPath

    askForSaveGamePath ()

let rec private askForSaveGamePath () = askUntilPathExists () |> saveSettings

let private askUntilPathExists () =
    let path =
        "Enter a new savegame path or leave empty to use the default path:"
        |> showOptionalTextPrompt

    match path with
    | None ->
        "Resetting savegame path to default" |> Styles.success |> showMessage

        None
    | Some path ->
        let pathExists = Path.Exists path

        if pathExists then
            "Savegame path set to:" |> Styles.success |> showMessage
            path |> showPath
            { Savegame.SavegamePath = path } |> Some
        else
            "The path you entered does not exist. Try again:"
            |> Styles.error
            |> showMessage

            askUntilPathExists ()

let private saveSettings settingsOrNone =
    let saveResult = Savegame.saveSettings settingsOrNone

    match saveResult with
    | Savegame.Success -> ()
    | Savegame.Failure exn ->
        """Failed to save the settings. This is probably a bug, please report it
over in the game's repository:
https://github.com/sleepyfran/duets/issues
"""
        |> showMessage

        exn |> showException
