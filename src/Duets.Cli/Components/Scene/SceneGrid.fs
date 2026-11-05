[<RequireQualifiedAccess>]
module Duets.Cli.Components.Scene.SceneGrid

open System.Text
open Spectre.Console

/// Defines a single cell in the grid, with the glyph to show and the Spectre
/// style (e.g. "bold yellow") that should be applied to it, if any.
type Cell = { Glyph: char; Style: string option }

/// Defines a 2D grid of cells that can be stamped with text and later rendered
/// as Spectre markup, one string per row.
type Grid =
    { Width: int
      Height: int
      Cells: Cell[,] }

let private emptyCell = { Glyph = ' '; Style = None }

/// Creates an empty grid of the given size.
let create width height =
    { Width = width
      Height = height
      Cells = Array2D.create height width emptyCell }

/// Returns whether the cell at the given position has nothing drawn on it.
/// Cells that contain a space but have a style are considered filled.
let isEmpty (grid: Grid) x y =
    x >= 0
    && y >= 0
    && x < grid.Width
    && y < grid.Height
    && grid.Cells[y, x] = emptyCell

/// Sets a single cell of the grid, ignoring positions outside of it.
let set (grid: Grid) x y glyph style =
    if x >= 0 && y >= 0 && x < grid.Width && y < grid.Height then
        grid.Cells[y, x] <- { Glyph = glyph; Style = style }

/// Stamps the given text starting at the given position. Spaces in the text
/// are treated as transparent so they don't erase what's below.
let stamp (grid: Grid) x y (text: string) style =
    text
    |> Seq.iteri (fun index glyph ->
        if glyph <> ' ' then
            set grid (x + index) y glyph style)

/// Like `stamp`, but spaces overwrite whatever was below them.
let stampOpaque (grid: Grid) x y (text: string) style =
    text |> Seq.iteri (fun index glyph -> set grid (x + index) y glyph style)

/// Renders the grid as a list of Spectre markup strings, one per row, grouping
/// contiguous cells with the same style to keep the markup small.
let toMarkup (grid: Grid) =
    [ for y in 0 .. grid.Height - 1 do
          let builder = StringBuilder()

          let flush (style: string option) (run: StringBuilder) =
              let text = run.ToString() |> Markup.Escape

              match style with
              | Some style when text.Trim() <> "" ->
                  builder.Append($"[{style}]{text}[/]") |> ignore
              | _ -> builder.Append(text) |> ignore

              run.Clear() |> ignore

          let run = StringBuilder()
          let mutable currentStyle = None

          for x in 0 .. grid.Width - 1 do
              let cell = grid.Cells[y, x]

              if cell.Style <> currentStyle then
                  flush currentStyle run
                  currentStyle <- cell.Style

              run.Append(cell.Glyph) |> ignore

          flush currentStyle run
          builder.ToString().TrimEnd() ]
