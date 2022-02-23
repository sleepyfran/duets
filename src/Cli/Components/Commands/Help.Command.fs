namespace Cli.Components.Commands

open Cli.Components
open Cli.Text

[<RequireQualifiedAccess>]
module HelpCommand =
    /// Creates a command that shows the name and description of all given
    /// commands as a list.
    let create (commands: Command list) =
        { Name = "help"
          Description = I18n.translate (CommandText CommandHelpDescription)
          Handler =
            fun _ ->
                I18n.translate (CommandText CommandHelpDescription)
                |> showMessage

                commands
                |> List.iter (fun command ->
                    CommandHelpEntry(command.Name, command.Description)
                    |> CommandText
                    |> I18n.translate
                    |> showMessage)

                None }
