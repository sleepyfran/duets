[<RequireQualifiedAccess>]
module Duets.Cli.Components.Scene.Street.FacadeRenderer

open Duets.Cli.Components.Scene
open Duets.Cli.Components.Scene.Common
open Duets.Cli.Text
open Duets.Entities
open Duets.Entities.World.Place.Type

module Styles = SceneStyles.Facade

let private extraFloorOf (place: Place) = seedOf place.Id % 2 |> abs

let private floorsOf (skin: CitySkin.Skin) (facade: Facades.Facade) place =
    let floors =
        if facade.Floors > 0 then
            max 1 (facade.Floors + skin.Facade.ExtraFloors)
        else
            0

    floors + extraFloorOf place

let private roofOf (skin: CitySkin.Skin) (facade: Facades.Facade) =
    if facade.Roof.IsEmpty && facade.Floors > 0 then
        skin.Facade.Roof
    else
        facade.Roof

/// Computes the height of the façade of the given place without its roof
/// decoration.
let bodyHeightOf skin (place: Place) =
    let facade = Facades.of' place

    1 (* Frame top. *)
    + floorsOf skin facade place
    + 1 (* Sign. *)
    + (if facade.Awning.IsSome then 1 else 0)
    + 1 (* Storefront. *)

/// Computes the total height of the façade of the given place, including its
/// roof decoration.
let heightOf skin (place: Place) =
    (roofOf skin (Facades.of' place)).Length + bodyHeightOf skin place

let private signStyle (skin: CitySkin.Skin) (random: System.Random) isOpen =
    if isOpen then
        skin.Signs
        |> List.item (random.Next(skin.Signs.Length))
        |> SceneStyles.bold
    else
        Styles.closedSignColor

let private storefrontRow
    (facade: Facades.Facade)
    (skin: CitySkin.Skin)
    wall
    lighting
    isOpen
    (random: System.Random)
    =
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
        | _ when random.NextDouble() < skin.Facade.Graffiti ->
            Styles.graffiti, Some Styles.graffitiColor
        | _ -> ' ', wall)
    |> List.ofSeq

let private floorRow (skin: CitySkin.Skin) wall lighting (random: System.Random) =
    skin.Facade.FloorPattern
    |> fit Facades.innerWidth
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
        | glyph -> glyph, wall)
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

    storefrontRow facade skin wall lighting isOpen random |> drawRow

    facade.Awning
    |> Option.iter (fun awning ->
        List.replicate Facades.innerWidth (awning, Some sign) |> drawRow)

    $"{icon} {place.Name}"
    |> fit Facades.innerWidth
    |> Seq.map (fun glyph -> glyph, Some sign)
    |> List.ofSeq
    |> drawRow

    let floorsTop = y

    for _ in 1 .. floorsOf skin facade place do
        floorRow skin wall lighting random |> drawRow

    if skin.Facade.VerticalSigns && floorsTop - y >= 2 then
        place.Name.ToUpperInvariant()
        |> Seq.filter System.Char.IsLetterOrDigit
        |> Seq.truncate (floorsTop - y)
        |> Seq.iteri (fun index glyph ->
            SceneGrid.set grid (x + Facades.width - 2) (y + 1 + index) glyph (Some sign))

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

    let roof =
        match facade.Roof, skin.Facade.RoofColor with
        | [], Some color -> Some color
        | _ -> roofStyle place sign wall isOpen

    roofOf skin facade
    |> List.rev
    |> List.iter (fun roofLine ->
        SceneGrid.stamp grid x y (center Facades.width roofLine) roof
        y <- y - 1)
