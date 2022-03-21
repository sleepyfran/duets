[<AutoOpen>]
module Cli.Components.CommandPrompt

open Cli.Text
open Cli.Components.Commands
open Common

/// <summary>
/// Renders a command prompt that accepts only the given available commands and
/// the default ones that are always available (phone, exit, help). The prompt
/// will keep on asking for a command as long as the handler of that command
/// returns None instead of some Scene. This is done so commands that just do
/// a side-effect or show some input but then need to return to the prompt (like
/// help) can indicate this.
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
    let commandsWithDefaults =
        availableCommands
        @ [ PhoneCommand.get; WaitCommand.get ]

    showCommandPromptWithoutDefaults title commandsWithDefaults

/// Like `showCommandPrompt` but with only the 'help' and 'exit' commands
/// available aside from the given commands.
/// <param name="title">
/// Title of the prompt to show when asking for a command
/// </param>
/// <param name="availableCommands">
/// List of commands that are available to be executed
/// </param>
/// <returns>
/// The scene that the last executed command returned.
/// </returns>
and showCommandPromptWithoutDefaults title availableCommands =
    let commandsWithEssentials =
        availableCommands @ [ ExitCommand.get ]
        |> fun commands -> [ HelpCommand.create commands ] @ commands

    let rec promptForCommand () =
        lineBreak ()
        showMessage title

        showTextPrompt (Literal ">")
        |> fun input ->
            commandsWithEssentials
            |> List.tryFind (fun command -> input.StartsWith(command.Name))
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
    | None ->
        I18n.translate (CommonText CommonInvalidCommand)
        |> showMessage

        None
