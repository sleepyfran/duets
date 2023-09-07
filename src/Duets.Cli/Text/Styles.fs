[<RequireQualifiedAccess>]
module rec Duets.Cli.Text.Styles


open Duets.Common
open Duets.Entities

/// Pre-defined style for asking the user for something.
let prompt title = $"[bold blue]{title}[/]"

/// Pre-defined style for errors.
let error text = $"[bold red]{text}[/]"

/// Pre-defined style for actions that cannot be undone.
let danger text = $"[red3]{text}[/]"

/// Pre-defined style for warnings and actions that might have unwanted consequences.
let warning text = $"[orange3]{text}[/]"

/// Pre-defined style for success messages.
let success text = $"[bold green3]{text}[/]"

/// Pre-defined style for showing numbers.
let number (value: int<_>) = System.String.Format("{0:#,0}", value)

/// Pre-defined style for showing decimal numbers.
let decimal (value: decimal<_>) =
    System.String.Format("{0:#,##0.00}", value)

/// Pre-defined style for showing money amounts.
let money (amount: Amount) = $"[bold green3]{decimal amount}d$[/]"

/// Pre-defined style for referencing people in text.
let person name = $"[bold lightgreen_1]{name}[/]"

/// Pre-defined style for showing percentages.
let percentage value = $"{value |> Level.from}%%"

/// Pre-defined style for referencing places in text.
let place name = $"[bold lightsalmon1]{name}[/]"

/// Pre-defined style for referencing rooms in text.
let room name = $"[bold salmon1]{name}[/]"

/// Pre-defined style for referencing items in text.
let item name = $"[bold lightsalmon1]{name}[/]"

/// Pre-defined style for referencing time in text.
let time text = $"[bold grey70]{text}[/]"

/// Pre-defined style for referencing actions in text.
let action name = $"[bold deepskyblue2]{name}[/]"

/// Pre-defined style for referencing objects in text.
let object name = $"[bold cadetblue]{name}[/]"

/// Pre-defined style for referencing bands.
let band name = $"[italic]{name}[/]"

/// Pre-defined style for referencing albums in text.
let album name = $"[bold springgreen2]{name}[/]"

/// Pre-defined style for referencing songs in text.
let song name = album name

/// Pre-defined style for referencing information in text.
let information text = $"[underline]{text}[/]"

/// Pre-defined style for showing text that does not draw attention immediately.
let faded text = $"[grey]{text}[/]"

/// Pre-defined style for showing text that draws attention.
let highlight text = $"[bold deepskyblue3]{text}[/]"

/// Pre-defined style for table headers.
let header text = $"[bold]{text}[/]"

/// Pre-defined style for crossed out things.
let crossed text = $"[strikethrough]{text}[/]"

/// Pre-defined style for genres.
let genre text = $"[grey]{text}[/]"

/// Pre-defined styles for showing a progress step. Shows with a random color.
let progress text =
    [ "deepskyblue3"
      "deepskyblue3_1"
      "dodgerblue1"
      "springgreen4"
      "springgreen3_1"
      "springgreen2_1"
      "springgreen1" ]
    |> List.sample
    |> fun color -> $"[{color}]{text}[/]"

module Level =
    /// Pre-defined style for a bad level.
    let bad text = $"[red]{text}[/]"

    /// Pre-defined style for a normal level.
    let normal text = $"[orange1]{text}[/]"

    /// Pre-defined style for a good level.
    let good text = $"[cadetblue]{text}[/]"

    /// Pre-defined style for a great level.
    let great text = $"[green3]{text}[/]"

    /// Pre-defined styles for showing levels that range from 0 to 100.
    let from l =
        match l with
        | level when level < (LanguagePrimitives.Int32WithMeasure 30) -> bad l
        | level when level < (LanguagePrimitives.Int32WithMeasure 60) ->
            normal l
        | level when level < (LanguagePrimitives.Int32WithMeasure 80) -> good l
        | _ -> great l

    /// Pre-defined styles for showing levels that are worse the closer they are to 100.
    let fromInverted l =
        match l with
        | level when level < (LanguagePrimitives.Int32WithMeasure 30) -> great l
        | level when level < (LanguagePrimitives.Int32WithMeasure 60) -> good l
        | level when level < (LanguagePrimitives.Int32WithMeasure 80) ->
            normal l
        | _ -> bad l

/// Pre-defined style for a title.
let title text = $"[bold underline]{text}[/]"

module Spacing =
    /// Adds a new line with a few spaces to align the next line with the
    /// previous one in choice prompts.
    let choicePromptNewLine = "\n  "
