module Cli.DefaultCommands

open Cli.View.Actions
open Cli.View.TextConstants

/// Creates a help command which shows the name and description of all the
/// given commands.
let createHelpCommand (commands: Command list) =
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

/// Command which opens the phone of the user.
let phoneCommand =
    { Name = "phone"
      Description = TextConstant CommandPhoneDescription
      Handler = HandlerWithNavigation(fun _ -> seq { Scene Phone }) }

/// Command which exits the app upon being called.
let exitCommand =
    { Name = "exit"
      Description = TextConstant CommandExitDescription
      Handler = HandlerWithNavigation(fun _ -> seq { Exit }) }
