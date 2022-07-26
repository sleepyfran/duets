[<AutoOpen>]
module Cli.Components.Table

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
let showTable (columns: string list) (rows: (string list) list) =
    let mutable table = Table()

    columns
    |> List.iter (fun column -> table <- table.AddColumn(column))

    rows
    |> List.iter (fun row -> table <- row |> Array.ofList |> table.AddRow)

    AnsiConsole.Write table
