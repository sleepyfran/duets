[<AutoOpen>]
module Cli.Components.Prompt

open Cli.Text
open Common
open Entities
open Spectre.Console

/// <summary>
/// Renders a basic confirmation prompt that accepts yes/no answers.
/// </summary>
/// <param name="title">Title of the prompt to show when asking</param>
/// <returns>
/// <i>true</i> if the user answered yes or <i>false</i> if no
/// </returns>
let showConfirmationPrompt title = AnsiConsole.Confirm(title)

/// <summary>
/// Renders a basic text prompt, forcing the user to write at least one character.
/// </summary>
/// <param name="title">Title of the prompt to show when asking</param>
/// <returns>The text given by the user</returns>
let showTextPrompt title = AnsiConsole.Ask<string>(title)

/// <summary>
/// Renders a basic integer prompt, forcing the user to give a valid number.
/// </summary>
/// <param name="title">Title of the prompt to show when asking</param>
/// <returns>The integer given by the user</returns>
let showNumberPrompt title = AnsiConsole.Ask<int>(title)

/// <summary>
/// Renders a prompt that accepts lengths in the format minutes:seconds.
/// </summary>
/// <param name="title">Title of the prompt to show when asking</param>
/// <returns>The length given by the user</returns>
let showLengthPrompt title =
    let mutable lengthPrompt =
        TextPrompt<string>(title)

    let validate (length: string) =
        match Time.Length.parse length with
        | Ok _ -> ValidationResult.Success()
        | Error _ -> ValidationResult.Error(Generic.invalidLength)

    lengthPrompt.Validator <- Func.toFunc validate

    AnsiConsole.Prompt(lengthPrompt)
    |> fun length ->
        match Time.Length.parse length with
        | Ok length -> length
        | _ ->
            raise (
                invalidOp
                    "The given input was not a correct length. This should've caught by the validator but apparently it didn't :)"
            )
