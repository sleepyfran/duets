[<AutoOpen>]
module Cli.Components.CommandPrompt

open Cli.Text
open Cli.Components.Commands
open Common

/// <summary>
/// Renders a command prompt that the given available commands and the exit/help
/// command. The prompt will keep on asking for a command as long as the handler
/// of that command returns None instead of some Scene. This is done so commands
/// that just do a side-effect or show some input but then need to return to the
/// prompt (like help) can indicate this.
/// </summary>
/// <param name="title">
/// Title of the prompt to show when asking for a command
/// </param>
/// <param name="availableCommands">
/// List of commands that are available to be executed
/// </param>
/// <returns>
/// The scene that the last executed command returned.
/// </returns>
let rec showCommandPrompt title availableCommands =
    let commandsWithEssentials =
        availableCommands @ [ ExitCommand.get ]
        |> fun commands -> [ HelpCommand.create commands ] @ commands

    let rec promptForCommand () =
        lineBreak ()
        showMessage title

        showTextPrompt (Literal ">")
        |> fun input ->
            let inputTokens =
                String.split ' ' input |> List.ofArray

            commandsWithEssentials
            |> List.tryFind (fun command ->
                let commandTokens =
                    String.split ' ' command.Name |> List.ofArray

                inputTokens
                |> List.truncate commandTokens.Length
                |> List.forall2' (=) commandTokens)
            |> tryRunCommand input
        |> Option.defaultWith promptForCommand

    promptForCommand ()

and private tryRunCommand input command =
    match command with
    | Some command ->
        input.Substring(command.Name.Length)
        |> String.trimStart
        |> String.split ' '
        |> List.ofArray
        |> command.Handler
        |> Some
    | None ->
        I18n.translate (CommonText CommonInvalidCommand)
        |> showMessage

        None
