[<RequireQualifiedAccess>]
module Cli.View.TextStyles

open Entities

/// Pre-defined style for asking the user for something.
let prompt title = $"[bold blue]%s{title}[/]"

/// Pre-defined style for errors.
let error text = $"[bold red]%s{text}[/]"

/// Pre-defined style for instructions on how to invoke something.
let instruction text = $"[italic]%s{text}[/]"

/// Pre-defined style for showing money amounts.
let money (amount: Amount) = $"[bold green3]%i{amount}d$[/]"

/// Pre-defined style for referencing people in text.
let person name = $"[bold lightgreen_1]%s{name}[/]"

/// Pre-defined style for referencing places in text.
let place name = $"[bold lightsalmon1]%s{name}[/]"

/// Pre-defined style for referencing actions in text.
let action name = $"[bold deepskyblue2]%s{name}[/]"

/// Pre-defined style for referencing objects in text.
let object name = $"[bold cadetblue]%s{name}[/]"

/// Pre-defined style for referencing albums in text.
let album name = $"[bold springgreen2]%s{name}[/]"

/// Pre-defined style for referencing information in text.
let information text = $"[reverse]%s{text}[/]"
