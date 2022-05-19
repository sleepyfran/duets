namespace Cli.Components.Commands

open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Entities

/// Defines a command that can be executed by the user.
type Command =
    { Name: string
      Description: Text
      Handler: string list -> Scene }

[<RequireQualifiedAccess>]
module Command =
    /// Disables the command for a given reason, which removes the actual handler
    /// of the command and mocks it with a message displaying the reason why the
    /// action is not possible.
    let disable disabledReason command =
        { command with
            Handler =
                (fun _ ->
                    match disabledReason with
                    | InteractionDisabledReason.NotEnoughEnergy energyNeeded ->
                        CommandDisabledNotEnoughEnergy energyNeeded
                    | InteractionDisabledReason.NotEnoughHealth healthNeeded ->
                        CommandDisabledNotEnoughHealth healthNeeded
                    | InteractionDisabledReason.NotEnoughMood moodNeeded ->
                        CommandDisabledNotEnoughMood moodNeeded
                    |> CommandText
                    |> I18n.translate
                    |> showMessage

                    Scene.World) }
