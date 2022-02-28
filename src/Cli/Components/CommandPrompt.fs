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
/// <returns>
/// The scene that the last executed command returned.
/// </returns>
let rec showCommandPrompt title availableCommands =
    let commandsWithDefaults =
        availableCommands
        @ [ PhoneCommand.get
            ExitCommand.get
            WaitCommand.get ]
        |> fun commands -> [ HelpCommand.create commands ] @ commands

    let rec promptForCommand () =
        lineBreak ()
        showMessage title

        showTextPrompt (Literal ">")
        |> String.split ' '
        |> List.ofArray
        |> fun commandWithArgs ->
            match commandWithArgs with
            | commandName :: args ->
                tryRunCommand commandsWithDefaults commandName args
            | _ -> None
        |> Option.defaultWith promptForCommand

    promptForCommand ()


and private tryRunCommand availableCommands commandName args =
    availableCommands
    |> List.tryFind (fun command -> command.Name = commandName)
    |> fun command ->
        match command with
        | Some command -> command.Handler args
        | None ->
            I18n.translate (CommonText CommonInvalidCommand)
            |> showMessage

            None
