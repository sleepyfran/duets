namespace Cli.View.Commands

open Cli.View.Actions
open Cli.View.Text

[<RequireQualifiedAccess>]
module HelpCommand =
    /// Creates a command that shows the name and description of all given
    /// commands as a list.
    let create (commands: Command list) =
        { Name = "help"
          Description = I18n.translate (CommandText CommandHelpDescription)
          Handler =
              HandlerWithoutNavigation
                  (fun _ ->
                      seq {
                          yield
                              Message
                              <| I18n.translate (
                                  CommandText CommandHelpDescription
                              )

                          yield!
                              commands
                              |> List.map
                                  (fun command ->
                                      CommandHelpEntry(
                                          command.Name,
                                          command.Description
                                      )
                                      |> CommandText
                                      |> I18n.translate
                                      |> Message)
                      }) }
