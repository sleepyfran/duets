namespace Duets.Cli.Components.Commands

open Duets.Cli.Components
open Duets.Cli.Text
open Duets.Cli.SceneIndex
open Duets.Entities

[<RequireQualifiedAccess>]
module HelpCommand =
    /// Creates a command that shows the name and description of all given
    /// commands as a list.
    let create (commands: (int<dayMoments> * Command) list) =
        { Name = "help"
          Description = Command.helpDescription
          Handler =
            fun _ ->
                showMessage Command.helpDescription

                let columns =
                    [ Styles.header "Command"
                      Styles.header "Description"
                      Styles.header Emoji.clock ]

                let rows =
                    commands
                    |> List.map (fun (timeConsumption, command) ->
                        [ Styles.highlight command.Name
                          command.Description
                          if timeConsumption > 0<dayMoments> then
                              $"{timeConsumption}" ])

                showTable columns rows

                lineBreak ()

                $"""Remember that when referencing items you {Styles.highlight "don't"} need to write diacritics"""
                |> Styles.faded
                |> showMessage

                "The numbers in the third column reference the amount of day moments that would pass when that command is executed"
                |> Styles.faded
                |> showMessage

                Scene.World }

    /// Creates a command that shows the name and description of all the commands
    /// available in the current context, tailored to be used when inside of an
    /// app, and which executes the given callback instead of returning to the
    /// world.
    let createForApp appName appCallback (commands: Command list) =
        { Name = "help"
          Description = Command.helpDescription
          Handler =
            fun _ ->
                showMessage
                    $"Here are all the commands you can execute in {appName}:"

                let columns =
                    [ Styles.header "Command"; Styles.header "Description" ]

                let rows =
                    commands
                    |> List.map (fun command ->
                        [ Styles.highlight command.Name; command.Description ])

                showTable columns rows

                lineBreak ()

                appCallback () }
