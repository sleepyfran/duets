[<RequireQualifiedAccess>]
module Duets.Cli.Components.Scene.Skyline

open Duets.Cli.Components.Scene.Common
open Duets.Cli.Text
open Duets.Data.Scenes

module Furniture = SceneStyles.Furniture
module Room = SceneStyles.Room

let private windowStyle lighting (random: System.Random) =
    match isDark lighting with
    | true when random.NextDouble() < 0.5 -> Room.lampOnColor
    | true -> Furniture.skylineUnlitColor
    | false -> Room.lampOffColor

/// Draws the art with its left edge and bottom on the given column and row.
let drawAt
    (grid: SceneGrid.Grid)
    (lighting: Lighting)
    (art: Template)
    left
    bottom
    isHidden
    isSolid
    seed
    =
    let random = System.Random(seed)
    let top = bottom - art.Height + 1

    (* Solid art hides what's behind the spaces inside each of its lines. *)
    art.Art
    |> List.iteri (fun y line ->
        let first = line.Length - line.TrimStart().Length
        let last = line.TrimEnd().Length - 1

        line
        |> Seq.iteri (fun x glyph ->
            let inside = x >= first && x <= last
            let x, y = left + x, top + y

            if glyph = ' ' && isSolid && inside && not (isHidden x y) then
                SceneGrid.set grid x y ' ' None
            elif glyph <> ' ' && not (isHidden x y) then
                let style =
                    art.Legend
                    |> Map.tryFind glyph
                    |> Option.orElse art.DefaultStyle
                    |> Option.map (fun style ->
                        if style = Furniture.lamp then
                            windowStyle lighting random
                        else
                            style)

                SceneGrid.set grid x y glyph style))

/// Draws the art centered in the grid with its bottom on the given row.
let draw (grid: SceneGrid.Grid) lighting (art: Template) bottom isHidden seed =
    drawAt grid lighting art ((grid.Width - art.Width) / 2) bottom isHidden false seed
