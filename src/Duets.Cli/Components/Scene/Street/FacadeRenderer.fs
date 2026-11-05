[<RequireQualifiedAccess>]
module Duets.Cli.Components.Scene.Street.FacadeRenderer

open Duets.Cli.Components.Scene
open Duets.Cli.Components.Scene.Common
open Duets.Cli.Text
open Duets.Entities
open Duets.Entities.World.Place.Type

module Styles = SceneStyles.Facade

/// Pattern of the windows of each floor, where W is a window.
let private floorPattern = " WW  WW  WW  WW "

let private extraFloorOf (place: Place) = seedOf place.Id % 2 |> abs

/// Computes the total height of the façade of the given place, including its
/// roof decoration.
let heightOf (place: Place) =
    let facade = Facades.of' place

    facade.Roof.Length
    + 1 (* Frame top. *)
    + facade.Floors
    + extraFloorOf place
    + 1 (* Sign. *)
    + (if facade.Awning.IsSome then 1 else 0)
    + 1 (* Storefront. *)

let private signStyle (skin: CitySkin.Skin) (random: System.Random) isOpen =
    if isOpen then
        skin.Signs
        |> List.item (random.Next(skin.Signs.Length))
        |> SceneStyles.bold
    else
        Styles.closedSignColor

let private storefrontRow (facade: Facades.Facade) wall lighting isOpen =
    let shopWindow, shopWindowStyle =
        match isOpen, isDark lighting with
        | false, _ -> Styles.closedShopWindow, Styles.closedShopWindowColor
        | true, true -> Styles.shopWindow, Styles.shopWindowNightColor
        | true, false -> Styles.shopWindow, Styles.shopWindowDayColor

    let mutable doorIndex = 0

    facade.Storefront
    |> fit Facades.innerWidth
    |> Seq.map (fun placeholder ->
        match placeholder with
        | 'W' -> shopWindow, Some shopWindowStyle
        | 'D' when isOpen ->
            doorIndex <- doorIndex + 1

            let door =
                if doorIndex % 2 = 1 then
                    Styles.doorLeft
                else
                    Styles.doorRight

            door, Some Styles.doorColor
        | 'D' -> Styles.shutter, Some Styles.shutterColor
        | 'G' -> Styles.graffiti, Some Styles.graffitiColor
        | 'S' -> Styles.stairs, wall
        | _ -> ' ', wall)
    |> List.ofSeq

let private floorRow wall lighting (random: System.Random) =
    floorPattern
    |> Seq.map (fun placeholder ->
        match placeholder with
        | 'W' ->
            let lit = isDark lighting && random.NextDouble() < 0.6

            let color =
                if lit then
                    Styles.litWindowColor
                else
                    Styles.unlitWindowColor

            Styles.window, Some color
        | _ -> ' ', wall)
    |> List.ofSeq

let private roofStyle (place: Place) sign wall isOpen =
    match place.PlaceType with
    | Hospital -> Some Styles.hospitalCrossStyle
    | Casino
    | Cinema
    | ConcertSpace _ when isOpen -> Some sign
    | _ -> wall

/// Draws the façade of the given place with its bottom-left corner at the
/// given position.
let draw
    (grid: SceneGrid.Grid)
    (skin: CitySkin.Skin)
    lighting
    x
    bottom
    (place: Place)
    isOpen
    (highlights: Highlights.Highlight list)
    =
    let facade = Facades.of' place
    let random = System.Random(seedOf place.Id)
    let topLeft, horizontal, topRight, side = Facades.frameGlyphs facade.Frame
    let wall = Some skin.Wall
    let sign = signStyle skin random isOpen
    let icon = place.PlaceType |> toIndex |> SceneStyles.placeIcon

    let mutable y = bottom

    let drawRow (content: (char * string option) list) =
        SceneGrid.set grid x y side wall

        content
        |> List.iteri (fun index (glyph, style) ->
            SceneGrid.set grid (x + 1 + index) y glyph style)

        SceneGrid.set grid (x + Facades.width - 1) y side wall
        y <- y - 1

    storefrontRow facade wall lighting isOpen |> drawRow

    facade.Awning
    |> Option.iter (fun awning ->
        List.replicate Facades.innerWidth (awning, Some sign) |> drawRow)

    $"{icon} {place.Name}"
    |> fit Facades.innerWidth
    |> Seq.map (fun glyph -> glyph, Some sign)
    |> List.ofSeq
    |> drawRow

    for _ in 1 .. facade.Floors + extraFloorOf place do
        floorRow wall lighting random |> drawRow

    let top =
        string topLeft
        + String.replicate Facades.innerWidth (string horizontal)
        + string topRight

    SceneGrid.stamp grid x y top wall

    highlights
    |> List.iteri (fun index highlight ->
        let glyph, style = Highlights.markerOf highlight
        SceneGrid.set grid (x + 2 + index * 2) y glyph (Some style))

    y <- y - 1

    let roof = roofStyle place sign wall isOpen

    facade.Roof
    |> List.rev
    |> List.iter (fun roofLine ->
        SceneGrid.stamp grid x y (center Facades.width roofLine) roof
        y <- y - 1)
