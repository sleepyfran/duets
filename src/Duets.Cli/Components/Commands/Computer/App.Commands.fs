namespace Duets.Cli.Components.Commands

open Duets.Cli
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module OpenAppCommand =
    let private appName =
        function
        | WordProcessor -> "Word"

    /// Command to open an app on the currently used computer.
    let create item computer apps =
        { Name = "open app"
          Description = "Shows a picker to open an app on the computer"
          Handler =
            fun _ ->
                let selectedApp =
                    showOptionalChoicePrompt
                        "Select an app"
                        Generic.cancel
                        appName
                        apps

                match selectedApp with
                | Some app ->
                    $"Opening {app |> appName}..." |> showMessage

                    wait 500<millisecond>

                    Computer.openApp app item computer |> Effect.applyMultiple
                | None -> ()

                Scene.World }

[<RequireQualifiedAccess>]
module CloseAppCommand =
    /// Command to close an app on the currently used computer.
    let create item computer =
        { Name = "close app"
          Description = "Closes the currently running app"
          Handler =
            fun _ ->
                Computer.closeApp item computer |> Effect.applyMultiple

                Scene.World }
