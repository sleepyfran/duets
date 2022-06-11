namespace Cli.Components.Commands

open Cli.Components
open Cli.SceneIndex
open Cli.Text
open Entities

/// Defines a command that can be executed by the user.
type Command =
    { Name: string
      Description: string
      Handler: string list -> Scene }

[<RequireQualifiedAccess>]
module Command =
    /// Creates a command with the given name and description that when called
    /// outputs the given message.
    let message name description message =
        { Name = name
          Description = description
          Handler =
            (fun _ ->
                showMessage message

                Scene.World) }

    /// Creates a command with the given name and description that when called
    /// applies the given effects and outputs the given message.
    let interaction name description effects message =
        { Name = name
          Description = description
          Handler =
            (fun _ ->
                effects |> Cli.Effect.applyMultiple

                showMessage message

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
                        Command.disabledNotEnoughEnergy energyNeeded
                    | InteractionDisabledReason.NotEnoughHealth healthNeeded ->
                        Command.disabledNotEnoughHealth healthNeeded
                    | InteractionDisabledReason.NotEnoughMood moodNeeded ->
                        Command.disabledNotEnoughMood moodNeeded
                    |> showMessage

                    Scene.World) }
