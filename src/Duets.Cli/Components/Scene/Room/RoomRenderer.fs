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
      /// Only set during a concert.
      Show: Show option
      Cuisine: RestaurantCuisine option }

/// Returns the color of the stage light at the given column.
let private stageColorAt (context: RoomContext) frame x =
    match context.Show with
    | None -> Styles.stageOffColor
    | Some { Energy = None } -> Styles.stageOnColor
    | Some { Energy = Some energy } ->
        let lights, framesPerStep =
            match energy with
            | Energetic -> Styles.energeticLights, 1
            | PerformEnergy.Normal -> Styles.normalLights, 2
            | Limited -> Styles.limitedLights, 4

        lights[(x / 2 + frame / framesPerStep) % lights.Length]

let private resolveStyle (context: RoomContext) accent stage (style: string) =
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
        .Replace("$stage", stage)

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
            SceneGrid.set grid (x + index) run.Y glyph (styleOf (x + index) glyph))

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

/// Returns how many people stay in front of the stage and how many drift away.
let private crowdCount (show: Show option) cellCount =
    match show with
    | None -> 0, 0
    | Some show ->
        let present = int (System.Math.Ceiling(float cellCount * show.Attendance))

        match show.Mood with
        | CrowdMood.Bored ->
            let staying =
                int (System.Math.Ceiling(float present * Styles.boredCrowdRatio))

            staying, present - staying
        | _ -> present, 0

let private crowdGlyphs (show: Show) (templateGlyphs: char list) =
    let people =
        if List.isEmpty templateGlyphs then
            Styles.crowd
        else
            Array.ofList templateGlyphs

    let armsUp count = Array.replicate count Styles.armsUp

    match show.Mood with
    | CrowdMood.Bored -> Styles.boredCrowd
    | CrowdMood.Warm -> people
    | CrowdMood.Hyped -> Array.append people (armsUp people.Length)
    | CrowdMood.Ecstatic -> Array.append (armsUp 4) Styles.crowdLights

let private crowdStyle (show: Show) accent glyph =
    if Array.contains glyph Styles.crowdLights then
        SceneStyles.bold accent
    else
        match show.Mood with
        | CrowdMood.Bored -> Styles.boredCrowdColor
        | CrowdMood.Warm -> Styles.crowdColor
        | CrowdMood.Hyped -> Styles.hypedCrowdColor
        | CrowdMood.Ecstatic -> Styles.ecstaticCrowdColor

/// Returns how likely each person is to move between frames.
let private crowdMotion mood =
    match mood with
    | CrowdMood.Bored -> 0.05
    | CrowdMood.Warm -> 0.2
    | CrowdMood.Hyped -> 0.5
    | CrowdMood.Ecstatic -> 0.7

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

/// Builds the given frame of the scene, where only the crowd moves.
let buildFrame frame (context: RoomContext) (template: Template) =
    let grid = SceneGrid.create template.Width template.Height
    let accentRandom = System.Random(context.Seed)

    let accent =
        context.Skin.Signs
        |> List.item (accentRandom.Next(context.Skin.Signs.Length))

    let styleOf x glyph =
        template.Legend
        |> Map.tryFind glyph
        |> Option.orElse template.DefaultStyle
        |> Option.map (resolveStyle context accent (stageColorAt context frame x))

    template.Art
    |> List.iteri (fun y line ->
        line
        |> Seq.iteri (fun x glyph ->
            match template.Slots |> Map.tryFind glyph with
            | Some slot when slot.Fallback <> ' ' ->
                SceneGrid.set grid x y slot.Fallback (styleOf x slot.Fallback)
            | Some _ -> ()
            | None when glyph <> ' ' -> SceneGrid.set grid x y glyph (styleOf x glyph)
            | None -> ()))

    (* Bored people leave the stage and go to the npc slots. *)
    let driftedCrowd =
        template.Slots
        |> Map.toList
        |> List.sumBy (fun (marker, slot) ->
            match slot.Kind with
            | SlotKind.Crowd _ ->
                Template.cellsOf template marker
                |> List.length
                |> crowdCount context.Show
                |> snd
            | _ -> 0)

    let clearRun (run: Run) =
        for x in run.X .. run.X + run.Length - 1 do
            SceneGrid.set grid x run.Y ' ' None

    template.Slots
    |> Map.iter (fun marker slot ->
        (* Separate random per slot, otherwise people coming in would move the props around. *)
        let random = System.Random(context.Seed + 7919 * int marker)
        let cells () = Template.cellsOf template marker
        let runs () = Template.runsOf template marker

        let fillCells count glyphs styleOf =
            cells ()
            |> shuffle random
            |> List.truncate count
            |> List.iter (fun (x, y) ->
                let glyph = glyphs |> Array.item (random.Next(Array.length glyphs))
                SceneGrid.set grid x y glyph (Some(styleOf glyph)))

        match slot.Kind with
        | SlotKind.Player ->
            runs ()
            |> List.tryHead
            |> Option.iter (fun run ->
                drawInRun grid run (string People.player) (fun _ _ ->
                    Some People.playerStyle))
        | SlotKind.Npc ->
            let positions = cells () |> shuffle random

            positions
            |> List.truncate (
                context.KnownPeople + context.UnknownPeople + driftedCrowd
            )
            |> List.iteri (fun index (x, y) ->
                if index < context.KnownPeople then
                    SceneGrid.set grid x y People.known (Some People.knownStyle)
                else
                    SceneGrid.set grid x y People.unknown (Some People.unknownStyle))
        | SlotKind.Band ->
            fillCells context.BandMembers [| People.known |] (fun _ ->
                People.knownStyle)
        | SlotKind.Crowd glyphs ->
            context.Show
            |> Option.iter (fun show ->
                let count, _ = crowdCount context.Show (cells ()).Length
                let glyphs = crowdGlyphs show glyphs
                let pick (random: System.Random) = glyphs[random.Next(glyphs.Length)]

                let motion =
                    System.Random(context.Seed + 7919 * int marker + 104729 * frame)

                cells ()
                |> shuffle random
                |> List.truncate count
                |> List.iter (fun (x, y) ->
                    let glyph = pick random

                    let glyph =
                        if frame > 0 && motion.NextDouble() < crowdMotion show.Mood then
                            pick motion
                        else
                            glyph

                    SceneGrid.set grid x y glyph (Some(crowdStyle show accent glyph))))
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
                drawInRun grid run arrow (fun _ _ -> Some Styles.exitColor))
        | SlotKind.Exit _ -> ()
        | SlotKind.Cuisine ->
            context.Cuisine
            |> Option.iter (fun cuisine ->
                runs ()
                |> List.iter (fun run ->
                    clearRun run

                    drawInRun grid run (Styles.cuisineSign cuisine) (fun _ _ ->
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
        drawInRun grid run art (fun _ _ -> Some style))

    grid

/// Builds the scene of the given template.
let build context template = buildFrame 0 context template
