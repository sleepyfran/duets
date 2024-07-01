[<AutoOpen>]
module Duets.Cli.Components.ChoicePrompt

open Duets.Common
open Spectre.Console
open Duets.Cli.Text

let private showSelection choice =
    showSeparator None

    Generic.choiceSelection choice |> showMessage

    showSeparator None

let private createChoicePrompt title optionTextFn (choices: 'a seq) =
    let mutable selectionPrompt = SelectionPrompt<'a>()

    selectionPrompt.Title <- title
    selectionPrompt <- selectionPrompt.AddChoices(choices)
    selectionPrompt <- selectionPrompt.UseConverter(optionTextFn)
    selectionPrompt

/// <summary>
/// Shows the user a list of options to choose from, forcing them to choose one
/// in order to continue.
/// </summary>
/// <param name="title">Title of the prompt</param>
/// <param name="optionTextFn">
/// Function to transform an item from the choice sequence into text
/// </param>
/// <param name="choices">Sequence of options to show to the user</param>
/// <returns>The selected item</returns>
let showChoicePrompt<'a> title (optionTextFn: 'a -> string) (choices: 'a seq) =
    createChoicePrompt title optionTextFn choices
    |> AnsiConsole.Prompt
    |> Pipe.tap (optionTextFn >> showSelection)

/// <summary>
/// Shows the user a list of options to choose from, with a text field to
/// be able to search. Forcing them to choose one in order to continue.
/// </summary>
/// <param name="title">Title of the prompt</param>
/// <param name="optionTextFn">
/// Function to transform an item from the choice sequence into text
/// </param>
/// <param name="choices">Sequence of options to show to the user</param>
/// <returns>The selected item</returns>
let showSearchableChoicePrompt<'a>
    title
    (optionTextFn: 'a -> string)
    (choices: 'a seq)
    =
    let mutable selectionPrompt = createChoicePrompt title optionTextFn choices
    selectionPrompt.SearchEnabled <- true

    selectionPrompt
    |> AnsiConsole.Prompt
    |> Pipe.tap (optionTextFn >> showSelection)

let private optionalTextFn optionTextFn backText opt =
    match opt with
    | Some item -> optionTextFn item
    | None -> backText


let private createOptionalChoicePrompt title backText optionTextFn choices =
    choices
    |> Seq.map Some
    |> fun options -> Seq.append options [ None ]
    |> createChoicePrompt title (optionalTextFn optionTextFn backText)

/// <summary>
/// Shows the user a list of options to choose from, with one extra option that
/// acts as an alternative choice to not select any of the items.
/// </summary>
/// <param name="title">Title of the prompt</param>
/// <param name="backText">
/// Text for the <i>cancel</i> or <i>back</i> option
/// </param>
/// <param name="optionTextFn">
/// Function to transform an item from the choice sequence into text
/// </param>
/// <param name="choices">Sequence of options to show to the user</param>
/// <returns>
/// An item wrapped in <i>Some</i> if the user selected an item or <i>None</i>
/// if they selected the <i>cancel</i> option.
/// </returns>
let showOptionalChoicePrompt title backText optionTextFn choices =
    createOptionalChoicePrompt title backText optionTextFn choices
    |> AnsiConsole.Prompt
    |> Pipe.tap (optionalTextFn optionTextFn backText >> showSelection)

/// <summary>
/// Shows the user a list of options to choose from, with the option to cancel
/// the selection by pressing Esc.
/// </summary>
/// <param name="title">Title of the prompt</param>
/// <param name="cancelOptionText">
/// Text for the <i>cancel</i> or <i>back</i> option
/// </param>
/// <param name="optionTextFn">
/// Function to transform an item from the choice sequence into text
/// </param>
/// <param name="choices">Sequence of options to show to the user</param>
/// <returns>
/// An item wrapped in <i>Some</i> if the user selected an item or <i>None</i>
/// if they selected the <i>cancel</i> option.
/// </returns>
let showCancellableChoicePrompt
    title
    cancelOptionText
    (optionTextFn: 'a -> string)
    (choices: 'a seq)
    =
    let mutable selectionPrompt = createChoicePrompt title optionTextFn choices

    selectionPrompt.AbortPlaceholderText <-
        $"Press Esc to {cancelOptionText}" |> Styles.faded

    match AnsiConsole.TryPrompt(selectionPrompt) with
    | true, item ->
        item |> optionTextFn |> showSelection
        Some item
    | false, _ ->
        showSelection cancelOptionText
        None

/// <summary>
/// Shows the user a list of options to choose from, with the option to cancel
/// the selection by pressing Esc.
/// </summary>
/// <param name="title">Title of the prompt</param>
/// <param name="cancelOptionText">
/// Text for the <i>cancel</i> or <i>back</i> option
/// </param>
/// <param name="optionTextFn">
/// Function to transform an item from the choice sequence into text
/// </param>
/// <param name="choices">Sequence of options to show to the user</param>
/// <returns>
/// An item wrapped in <i>Some</i> if the user selected an item or <i>None</i>
/// if they selected the <i>cancel</i> option.
/// </returns>
let showSearchableOptionalChoicePrompt
    title
    cancelOptionText
    (optionTextFn: 'a -> string)
    (choices: 'a seq)
    =
    let mutable selectionPrompt = createChoicePrompt title optionTextFn choices

    selectionPrompt.SearchEnabled <- true

    selectionPrompt.AbortPlaceholderText <-
        $"Press Esc to {cancelOptionText}" |> Styles.faded

    match AnsiConsole.TryPrompt(selectionPrompt) with
    | true, item ->
        item |> optionTextFn |> showSelection
        Some item
    | false, _ ->
        showSelection cancelOptionText
        None

/// <summary>
/// Shows the user a list of options to choose from, allowing them to choose
/// one or multiple items.
/// </summary>
/// <param name="title">Title of the prompt</param>
/// <param name="optionTextFn">
/// Function to transform an item from the choice sequence into text
/// </param>
/// <param name="choices">Sequence of options to show to the user</param>
/// <returns>The selected item(s)</returns>
let showMultiChoicePrompt<'a> title optionTextFn (choices: 'a seq) =
    let mutable multiSelectionPrompt = MultiSelectionPrompt<'a>()

    multiSelectionPrompt.Title <- title

    multiSelectionPrompt.MoreChoicesText <- Generic.multiChoiceMoreChoices

    multiSelectionPrompt.InstructionsText <- Generic.multiChoiceInstructions

    multiSelectionPrompt.Required <- true
    multiSelectionPrompt.PageSize <- 10
    multiSelectionPrompt <- multiSelectionPrompt.AddChoices(choices)

    multiSelectionPrompt <- multiSelectionPrompt.UseConverter(optionTextFn)

    AnsiConsole.Prompt(multiSelectionPrompt) |> List.ofSeq
