[<AutoOpen>]
module Cli.Components.Layout

open Spectre.Console

/// Clears the whole console buffer and shows an empty screen.
let clearScreen () = System.Console.Clear()

/// Prints an empty line in the console.
let lineBreak () = AnsiConsole.WriteLine()
