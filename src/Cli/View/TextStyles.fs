[<RequireQualifiedAccess>]
module Cli.View.TextStyles

open Common
open Entities

/// Pre-defined style for asking the user for something.
let prompt title = $"[bold blue]{title}[/]"

/// Pre-defined style for errors.
let error text = $"[bold red]{text}[/]"

/// Pre-defined style for actions that cannot be undone.
let danger text = $"[red3]{text}[/]"

/// Pre-defined style for success messages.
let success text = $"[bold green3]{text}[/]"

/// Pre-defined style for showing money amounts.
let money (amount: Amount) = $"[bold green3]%i{amount}d$[/]"

/// Pre-defined style for referencing people in text.
let person name = $"[bold lightgreen_1]{name}[/]"

/// Pre-defined style for referencing places in text.
let place name = $"[bold lightsalmon1]{name}[/]"

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

/// Pre-defined style for crossed out things.
let crossed text = $"[strikethrough]{text}[/]"

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

/// Pre-defined styles for showing levels that range from 0 to 100.
let level l =
    match l with
    | level when level < (LanguagePrimitives.Int32WithMeasure 30) ->
        $"[red]{l}[/]"
    | level when level < (LanguagePrimitives.Int32WithMeasure 60) ->
        $"[orange1]{l}[/]"
    | level when level < (LanguagePrimitives.Int32WithMeasure 80) ->
        $"[green]{l}[/]"
    | _ -> $"[gold3]{l}[/]"

/// Pre-defined style for a title.
let title text = $"[bold underline]{text}[/]"
