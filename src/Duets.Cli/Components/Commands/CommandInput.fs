namespace Duets.Cli.Components.Commands

open Duets.Cli.Text
open Duets.Common
open Spectre.Console

/// Analysis of what the player has typed so far in the command prompt, used
/// to highlight the input and show hints while the player types.
[<RequireQualifiedAccess>]
module CommandInput =
    type Analysis =
        /// Nothing has been typed yet.
        | Empty
        /// The input starts with a known command, optionally followed by
        /// arguments.
        | Matched of Command
        /// The input is not a command yet, but it's the beginning of these.
        | Partial of Command list
        /// The input matches a hidden command, which should not be revealed.
        | Hidden
        /// The input does not match any command.
        | Unknown

    let private maxCandidatesInHint = 4

    /// Returns the first command whose tokens match the beginning of the input.
    /// This is the same matching used to decide which command to run.
    let tryFind (commands: Command list) (input: string) =
        let inputTokens = String.split ' ' input |> List.ofArray

        commands
        |> List.tryFind (fun command ->
            let commandTokens = String.split ' ' command.Name |> List.ofArray

            inputTokens
            |> List.truncate commandTokens.Length
            |> List.forall2' (=) commandTokens)

    /// Analyzes the given input against the visible and hidden commands. Hidden
    /// commands are matched first, like in the dispatcher.
    let analyze
        (commands: Command list)
        (hiddenCommands: Command list)
        (input: string)
        =
        let startsWith (command: Command) =
            command.Name.StartsWith(input, System.StringComparison.Ordinal)

        if String.isEmpty input then
            Empty
        else
            match tryFind (hiddenCommands @ commands) input with
            | Some command when
                hiddenCommands |> List.exists (fun c -> c.Name = command.Name)
                ->
                Hidden
            | Some command -> Matched command
            | None ->
                match commands |> List.filter startsWith with
                | [] when hiddenCommands |> List.exists startsWith -> Hidden
                | [] -> Unknown
                | candidates -> Partial candidates

    /// Returns the possible completions for the word being typed, given the
    /// text before it. Only commands that start with all the text typed so far
    /// are considered, and the completion is the word of the command name that
    /// is in the same position as the word being typed.
    let completions (commands: Command list) (prefix: string) (word: string) =
        let typed = prefix + word
        let wordIndex = (String.split ' ' prefix).Length - 1

        commands
        |> List.filter _.Name.StartsWith(typed, System.StringComparison.Ordinal)
        |> List.choose (fun command ->
            String.split ' ' command.Name |> Array.tryItem wordIndex)
        |> List.distinct

    let private commandStyle = Style(foreground = Color.Green)
    let private argumentStyle = Style(foreground = Color.DeepSkyBlue3)
    let private unknownStyle = Style(foreground = Color.Red3)

    /// Splits the input into segments with the style each should be shown in.
    let highlight analysis (input: string) =
        match analysis with
        | Matched command ->
            let name = input.Substring(0, command.Name.Length)
            let arguments = input.Substring(command.Name.Length)

            [ yield name, Some commandStyle
              if arguments <> "" then
                  yield arguments, Some argumentStyle ]
        | Unknown -> [ input, Some unknownStyle ]
        | Empty
        | Partial _
        | Hidden -> [ input, None ]

    /// Returns the hint to show below the input, if any, as markup.
    let hint analysis =
        match analysis with
        | Matched command when not (String.isEmpty command.Description) ->
            Styles.hint command.Description |> Some
        | Partial [ command ] ->
            Command.inputHintSingleCandidate command.Name command.Description
            |> Some
        | Partial candidates ->
            let names =
                candidates
                |> List.truncate maxCandidatesInHint
                |> List.map _.Name

            let remaining = candidates.Length - names.Length

            Command.inputHintCandidates names remaining |> Some
        | Unknown -> Command.inputHintUnknown |> Some
        | Matched _
        | Empty
        | Hidden -> None
