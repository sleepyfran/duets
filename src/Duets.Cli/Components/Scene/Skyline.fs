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

/// Draws the skyline centered in the grid with its bottom on the given row.
let draw
    (grid: SceneGrid.Grid)
    (lighting: Lighting)
    (skyline: Template)
    bottom
    isHidden
    seed
    =
    let random = System.Random(seed)
    let left = (grid.Width - skyline.Width) / 2
    let top = bottom - skyline.Height + 1

    skyline.Art
    |> List.iteri (fun y line ->
        line
        |> Seq.iteri (fun x glyph ->
            let x, y = left + x, top + y

            if glyph <> ' ' && not (isHidden x y) then
                let style =
                    skyline.Legend
                    |> Map.tryFind glyph
                    |> Option.orElse skyline.DefaultStyle
                    |> Option.map (fun style ->
                        if style = Furniture.lamp then
                            windowStyle lighting random
                        else
                            style)

                SceneGrid.set grid x y glyph style))
