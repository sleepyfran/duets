[<RequireQualifiedAccess>]
module Duets.Cli.Components.Scene.Room.RoomRenderer

open Duets.Cli.Components.Scene
open Duets.Cli.Components.Scene.Common
open Duets.Cli.Text
open Duets.Data.Scenes
open Duets.Entities

module People = SceneStyles.People
module Styles = SceneStyles.Room
module Weather = SceneStyles.Weather

/// Everything needed to build a room scene.
type RoomContext =
    { Skin: CitySkin.Skin
      Lighting: Lighting
      Quality: Quality
      /// Seed of the place and room, keeps the props the same between visits.
      Seed: int
      KnownPeople: int
      UnknownPeople: int
      /// Members of the band other than the player, only during concerts.
      BandMembers: int
      Items: Item list
      Exits: ExitTarget list
      /// How full the venue is, from 0 to 1.
      Crowd: float
      ConcertOngoing: bool
      Cuisine: RestaurantCuisine option }

let private resolveStyle (context: RoomContext) accent (style: string) =
    style
        .Replace("$wall", context.Skin.Wall)
        .Replace("$accent", accent)
        .Replace(
            "$lamp",
            if isDark context.Lighting then
                Styles.lampOnColor
            else
                Styles.lampOffColor
        )
        .Replace(
            "$stage",
            if context.ConcertOngoing then
                Styles.stageOnColor
            else
                Styles.stageOffColor
        )

/// Cuts the art to the given length, keeping the last character so that
/// things like the metro still have an end.
let private fitArt length (art: string) =
    if art.Length <= length then art
    elif length <= 1 then art.Substring(0, length)
    else art.Substring(0, length - 1) + string art[art.Length - 1]

let private drawInRun (grid: SceneGrid.Grid) (run: Run) (art: string) styleOf =
    let art = fitArt run.Length art
    let x = run.X + (run.Length - art.Length) / 2

    art
    |> Seq.iteri (fun index glyph ->
        if glyph <> ' ' then
            SceneGrid.set grid (x + index) run.Y glyph (styleOf glyph))

let private shuffle (random: System.Random) list =
    list |> List.map (fun item -> random.Next(), item) |> List.sortBy fst |> List.map snd

let private windowCell (lighting: Lighting) (random: System.Random) =
    let pick (glyphs: char array) = glyphs[random.Next(glyphs.Length)]

    match lighting.Weather with
    | WeatherCondition.Rainy -> pick Weather.rain, Weather.rainColor
    | WeatherCondition.Stormy -> pick Weather.storm, Weather.stormColor
    | WeatherCondition.Snowy -> pick Weather.snow, Weather.snowColor
    | _ when isNight lighting ->
        if random.NextDouble() < 0.3 then
            pick Weather.stars, Weather.starsColor
        else
            Styles.nightSky, Weather.starsColor
    | WeatherCondition.Cloudy -> Styles.daySky, Styles.cloudySkyColor
    | _ when lighting.DayMoment = Evening ->
        Styles.daySky, Styles.sunsetSkyColor
    | _ -> Styles.daySky, Styles.daySkyColor

/// Places the items in the item slots of the template. Slots with a category
/// get filled first and the rest of items go to the generic ones.
let private assignItems (template: Template) (items: Item list) =
    let itemRuns =
        template.Slots
        |> Map.toList
        |> List.choose (fun (marker, slot) ->
            match slot.Kind with
            | SlotKind.Item category ->
                Template.runsOf template marker
                |> List.map (fun run -> category, run)
                |> Some
            | _ -> None)
        |> List.concat
        |> List.sortBy (fun (category, _) -> category = ItemCategory.Any)

    itemRuns
    |> List.fold
        (fun (assigned, remaining) (category, run) ->
            let fits item =
                category = ItemCategory.Any || ItemCategory.ofItem item = category

            match remaining |> List.tryFindIndex fits with
            | Some index ->
                (run, remaining[index]) :: assigned,
                remaining |> List.removeAt index
            | None -> assigned, remaining)
        ([], items)
    |> fst

/// Builds the scene of the given template.
let build (context: RoomContext) (template: Template) =
    let grid = SceneGrid.create template.Width template.Height
    let accentRandom = System.Random(context.Seed)

    let accent =
        context.Skin.Signs
        |> List.item (accentRandom.Next(context.Skin.Signs.Length))

    let styleOf glyph =
        template.Legend
        |> Map.tryFind glyph
        |> Option.orElse template.DefaultStyle
        |> Option.map (resolveStyle context accent)

    template.Art
    |> List.iteri (fun y line ->
        line
        |> Seq.iteri (fun x glyph ->
            match template.Slots |> Map.tryFind glyph with
            | Some slot when slot.Fallback <> ' ' ->
                SceneGrid.set grid x y slot.Fallback (styleOf slot.Fallback)
            | Some _ -> ()
            | None when glyph <> ' ' -> SceneGrid.set grid x y glyph (styleOf glyph)
            | None -> ()))

    let clearRun (run: Run) =
        for x in run.X .. run.X + run.Length - 1 do
            SceneGrid.set grid x run.Y ' ' None

    template.Slots
    |> Map.iter (fun marker slot ->
        (* Separate random per slot, otherwise people coming in would move the props around. *)
        let random = System.Random(context.Seed + 7919 * int marker)
        let cells () = Template.cellsOf template marker
        let runs () = Template.runsOf template marker

        let fillCells count glyphs style =
            cells ()
            |> shuffle random
            |> List.truncate count
            |> List.iter (fun (x, y) ->
                let glyph = glyphs |> Array.item (random.Next(Array.length glyphs))
                SceneGrid.set grid x y glyph (Some style))

        match slot.Kind with
        | SlotKind.Player ->
            runs ()
            |> List.tryHead
            |> Option.iter (fun run ->
                drawInRun grid run (string People.player) (fun _ ->
                    Some People.playerStyle))
        | SlotKind.Npc ->
            let positions = cells () |> shuffle random

            positions
            |> List.truncate (context.KnownPeople + context.UnknownPeople)
            |> List.iteri (fun index (x, y) ->
                if index < context.KnownPeople then
                    SceneGrid.set grid x y People.known (Some People.knownStyle)
                else
                    SceneGrid.set grid x y People.unknown (Some People.unknownStyle))
        | SlotKind.Band ->
            fillCells context.BandMembers [| People.known |] People.knownStyle
        | SlotKind.Crowd glyphs ->
            let cells = cells ()
            let count = int (System.Math.Ceiling(float cells.Length * context.Crowd))

            let glyphs =
                if List.isEmpty glyphs then Styles.crowd else Array.ofList glyphs

            fillCells count glyphs Styles.crowdColor
        | SlotKind.Window ->
            cells ()
            |> List.iter (fun (x, y) ->
                let glyph, style = windowCell context.Lighting random
                SceneGrid.set grid x y glyph (Some style))
        | SlotKind.Exit target when context.Exits |> List.contains target ->
            let arrow =
                match target with
                | ExitTarget.Room direction -> SceneStyles.directionArrow direction
                | ExitTarget.Street -> string Styles.streetExit

            runs ()
            |> List.iter (fun run ->
                clearRun run
                drawInRun grid run arrow (fun _ -> Some Styles.exitColor))
        | SlotKind.Exit _ -> ()
        | SlotKind.Cuisine ->
            context.Cuisine
            |> Option.iter (fun cuisine ->
                runs ()
                |> List.iter (fun run ->
                    clearRun run

                    drawInRun grid run (Styles.cuisineSign cuisine) (fun _ ->
                        Some(SceneStyles.bold accent))))
        | SlotKind.Prop tiers ->
            let alternatives =
                tiers |> List.item (Template.tierFor context.Quality tiers.Length)

            runs ()
            |> List.iter (fun run ->
                let prop = alternatives |> List.item (random.Next(alternatives.Length))

                if prop <> "" then
                    clearRun run
                    drawInRun grid run prop styleOf)
        | SlotKind.Item _ -> ())

    assignItems template context.Items
    |> List.iter (fun (run, item) ->
        let art, style = Styles.itemVisual item
        clearRun run
        drawInRun grid run art (fun _ -> Some style))

    grid
