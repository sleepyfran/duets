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
    (*
    RadLine replaces the word with the completion and asks again on the next
    Tab, cycling to the next item only if the list still contains the current
    word. Since the word is now the full completion, recomputing would only
    return that one, so keep returning the last list while the word is in it.
    *)
    let mutable lastCompletions: (string * string list) option = None

    interface ITextCompletion with
        member this.GetCompletions(prefix, word, _) =
            match lastCompletions with
            | Some(lastPrefix, completions) when
                lastPrefix = prefix && completions |> List.contains word
                ->
                completions
            | _ ->
                let completions =
                    CommandInput.completions availableCommands prefix word

                lastCompletions <- Some(prefix, completions)
                completions
            |> Seq.ofList

/// Highlights recognized commands in green and their arguments in blue, and
/// any input that can't become a command in red.
type CommandHighlighter(analyze: string -> CommandInput.Analysis) =
    interface IHighlighter with
        member this.BuildHighlightedText(text) =
            let paragraph = Paragraph()

            CommandInput.highlight (analyze text) text
            |> List.iter (fun (segment, style) ->
                paragraph.Append(segment, Option.toNullable style) |> ignore)

            paragraph

/// Renders a hint for the current input in the line right below the prompt.
type CommandHintRenderer(analyze: string -> CommandInput.Analysis) =
    static let saveCursor = "\u001b7"
    static let restoreCursor = "\u001b8"
    static let nextLineStart = "\u001b[1B\u001b[1G"
    static let clearLine = "\u001b[2K"

    static let fitToWidth (hint: string) =
        let maxLength = AnsiConsole.Profile.Width - 1
        let plainHint = Markup.Remove hint

        if plainHint.Length <= maxLength then
            hint
        else
            plainHint.Substring(0, maxLength - 1) + "…"
            |> Markup.Escape
            |> Styles.hint

    interface ILineDecorationRenderer with
        member this.RenderLineDecoration(buffer) =
            let hint =
                CommandInput.hint (analyze buffer.Content)
                |> Option.map (fitToWidth >> Markup >> AnsiConsole.Console.ToAnsi)
                |> Option.defaultValue ""

            AnsiConsole.Console.WriteAnsi(
                saveCursor + nextLineStart + clearLine + hint + restoreCursor
            )

    /// Makes sure there's an empty line below the cursor for the hint, even if
    /// the prompt is at the bottom of the terminal.
    static member reserveHintLine() =
        AnsiConsole.WriteLine()
        AnsiConsole.Cursor.MoveUp()

    /// Clears the hint once the input has been submitted, which leaves the
    /// cursor at the start of the hint line.
    static member clearHintLine() = AnsiConsole.Console.WriteAnsi clearLine

/// Shortcut to enter the cheat menu without typing the full secret command.
type CheatMenuShortcutCommand() =
    inherit LineEditorCommand()

    static let lastPressStateKey = "cheatMenuShortcutLastPress"
    static let maxTimeBetweenPresses = TimeSpan.FromSeconds 1.0

    override _.Execute(context) =
        let now = DateTime.UtcNow

        let lastPress =
            context.GetState(lastPressStateKey, fun () -> DateTime.MinValue)

        if now - lastPress <= maxTimeBetweenPresses then
            let buffer = context.Buffer
            buffer.Move(0) |> ignore
            buffer.Clear(0, buffer.Length) |> ignore
            buffer.Insert(Cheats.Index.enterCommand.Name)
            context.Submit(SubmitAction.Submit)
        else
            context.SetState(lastPressStateKey, now)

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

/// Replaces the input with the previous (negative step) or next (positive
/// step) command in the history. Unlike RadLine's history commands, this edits
/// the buffer in place so that the hint below the prompt gets updated.
type private HistoryNavigationCommand(step: int) =
    inherit LineEditorCommand()

    static let indexStateKey = "historyIndex"
    static let draftStateKey = "historyDraft"

    override _.Execute(context) =
        let history = historyAgent.Get() |> Array.ofList
        let buffer = context.Buffer

        (* The position right after the last command holds what was being typed. *)
        let draftIndex = history.Length
        let index = context.GetState(indexStateKey, fun () -> draftIndex)
        let newIndex = index + step |> max 0 |> min draftIndex

        if newIndex <> index then
            if index = draftIndex then
                context.SetState(draftStateKey, buffer.Content)

            let content =
                if newIndex = draftIndex then
                    context.GetState(draftStateKey, fun () -> "")
                else
                    history[newIndex]

            buffer.Move(0) |> ignore
            buffer.Clear(0, buffer.Length) |> ignore
            buffer.Insert(content)
            buffer.MoveEnd() |> ignore
            context.SetState(indexStateKey, newIndex)

let private editor availableCommands hiddenCommands =
    let analyze = CommandInput.analyze availableCommands hiddenCommands

    let editor =
        LineEditor(
            Prompt = LineEditorPrompt Command.prompt,
            MultiLine = false,
            Completion = CommandCompletion(availableCommands),
            Highlighter = CommandHighlighter(analyze),
            LineDecorationRenderer = CommandHintRenderer(analyze)
        )

    (*
    Setup history with up and down arrow. By default RadLine includes the
    history navigation as CTRL + Arrow but that doesn't really work properly
    in macOS and it's confusing anyway. Since the prompt is not multi-line we
    can use the normal arrow keys, so override.
    *)
    editor.KeyBindings.Remove(ConsoleKey.UpArrow)
    editor.KeyBindings.Remove(ConsoleKey.DownArrow)

    editor.KeyBindings.Add(
        ConsoleKey.UpArrow,
        fun () -> HistoryNavigationCommand(-1)
    )

    editor.KeyBindings.Add(
        ConsoleKey.DownArrow,
        fun () -> HistoryNavigationCommand(1)
    )

    (*
    RadLine binds going back through the completions to Ctrl+Tab, which most
    terminals don't send, so use Shift+Tab instead.
    *)
    editor.KeyBindings.Add(
        ConsoleKey.Tab,
        ConsoleModifiers.Shift,
        fun () -> AutoCompleteCommand(AutoComplete.Previous)
    )

    (* Ctrl+G twice in a row enters the cheat menu. *)
    editor.KeyBindings.Add<CheatMenuShortcutCommand>(
        ConsoleKey.G,
        ConsoleModifiers.Control
    )

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
    /// Adds a way to enter a "secret" cheat/debug prompt from any command
    /// prompt.
    let hiddenCommands = [ Cheats.Index.enterCommand ]

    let prompt = editor availableCommands hiddenCommands

    let rec promptForCommand () =
        lineBreak ()
        showMessage title

        CommandHintRenderer.reserveHintLine ()

        prompt.ReadLine(CancellationToken.None)
        |> Async.AwaitTask
        |> Async.RunSynchronously
        |> fun input ->
            CommandHintRenderer.clearHintLine ()

            if not (String.isEmpty input) then
                historyAgent.Add input

            CommandInput.tryFind (hiddenCommands @ availableCommands) input
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
