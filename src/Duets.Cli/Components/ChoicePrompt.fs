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
    selectionPrompt <- selectionPrompt.UseConverter(fun c -> optionTextFn c)
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
let showChoicePrompt<'a> title optionTextFn (choices: 'a seq) =
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
let showSearchableChoicePrompt<'a> title optionTextFn (choices: 'a seq) =
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

let private createCancellableChoicePrompt
    title
    cancelOptionText
    optionTextFn
    (choices: 'a seq)
    =
    let mutable selectionPrompt =
        choices
        |> Seq.map Some
        |> createChoicePrompt title (optionalTextFn optionTextFn cancelOptionText)

    selectionPrompt.CancelResult <- fun () -> None

    selectionPrompt.CancelText <-
        $"Press Esc to {cancelOptionText}" |> Styles.faded

    selectionPrompt

let private showCancellableChoicePrompt' cancelOptionText optionTextFn prompt =
    prompt
    |> AnsiConsole.Prompt
    |> Pipe.tap (optionalTextFn optionTextFn cancelOptionText >> showSelection)

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
    optionTextFn
    (choices: 'a seq)
    =
    createCancellableChoicePrompt title cancelOptionText optionTextFn choices
    |> showCancellableChoicePrompt' cancelOptionText optionTextFn

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
    optionTextFn
    (choices: 'a seq)
    =
    let mutable selectionPrompt =
        createCancellableChoicePrompt title cancelOptionText optionTextFn choices

    selectionPrompt.SearchEnabled <- true

    selectionPrompt
    |> showCancellableChoicePrompt' cancelOptionText optionTextFn

type private GroupedChoice<'a> =
    | Group of string
    | Choice of 'a
    | MoreChoices
    | Back

let private groupedChoiceText optionTextFn moreText backText choice =
    match choice with
    | Group name -> Styles.faded name
    | Choice item -> optionTextFn item
    | MoreChoices -> moreText
    | Back -> backText

let private showGroupedChoicePrompt'
    title
    (backText: string option)
    moreText
    optionTextFn
    (groups: (string * 'a list) list)
    (rest: 'a list)
    =
    let textFn =
        groupedChoiceText
            optionTextFn
            moreText
            (backText |> Option.defaultValue "")

    let mutable selectionPrompt = SelectionPrompt<GroupedChoice<'a>>()
    selectionPrompt.Title <- title

    groups
    |> List.filter (snd >> List.isEmpty >> not)
    |> List.iter (fun (name, items) ->
        selectionPrompt <-
            selectionPrompt.AddChoiceGroup(Group name, items |> List.map Choice))

    if not (List.isEmpty rest) then
        selectionPrompt <- selectionPrompt.AddChoices([ MoreChoices ])

    if backText.IsSome then
        selectionPrompt <- selectionPrompt.AddChoices([ Back ])

    selectionPrompt <- selectionPrompt.UseConverter(textFn)

    match AnsiConsole.Prompt selectionPrompt with
    | Choice item ->
        showSelection (optionTextFn item)
        Some item
    | MoreChoices ->
        match backText with
        | Some backText ->
            showSearchableOptionalChoicePrompt title backText optionTextFn rest
        | None -> showSearchableChoicePrompt title optionTextFn rest |> Some
    | Back ->
        showSelection (textFn Back)
        None
    (* Groups can't be selected since the prompt only allows selecting leaves. *)
    | Group _ -> None

/// Shows the user a list of options split in groups, plus an extra option that
/// opens a searchable list with the rest of the options.
let showGroupedChoicePrompt title moreText optionTextFn groups rest =
    showGroupedChoicePrompt' title None moreText optionTextFn groups rest
    |> Option.get

/// Shows the user a list of options split in groups, plus an extra option that
/// opens a searchable list with the rest of the options and a back option.
let showGroupedOptionalChoicePrompt
    title
    backText
    moreText
    optionTextFn
    groups
    rest
    =
    showGroupedChoicePrompt' title (Some backText) moreText optionTextFn groups rest

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

    multiSelectionPrompt <-
        multiSelectionPrompt.UseConverter(fun c -> optionTextFn c)

    AnsiConsole.Prompt(multiSelectionPrompt) |> List.ofSeq
