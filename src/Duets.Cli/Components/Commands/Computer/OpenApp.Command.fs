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

                    Computer.openApp item computer app |> Effect.applyMultiple
                | None -> ()

                Scene.World }
