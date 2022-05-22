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
    /// Creates a command with the given name and description that when called
    /// outputs the given message.
    let message name description message =
        { Name = name
          Description = CommandText description |> I18n.translate
          Handler =
            (fun _ ->
                ConcertText message
                |> I18n.translate
                |> showMessage

                Scene.World) }

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
