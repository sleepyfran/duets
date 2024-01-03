[<AutoOpen>]
module Duets.Cli.Components.Prompt

open Duets.Cli.Text
open Duets.Common
open Duets.Entities
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
/// Renders a basic text prompt, allowing the user to write nothing.
/// </summary>
/// <param name="title">Title of the prompt to show when asking</param>
/// <returns>The text given by the user or None if empty.</returns>
let showOptionalTextPrompt title =
    let mutable txtPrompt = TextPrompt<string> title
    txtPrompt.AllowEmpty <- true
    let result = AnsiConsole.Prompt txtPrompt

    match result with
    | "" -> None
    | _ -> Some result

/// <summary>
/// Renders a basic integer prompt, forcing the user to give a valid number.
/// </summary>
/// <param name="title">Title of the prompt to show when asking</param>
/// <returns>The integer given by the user</returns>
let showNumberPrompt title = AnsiConsole.Ask<int>(title)

/// <summary>
/// Renders a basic integer prompt that forces the user to give a valid number
/// between the given inclusive range.
/// </summary>
/// <param name="min">Minimum number allowed</param>
/// <param name="max">Maximum number allowed</param>
/// <param name="title">Title of the prompt to show when asking</param>
let showRangedNumberPrompt (min: int<'a>) (max: int<'a>) title =
    let title = $"""{title} {Styles.faded $"(Between {min} and {max})"}"""

    TextPrompt<int<'a>>(
        title,
        Validator =
            (fun number ->
                match number with
                | n when n >= min && n <= max -> ValidationResult.Success()
                | n ->
                    ValidationResult.Error(
                        $"{n} is not between {min} and {max}. Choose another number"
                        |> Styles.error
                    ))
    )
    |> AnsiConsole.Prompt

/// <summary>
/// Renders a decimal prompt that forces the user to give a valid decimal
/// number between the given inclusive range.
/// </summary>
/// <param name="min">Minimum number allowed</param>
/// <param name="max">Maximum number allowed</param>
/// <param name="title">Title of the prompt to show when asking</param>
let showRangedDecimalPrompt min max title =
    let title = $"""{title} {Styles.faded $"(Between {min} and {max})"}"""

    TextPrompt<decimal>(
        title,
        Validator =
            (fun number ->
                match number with
                | n when n >= min && n <= max -> ValidationResult.Success()
                | n ->
                    ValidationResult.Error(
                        $"{n} is not between {min} and {max}. Choose another number"
                        |> Styles.error
                    ))
    )
    |> AnsiConsole.Prompt

/// <summary>
/// Renders a basic decimal prompt, forcing the user to give a valid number.
/// </summary>
/// <param name="title">Title of the prompt to show when asking</param>
/// <returns>The decimal given by the user</returns>
let showDecimalPrompt title = AnsiConsole.Ask<decimal>(title)

/// <summary>
/// Renders a prompt that accepts a date in the dd/mm/YYYY format.
/// </summary>
/// <param name="title">Title of the prompt to show when asking</param>
/// <returns>The date given by the user</returns>
let showTextDatePrompt title =
    let mutable datePrompt = TextPrompt<string>(title)

    let validate (date: string) =
        match Calendar.Parse.date date with
        | Some _ -> ValidationResult.Success()
        | None -> ValidationResult.Error(Generic.invalidDate)

    datePrompt.Validator <- Func.toFunc validate

    AnsiConsole.Prompt(datePrompt)
    |> fun date ->
        match Calendar.Parse.date date with
        | Some date -> date
        | None ->
            raise (
                invalidOp
                    "The given input was not a correct date. This should've been caught by the validator but apparently it didn't :)"
            )

type private InteractiveDatePromptOption =
    | Date of Date
    | NextMonth

/// <summary>
/// Renders a choice prompt with all the dates of the month after the given
/// first date. Returns some date if character selected something, none if the
/// prompt was cancelled.
/// </summary>
let rec showInteractiveDatePrompt title firstDate =
    let monthDays = Calendar.Query.monthDaysFrom firstDate |> Seq.map Date

    let nextMonthDate = Calendar.Query.firstDayOfNextMonth firstDate

    let toText opt =
        match opt with
        | Date date -> Generic.dateWithDay date
        | NextMonth -> Generic.moreDates

    let selectedDate =
        showOptionalChoicePrompt
            title
            Generic.cancel
            toText
            (seq {
                yield! monthDays
                yield NextMonth
            })

    match selectedDate with
    | Some(Date date) -> Some date
    | Some NextMonth -> showInteractiveDatePrompt title nextMonthDate
    | None -> None

/// <summary>
/// Renders a prompt that accepts lengths in the format minutes:seconds.
/// </summary>
/// <param name="title">Title of the prompt to show when asking</param>
/// <returns>The length given by the user</returns>
let showLengthPrompt title =
    let mutable lengthPrompt = TextPrompt<string>(title)

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
                    "The given input was not a correct length. This should've been caught by the validator but apparently it didn't :)"
            )

/// <summary>
/// Renders a prompt that blocks the user until they press any key.
/// </summary>
let showContinuationPrompt () =
    "Press any key to continue..." |> showMessage

    AnsiConsole.Console.Input.ReadKey(true) |> ignore
