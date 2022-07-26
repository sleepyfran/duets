namespace Cli.Components.Commands

open Cli.Components
open Cli.Text
open Cli.SceneIndex

[<RequireQualifiedAccess>]
module HelpCommand =
    /// Creates a command that shows the name and description of all given
    /// commands as a list.
    let create (commands: Command list) =
        { Name = "help"
          Description = Command.helpDescription
          Handler =
            fun _ ->
                showMessage Command.helpDescription

                commands
                |> List.iter (fun command ->
                    Command.helpEntry command.Name command.Description
                    |> showMessage)

                lineBreak ()
                showMessage Command.helpFooter

                Scene.World }
