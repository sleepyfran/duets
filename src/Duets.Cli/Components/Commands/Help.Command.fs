namespace Duets.Cli.Components.Commands

open Duets.Cli.Components
open Duets.Cli.Text
open Duets.Cli.SceneIndex

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

                let columns =
                    [ Styles.header "Command"; Styles.header "Description" ]

                let rows =
                    commands
                    |> List.map (fun command ->
                        [ Styles.highlight command.Name; command.Description ])

                showTable columns rows

                lineBreak ()
                showMessage Command.helpFooter

                Scene.World }
