[<AutoOpen>]
module Cli.Components.ChoicePrompt

open Cli.Localization
open Cli.Text
open Common
open Spectre.Console

let private showSelection choice =
    showSeparator None

    toString choice
    |> CommonChoiceSelection
    |> CommonText
    |> I18n.translate
    |> showMessage

    showSeparator None

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
    let mutable selectionPrompt = SelectionPrompt<'a>()
    selectionPrompt.Title <- toString title
    selectionPrompt <- selectionPrompt.AddChoices(choices)

    selectionPrompt <-
        selectionPrompt.UseConverter(fun c -> optionTextFn c |> toString)

    AnsiConsole.Prompt(selectionPrompt)
    |> Pipe.tap (optionTextFn >> showSelection)

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
    let optionTextFnWrapper opt =
        match opt with
        | Some item -> optionTextFn item
        | None -> backText

    choices
    |> Seq.map Some
    |> fun options -> Seq.append options [ None ]
    |> showChoicePrompt title optionTextFnWrapper

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
    multiSelectionPrompt.Title <- toString title

    multiSelectionPrompt.MoreChoicesText <-
        I18n.translate (CommonText CommonMultiChoiceMoreChoices)
        |> toString

    multiSelectionPrompt.InstructionsText <-
        I18n.translate (CommonText CommonMultiChoiceInstructions)
        |> toString

    multiSelectionPrompt.Required <- true
    multiSelectionPrompt.PageSize <- 10
    multiSelectionPrompt <- multiSelectionPrompt.AddChoices(choices)

    multiSelectionPrompt <-
        multiSelectionPrompt.UseConverter(fun c -> optionTextFn c |> toString)

    AnsiConsole.Prompt(multiSelectionPrompt)
    |> List.ofSeq
