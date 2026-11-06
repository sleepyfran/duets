[<RequireQualifiedAccess>]
module Duets.Cli.Components.Scene.Street.Furniture

open Duets.Cli.Components.Scene
open Duets.Cli.Components.Scene.Common
open Duets.Cli.Text
open Duets.Entities

module Furniture = SceneStyles.Furniture
module Room = SceneStyles.Room

let private propChance = 0.6

let private resolveStyle lighting style =
    match style with
    | style when style = Furniture.lamp ->
        if isDark lighting then Room.lampOnColor else Room.lampOffColor
    | style -> style

/// Draws the props of the city in some of the given columns.
let drawProps
    (grid: SceneGrid.Grid)
    (skin: CitySkin.Skin)
    lighting
    columns
    groundRow
    seed
    =
    let random = System.Random(seed)

    match skin.StreetProps with
    | [] -> ()
    | props ->
        columns
        |> List.iter (fun x ->
            if random.NextDouble() < propChance then
                props
                |> List.item (random.Next(props.Length))
                |> List.iteri (fun index (glyph, style) ->
                    SceneGrid.set
                        grid
                        x
                        (groundRow - 1 - index)
                        glyph
                        (Some(resolveStyle lighting style))))

/// Draws the traffic of the city on the road.
let drawTraffic
    (grid: SceneGrid.Grid)
    (skin: CitySkin.Skin)
    (lighting: Lighting)
    y
    seed
    =
    let random = System.Random(seed)

    let count =
        match lighting.DayMoment with
        | Night
        | Midnight -> grid.Width / 80
        | _ -> grid.Width / 40

    match skin.Vehicles with
    | [] -> ()
    | vehicles when count > 0 ->
        (* Each vehicle gets its own part of the road so they don't overlap. *)
        let stretch = grid.Width / count

        for index in 0 .. count - 1 do
            let (art: string), style =
                vehicles |> List.item (random.Next(vehicles.Length))

            let room = max 1 (stretch - art.Length)
            let x = index * stretch + random.Next(room)
            SceneGrid.stampOpaque grid x y art (Some style)
    | _ -> ()
