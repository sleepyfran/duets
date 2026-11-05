[<RequireQualifiedAccess>]
module Duets.Cli.Components.Scene.Street.Ground

open Duets.Cli.Components.Scene
open Duets.Cli.Components.Scene.Common
open Duets.Cli.Text
open Duets.Entities

module People = SceneStyles.People

/// Draws the sidewalk and, if given, the player and the people around them.
let drawSidewalk
    (grid: SceneGrid.Grid)
    (skin: CitySkin.Skin)
    y
    people
    seed
    =
    let random = System.Random(seed)

    SceneGrid.stampOpaque
        grid
        0
        y
        (repeat grid.Width skin.Sidewalk)
        (Some skin.SidewalkColor)

    people
    |> Option.iter (fun (knownPeople: Character list, unknownPeople) ->
        let playerX = grid.Width / 2
        SceneGrid.set grid playerX y People.player (Some People.playerStyle)

        let placePerson glyph style =
            let x = random.Next(grid.Width)

            if x <> playerX then
                SceneGrid.set grid x y glyph (Some style)

        knownPeople
        |> List.iter (fun _ -> placePerson People.known People.knownStyle)

        unknownPeople
        |> List.iter (fun _ -> placePerson People.unknown People.unknownStyle))

/// Draws the road along the whole width of the grid.
let drawRoad (grid: SceneGrid.Grid) (skin: CitySkin.Skin) y =
    let road = repeat grid.Width skin.Road
    SceneGrid.stampOpaque grid 0 y road (Some skin.RoadColor)
