[<AutoOpen>]
module Cli.Components.Table

open Spectre.Console

let private createTable
    title
    (columns: string list)
    (rows: (string list) list)
    =
    let mutable table = Table()

    match title with
    | Some title -> table.Title <- TableTitle(title)
    | None -> ()

    columns
    |> List.iter (fun column -> table <- table.AddColumn(column))

    rows
    |> List.iter (fun row -> table <- row |> Array.ofList |> table.AddRow)

    table

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
    createTable None columns rows |> AnsiConsole.Write

/// <summary>
/// Shows a table with the given title, columns and rows.
/// </summary>
/// <param name="title">
/// Title of the table.
/// </param>
/// <param name="columns">
/// Column text to be shown. Each field in the list correspond with each column
/// </param>
/// <param name="rows">
/// Row text to be shown. Each field in the list correspond with each column
/// </param>
let showTableWithTitle title columns rows =
    createTable (Some title) columns rows
    |> AnsiConsole.Write
