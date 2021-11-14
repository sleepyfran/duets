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

/// Command that shows the list of all objects around the player that they can
/// interact with.
let createLookCommand (room: InteractiveRoom) =
    { Name = "look"
      Description = TextConstant CommandLookDescription
      Handler =
          HandlerWithoutNavigation
              (fun _ ->
                  seq {
                      if List.isEmpty room.Objects then
                          yield
                              Message <| TextConstant CommandLookNoObjectsAround
                      else
                          yield
                              CommandLookEnvironmentDescription room.Description
                              |> TextConstant
                              |> Message

                          yield!
                              room.Objects
                              |> List.map
                                  (fun object ->
                                      let commandNames =
                                          object.Commands
                                          |> List.map
                                              (fun command -> command.Name)

                                      (object.Type, commandNames))
                              |> List.map (
                                  CommandLookObjectEntry
                                  >> TextConstant
                                  >> Message
                              )
                  }) }

/// Command which navigates the user to the map scene.
let mapCommand =
    { Name = "map"
      Description = TextConstant CommandMapDescription
      Handler = HandlerWithNavigation(fun _ -> seq { Scene Map }) }

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
