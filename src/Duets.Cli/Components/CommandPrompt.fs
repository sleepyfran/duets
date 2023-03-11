[<AutoOpen>]
module Duets.Cli.Components.CommandPrompt

open System.Threading
open Duets.Cli.Text
open Duets.Cli.Components.Commands
open Duets.Common
open RadLine
open Spectre.Console
open System

type CommandCompletion(availableCommands: Command list) =
    interface ITextCompletion with
        member this.GetCompletions(prefix, word, _) =
            availableCommands
            |> List.filter (fun cmd ->
                String.nonEmptyContains prefix cmd.Name
                || String.nonEmptyContains word cmd.Name)
            |> List.choose (fun cmd ->
                let tokens = String.split ' ' cmd.Name

                match tokens with
                | [| fullCommand |] ->
                    if String.nonEmptyContains word fullCommand then
                        Some cmd.Name
                    else
                        None
                | [| firstWord; secondWord |] ->
                    (* Player has already introduced the full first word, autocomplete the second. *)
                    if prefix.Trim() = firstWord then
                        Some secondWord
                    (* Player has introduced part of the first word, so autocomplete it. *)
                    else if String.nonEmptyContains word firstWord then
                        Some firstWord
                    else
                        None
                | _ -> None)
            |> Seq.ofList

type private HistoryAgentMsg =
    | Add of string
    | Get of AsyncReplyChannel<string list>

type private HistoryAgent() =
    let agent =
        MailboxProcessor.Start
        <| fun inbox ->
            let rec loop history =
                async {
                    let! msg = inbox.Receive()

                    match msg with
                    | Add command -> return! loop (history @ [ command ])
                    | Get channel -> channel.Reply history

                    return! loop history
                }

            loop []

    member public this.Add command = Add command |> agent.Post
    member public this.Get() = agent.PostAndReply Get

let private historyAgent = HistoryAgent()

let private editor availableCommands =
    (* Highlight all recognized commands in green. *)
    let mutable highlighter = WordHighlighter()

    availableCommands
    |> List.iter (fun cmd ->
        (*
        Highlight command names in green, but if a command has more than one
        token (for example: "compose song"), highlight the first token in green
        and the second one in a lighter green. This is done mainly to overcome
        RadLine's lack of support of spaces in the highlighter.
        *)
        String.split ' ' cmd.Name
        |> Array.iteri (fun index token ->
            let tokenColor =
                match index with
                | 0 -> Color.Green
                | _ -> Color.SpringGreen4

            highlighter <-
                highlighter.AddWord(token, Style(foreground = tokenColor))))

    let editor =
        LineEditor(
            Prompt = LineEditorPrompt Command.prompt,
            MultiLine = false,
            Completion = CommandCompletion(availableCommands),
            Highlighter = highlighter
        )

    (*
    TODO: Consider a better way of keeping the history without duplicating it. Maybe a static editor wouldn't be that bad idea.
    *)
    historyAgent.Get() |> List.iter editor.History.Add

    (*
    Setup history with up and down arrow. By default RadLine includes the
    history navigation as CTRL + Arrow but that doesn't really work properly
    in macOS and it's confusing anyway. Since the prompt is not multi-line we
    can use the normal arrow keys, so override.
    *)
    editor.KeyBindings.Remove(ConsoleKey.UpArrow)
    editor.KeyBindings.Remove(ConsoleKey.DownArrow)
    editor.KeyBindings.Add<PreviousHistoryCommand>(ConsoleKey.UpArrow)
    editor.KeyBindings.Add<NextHistoryCommand>(ConsoleKey.DownArrow)

    editor

/// <summary>
/// Renders a command prompt with the given available commands and the exit/help
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
    let prompt = editor availableCommands

    let rec promptForCommand () =
        lineBreak ()
        showMessage title

        prompt.ReadLine(CancellationToken.None)
        |> Async.AwaitTask
        |> Async.RunSynchronously
        |> fun input ->
            historyAgent.Add input

            let inputTokens = String.split ' ' input |> List.ofArray

            availableCommands
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
        Generic.invalidCommand |> showMessage

        None
