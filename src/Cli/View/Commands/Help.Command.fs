namespace Cli.View.Commands

open Cli.View.Actions
open Cli.View.TextConstants

[<RequireQualifiedAccess>]
module HelpCommand =
    /// Creates a command that shows the name and description of all given
    /// commands as a list.
    let create (commands: Command list) =
        { Name = "help"
          Description = TextConstant CommandHelpDescription
          Handler =
              HandlerWithoutNavigation
                  (fun _ ->
                      seq {
                          yield Message <| TextConstant CommandHelpDescription

                          yield!
                              commands
                              |> List.map
                                  (fun command ->
                                      CommandHelpEntry(
                                          command.Name,
                                          command.Description
                                      )
                                      |> TextConstant
                                      |> Message)
                      }) }
