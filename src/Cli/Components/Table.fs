[<AutoOpen>]
module Cli.Components.Table

open Cli.Localization
open Spectre.Console

/// <summary>
/// Shows a table with the given columns and rows.
/// </summary>
/// <param name="columns">
/// Column text to be shown. Each field in the list correspond with each column
/// </param>
/// <param name="rows">
/// Row text to be shown. Each field in the list correspond with each column
/// </param>
let showTable columns rows =
    let mutable table = Table()

    columns
    |> List.iter (fun column -> table <- table.AddColumn(toString column))

    table <-
        rows
        |> List.map toString
        |> Array.ofList
        |> table.AddRow

    AnsiConsole.Write table
