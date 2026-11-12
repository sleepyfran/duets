/// Parsing and resolution of the room scene templates. Check the README in
/// Resources/Scenes for the format of the templates.
module Duets.Data.Scenes

open System
open System.Globalization
open System.IO
open System.Text
open Duets.Common
open Duets.Entities
open Duets.Entities.World.Place.Type

/// Defines where an exit slot leads to.
[<RequireQualifiedAccess>]
type ExitTarget =
    /// Leads to the room in the given direction.
    | Room of Direction
    /// Leads to the street outside of the place.
    | Street

/// Defines which items can be drawn in an item slot, based on the main
/// property of the item.
[<RequireQualifiedAccess>]
type ItemCategory =
    | Any
    | Cookware
    | Drinkable
    | Edible
    | FitnessEquipment
    | Listenable
    | Playable
    | Readable
    | Rideable
    | Sleepable
    | Storage
    | Watchable
    | Wearable

/// Defines what gets drawn in a slot. Some slots use each cell as a possible
/// position (eg: people) and others treat each group of consecutive cells in a
/// row as a single spot (eg: items).
[<RequireQualifiedAccess>]
type SlotKind =
    /// The player, drawn in the first spot.
    | Player
    /// People in the room, placed randomly in the cells.
    | Npc
    /// An item of the given category.
    | Item of ItemCategory
    /// An arrow pointing to the exit, if the room has it.
    | Exit of ExitTarget
    /// The sky, which changes with the time and weather.
    | Window
    /// The audience during a concert, drawn with the given glyphs or the
    /// default ones if empty.
    | Crowd of glyphs: char list
    /// The rest of the band during a concert.
    | Band
    /// Sign of the restaurant, which depends on its cuisine.
    | Cuisine
    /// Decoration that depends on the quality of the place, from the lowest
    /// tier to the highest. An empty string means nothing gets drawn.
    | Prop of tiers: string list list
    /// Art of the merch stand, only drawn once the band has set it up.
    | Merch of art: string
    /// Fans waiting at the merch stand after a concert, lined up from the
    /// cell closest to the stand.
    | Queue

/// Defines a slot of a template and the glyph to draw when it's empty.
type Slot = { Kind: SlotKind; Fallback: char }

/// Defines a group of consecutive cells in a row.
type Run = { X: int; Y: int; Length: int }

/// Defines a parsed template.
type Template =
    {
        /// Lines of the art, padded to the same width.
        Art: string list
        Width: int
        Height: int
        Legend: Map<char, string>
        DefaultStyle: string option
        Slots: Map<char, Slot>
    }

/// Defines where the art of a landmark is drawn.
[<RequireQualifiedAccess>]
type LandmarkLayer =
    /// Behind the façades, on top of the skyline.
    | Back
    /// On top of everything, standing on the sidewalk.
    | Front

/// Defines where the art of a landmark goes along the street.
[<RequireQualifiedAccess>]
type LandmarkAlign =
    | Center
    /// Next to the first façade.
    | Left
    /// Next to the last façade.
    | Right

/// Defines a parsed landmark.
type Landmark =
    {
        Art: Template option
        Layer: LandmarkLayer
        Align: LandmarkAlign
        /// Rows between the sidewalk and the bottom of the art.
        Elevation: int
        /// Pattern repeated along the sidewalk instead of the city one.
        Sidewalk: string option
        /// Lines that replace the road, from top to bottom.
        Road: string list
        Traffic: bool
        /// Props drawn between the façades, each one from the top down.
        Props: string list
        Captions: string list
        Legend: Map<char, string>
        DefaultStyle: string option
    }

type private ResultBuilder() =
    member _.Bind(result, binder) = Result.bind binder result
    member _.Return value = Ok value

let private result = ResultBuilder()

module private Sections =
    let splitAssignment (line: string) =
        match line.IndexOf(" = ") with
        | -1 -> Error $"Expected 'key = value' but got '{line}'"
        | index ->
            Ok(line.Substring(0, index).Trim(), line.Substring(index + 3).Trim())

    let private isIgnored (line: string) =
        String.IsNullOrWhiteSpace line || line.TrimStart().StartsWith("#")

    let read (headers: string list) (drawings: string list) (text: string) =
        let lines = text.Replace("\r", "").Split('\n') |> List.ofArray

        let rec readLines section lines sections =
            match lines, section with
            | [], _ -> Ok sections
            | (line: string) :: rest, _ when headers |> List.contains (line.Trim()) ->
                readLines (Some(line.Trim())) rest sections
            | line :: rest, Some section when drawings |> List.contains section ->
                readLines (Some section) rest ((section, line.TrimEnd()) :: sections)
            | line :: rest, _ when isIgnored line -> readLines section rest sections
            | line :: rest, Some section ->
                readLines (Some section) rest ((section, line) :: sections)
            | line :: _, None -> Error $"Unexpected line outside of a section: '{line}'"

        readLines None lines []
        |> Result.map (fun sections ->
            sections
            |> List.rev
            |> List.groupBy fst
            |> List.map (fun (section, lines) -> section, List.map snd lines)
            |> Map.ofList)

    let find section (sections: Map<string, string list>) =
        sections |> Map.tryFind section |> Option.defaultValue []

    let trimEmptyLines lines =
        lines
        |> List.skipWhile String.IsNullOrWhiteSpace
        |> List.rev
        |> List.skipWhile String.IsNullOrWhiteSpace
        |> List.rev

    let collect parser lines =
        lines
        |> List.fold
            (fun acc line ->
                match acc, splitAssignment line |> Result.bind parser with
                | Ok parsed, Ok entry -> Ok(entry :: parsed)
                | Error error, _
                | _, Error error -> Error error)
            (Ok [])
        |> Result.map List.rev

    let parseLegend lines =
        collect Ok lines
        |> Result.map (fun legend ->
            let defaultStyle =
                legend |> List.tryFind (fst >> (=) "default") |> Option.map snd

            let styles =
                legend
                |> List.filter (fst >> (<>) "default")
                |> List.collect (fun (glyphs, style) ->
                    glyphs |> Seq.map (fun glyph -> glyph, style) |> List.ofSeq)
                |> Map.ofList

            styles, defaultStyle)

module Template =
    let private sectionHeaders = [ "[art]"; "[legend]"; "[slots]" ]

    let private tokens (text: string) =
        text.Split(
            [| ' '; '\t' |],
            StringSplitOptions.RemoveEmptyEntries
        )
        |> List.ofArray

    let private parseDirection text =
        match text with
        | "north" -> Ok(ExitTarget.Room North)
        | "north-east" -> Ok(ExitTarget.Room NorthEast)
        | "east" -> Ok(ExitTarget.Room East)
        | "south-east" -> Ok(ExitTarget.Room SouthEast)
        | "south" -> Ok(ExitTarget.Room South)
        | "south-west" -> Ok(ExitTarget.Room SouthWest)
        | "west" -> Ok(ExitTarget.Room West)
        | "north-west" -> Ok(ExitTarget.Room NorthWest)
        | "street" -> Ok ExitTarget.Street
        | other -> Error $"Unknown exit '{other}'"

    let private parseItemCategory text =
        match text with
        | "any" -> Ok ItemCategory.Any
        | "cookware" -> Ok ItemCategory.Cookware
        | "drinkable" -> Ok ItemCategory.Drinkable
        | "edible" -> Ok ItemCategory.Edible
        | "fitness" -> Ok ItemCategory.FitnessEquipment
        | "listenable" -> Ok ItemCategory.Listenable
        | "playable" -> Ok ItemCategory.Playable
        | "readable" -> Ok ItemCategory.Readable
        | "rideable" -> Ok ItemCategory.Rideable
        | "sleepable" -> Ok ItemCategory.Sleepable
        | "storage" -> Ok ItemCategory.Storage
        | "watchable" -> Ok ItemCategory.Watchable
        | "wearable" -> Ok ItemCategory.Wearable
        | other -> Error $"Unknown item category '{other}'"

    let private parseTiers (text: string) =
        text.Split('|')
        |> List.ofArray
        |> List.map (fun tier ->
            tokens tier
            |> List.map (fun alternative ->
                if alternative = "_" then "" else alternative))
        |> function
            | tiers when tiers |> List.exists List.isEmpty ->
                Error "Prop tiers can't be empty, use _ to draw nothing"
            | tiers -> Ok tiers

    let private parseSlotKind (text: string) =
        match tokens text with
        | [ "player" ] -> Ok SlotKind.Player
        | [ "npc" ] -> Ok SlotKind.Npc
        | [ "window" ] -> Ok SlotKind.Window
        | "crowd" :: glyphs when glyphs |> List.forall (fun glyph -> glyph.Length = 1) ->
            glyphs |> List.map (fun glyph -> glyph[0]) |> SlotKind.Crowd |> Ok
        | "crowd" :: _ -> Error $"Crowd glyphs must be single characters in '{text}'"
        | [ "band" ] -> Ok SlotKind.Band
        | [ "cuisine" ] -> Ok SlotKind.Cuisine
        | [ "item" ] -> Ok(SlotKind.Item ItemCategory.Any)
        | [ "item"; category ] ->
            parseItemCategory category |> Result.map SlotKind.Item
        | [ "exit"; target ] -> parseDirection target |> Result.map SlotKind.Exit
        | [ "queue" ] -> Ok SlotKind.Queue
        | "merch" :: _ :: _ ->
            text.Trim().Substring("merch".Length).Trim() |> SlotKind.Merch |> Ok
        | "prop" :: _ ->
            text.Trim().Substring("prop".Length)
            |> parseTiers
            |> Result.map SlotKind.Prop
        | _ -> Error $"Unknown slot '{text}'"

    let private parseSlot (key: string) (value: string) =
        if key.Length <> 1 then
            Error $"Slot markers must be a single character, got '{key}'"
        else
            let kind, fallback =
                match value.LastIndexOf(" else ") with
                | -1 -> value, Ok ' '
                | index ->
                    let fallback = value.Substring(index + 6).Trim()

                    value.Substring(0, index),
                    if fallback.Length = 1 then
                        Ok fallback[0]
                    else
                        Error $"Fallback must be a single character in '{value}'"

            match parseSlotKind kind, fallback with
            | Ok kind, Ok fallback -> Ok(key[0], { Kind = kind; Fallback = fallback })
            | Error error, _
            | _, Error error -> Error error

    /// Parses the given template text.
    let parse (text: string) : Result<Template, string> =
        result {
            let! sections = Sections.read sectionHeaders [ "[art]" ] text
            let artLines = Sections.find "[art]" sections
            let legendLines = Sections.find "[legend]" sections
            let slotLines = Sections.find "[slots]" sections

            let art = Sections.trimEmptyLines artLines

            do!
                if art.IsEmpty then
                    Error "The template has no art"
                else
                    Ok()

            let width = art |> List.map _.Length |> List.max
            let art = art |> List.map _.PadRight(width)

            let! legend, defaultStyle = Sections.parseLegend legendLines

            let! slots =
                slotLines
                |> Sections.collect (fun (key, value) -> parseSlot key value)

            let missingMarker =
                slots
                |> List.tryFind (fun (marker, _) ->
                    art |> List.exists (String.contains (string marker)) |> not)

            do!
                match missingMarker with
                | Some(marker, _) -> Error $"Slot '{marker}' is not used in the art"
                | None -> Ok()

            return
                { Art = art
                  Width = width
                  Height = art.Length
                  Legend = legend
                  DefaultStyle = defaultStyle
                  Slots = Map.ofList slots }
        }

    /// Returns all the cells in the art that contain the given marker.
    let cellsOf (template: Template) marker =
        template.Art
        |> List.mapi (fun y line ->
            line
            |> Seq.indexed
            |> Seq.choose (fun (x, glyph) ->
                if glyph = marker then Some(x, y) else None)
            |> List.ofSeq)
        |> List.concat

    /// Returns all the groups of consecutive cells that contain the given
    /// marker.
    let runsOf (template: Template) marker =
        template.Art
        |> List.mapi (fun y line ->
            line
            |> Seq.indexed
            |> Seq.fold
                (fun runs (x, glyph) ->
                    match runs with
                    | run :: rest when glyph = marker && run.X + run.Length = x ->
                        { run with Length = run.Length + 1 } :: rest
                    | runs when glyph = marker ->
                        { X = x; Y = y; Length = 1 } :: runs
                    | runs -> runs)
                []
            |> List.rev)
        |> List.concat

    /// Returns which of the tiers should be used for the given quality.
    let tierFor (quality: Quality) tierCount =
        let quality = Math.Clamp(int quality, 0, 100)
        min (tierCount - 1) (quality * tierCount / 101)

module Landmark =
    let private sectionHeaders =
        [ "[art]"; "[legend]"; "[options]"; "[sidewalk]"; "[road]"; "[props]"; "[captions]" ]

    let private drawings = [ "[art]"; "[sidewalk]"; "[road]"; "[props]" ]

    let private parseLayer text =
        match text with
        | "back" -> Ok LandmarkLayer.Back
        | "front" -> Ok LandmarkLayer.Front
        | other -> Error $"Unknown layer '{other}'"

    let private parseAlign text =
        match text with
        | "center" -> Ok LandmarkAlign.Center
        | "left" -> Ok LandmarkAlign.Left
        | "right" -> Ok LandmarkAlign.Right
        | other -> Error $"Unknown alignment '{other}'"

    let private parseElevation (text: string) =
        match Int32.TryParse text with
        | true, elevation when elevation >= 0 -> Ok elevation
        | _ -> Error $"Expected a positive number but got '{text}'"

    let private parseSwitch text =
        match text with
        | "yes" -> Ok true
        | "no" -> Ok false
        | other -> Error $"Expected 'yes' or 'no' but got '{other}'"

    (* Each column is a prop, so all of them stand on the same ground. *)
    let private parseProps (lines: string list) =
        let width = lines |> List.map _.Length |> List.fold max 0
        let lines = lines |> List.map _.PadRight(width)

        [ for x in 0 .. width - 1 do
              let column =
                  lines |> List.map (fun line -> line[x]) |> Array.ofList |> String

              if not (String.IsNullOrWhiteSpace column) then
                  yield column.TrimStart() ]

    /// Parses the given landmark text.
    let parse (text: string) : Result<Landmark, string> =
        result {
            let! sections = Sections.read sectionHeaders drawings text
            let find section = Sections.find section sections
            let art = find "[art]" |> Sections.trimEmptyLines
            let sidewalk = find "[sidewalk]" |> Sections.trimEmptyLines
            let road = find "[road]" |> Sections.trimEmptyLines
            let props = find "[props]" |> Sections.trimEmptyLines |> parseProps
            let captions = find "[captions]" |> List.map _.Trim()

            let! legend, defaultStyle = Sections.parseLegend (find "[legend]")
            let! options = Sections.collect Ok (find "[options]")

            let option key parser defaultValue =
                match options |> List.tryFind (fst >> (=) key) with
                | Some(_, value) -> parser value
                | None -> Ok defaultValue

            let! layer = option "layer" parseLayer LandmarkLayer.Back
            let! align = option "align" parseAlign LandmarkAlign.Center
            let! elevation = option "elevation" parseElevation 0
            let! traffic = option "traffic" parseSwitch road.IsEmpty

            let knownOptions = [ "layer"; "align"; "elevation"; "traffic" ]

            do!
                match options |> List.tryFind (fun (key, _) -> not (List.contains key knownOptions)) with
                | Some(key, _) -> Error $"Unknown option '{key}'"
                | None -> Ok()

            do!
                if sidewalk.Length > 1 then
                    Error "The sidewalk must be a single line"
                elif
                    art.IsEmpty && sidewalk.IsEmpty && road.IsEmpty && props.IsEmpty && captions.IsEmpty
                then
                    Error "The landmark doesn't change anything"
                else
                    Ok()

            let art =
                match art with
                | [] -> None
                | art ->
                    let width = art |> List.map _.Length |> List.max

                    Some
                        { Art = art |> List.map _.PadRight(width)
                          Width = width
                          Height = art.Length
                          Legend = legend
                          DefaultStyle = defaultStyle
                          Slots = Map.empty }

            return
                { Art = art
                  Layer = layer
                  Align = align
                  Elevation = elevation
                  Sidewalk = List.tryHead sidewalk
                  Road = road
                  Traffic = traffic
                  Props = props
                  Captions = captions
                  Legend = legend
                  DefaultStyle = defaultStyle }
        }

    /// Returns the style of the given glyph of the landmark, if it has one.
    let styleOf (landmark: Landmark) glyph =
        landmark.Legend |> Map.tryFind glyph |> Option.orElse landmark.DefaultStyle

module ItemCategory =
    /// Returns the category of the given item.
    let ofItem (item: Item) =
        match item.Properties |> List.tryHead with
        | Some Cookware -> ItemCategory.Cookware
        | Some(Drinkable _) -> ItemCategory.Drinkable
        | Some(Edible _) -> ItemCategory.Edible
        | Some FitnessEquipment -> ItemCategory.FitnessEquipment
        | Some(Listenable _) -> ItemCategory.Listenable
        | Some(Playable _) -> ItemCategory.Playable
        | Some(Readable _) -> ItemCategory.Readable
        | Some(Rideable _) -> ItemCategory.Rideable
        | Some Sleepable -> ItemCategory.Sleepable
        | Some(Storage _) -> ItemCategory.Storage
        | Some Watchable -> ItemCategory.Watchable
        | Some(Wearable _) -> ItemCategory.Wearable
        | _ -> ItemCategory.Any

/// Turns the given text into a key for the templates, eg: `NewYork` becomes
/// `new-york` and `Café Ñandú` becomes `cafe-nandu`.
let toKey (text: string) =
    let builder = StringBuilder()
    let mutable previous = ' '

    for char in text.Normalize(NormalizationForm.FormD) do
        if
            CharUnicodeInfo.GetUnicodeCategory char
            <> UnicodeCategory.NonSpacingMark
        then
            if Char.IsUpper char && Char.IsLower previous then
                builder.Append('-') |> ignore

            if Char.IsLetterOrDigit char then
                builder.Append(Char.ToLowerInvariant char) |> ignore
            else
                builder.Append('-') |> ignore

            previous <- char

    builder.ToString().Split('-', StringSplitOptions.RemoveEmptyEntries)
    |> String.concat "-"

/// Returns the size tier of the given place, if it's a concert space.
let sizeTier (place: Place) =
    match place.PlaceType with
    | PlaceType.ConcertSpace space when space.Capacity <= 500 -> Some "club"
    | PlaceType.ConcertSpace space when space.Capacity <= 3000 -> Some "theatre"
    | PlaceType.ConcertSpace space when space.Capacity <= 25000 -> Some "arena"
    | PlaceType.ConcertSpace _ -> Some "stadium"
    | _ -> None

/// Returns the keys of the templates that could be used for the given room,
/// from the most specific (the place itself) to the most generic (just the
/// room type).
let candidateKeys (cityId: CityId) (place: Place) (roomType: RoomType) =
    let city = Union.caseName cityId |> toKey
    let placeName = toKey place.Name
    let placeType = place.PlaceType |> toIndex |> Union.caseName |> toKey
    let room = Union.caseName roomType |> toKey

    [ $"places/{city}/{placeName}/{room}"
      yield! place.SceneTags |> List.map (fun tag -> $"rooms/{placeType}.{room}.{tag}")
      $"cities/{city}/{placeType}.{room}"
      $"cities/{city}/{room}"
      match sizeTier place with
      | Some tier -> $"rooms/{placeType}.{room}.{tier}"
      | None -> ()
      $"rooms/{placeType}.{room}"
      $"rooms/{room}" ]

/// Returns the template for the given room from the given templates that fits
/// in the given width, if any.
let tryResolve (templates: Map<string, Template>) maxWidth cityId place roomType =
    candidateKeys cityId place roomType
    |> List.tryPick (fun key ->
        templates.TryFind key
        |> Option.filter (fun template -> template.Width <= maxWidth))

let private scenesFolder = Files.dataFolder "Scenes"

let private landmarksFolder = "landmarks"

let private loadFiles parse kind inLandmarks folder =
    if Directory.Exists folder then
        Directory.EnumerateFiles(folder, "*.scene", SearchOption.AllDirectories)
        |> Seq.map (fun path ->
            let key =
                Path.ChangeExtension(Path.GetRelativePath(folder, path), null)
                    .Replace('\\', '/')

            key, path)
        |> Seq.filter (fun (key, _) ->
            key.StartsWith $"{landmarksFolder}/" = inLandmarks)
        |> Seq.map (fun (key, path) ->
            match File.ReadAllText path |> parse with
            | Ok parsed -> key, parsed
            | Error error -> failwith $"Invalid {kind} {key}: {error}")
        |> Map.ofSeq
    else
        Map.empty

/// Loads all the templates in the given folder, keyed by their path. Throws
/// if any of them is invalid.
let loadFrom folder = loadFiles Template.parse "scene template" false folder

/// Loads all the landmarks in the given folder, keyed by their path. Throws
/// if any of them is invalid.
let loadLandmarksFrom folder = loadFiles Landmark.parse "landmark" true folder

/// All the templates of the game.
let all = lazy (loadFrom scenesFolder)

/// All the landmarks of the game.
let landmarks = lazy (loadLandmarksFrom scenesFolder)

/// Returns the template for the given room that fits in the given width, if any.
let resolve maxWidth cityId place roomType =
    tryResolve all.Value maxWidth cityId place roomType

let private hasAirport (zone: Zone) =
    zone.Streets.Nodes
    |> Map.exists (fun _ street ->
        street.Places
        |> List.exists (fun place -> place.PlaceType = PlaceType.Airport))

/// Returns the keys of the possible skylines of the given zone. Zones with an
/// airport use the airport skyline unless they have their own.
let skylineKeys (cityId: CityId) (zone: Zone) =
    let city = Union.caseName cityId |> toKey

    [ $"skylines/{city}.{toKey zone.Name}"
      if hasAirport zone then
          "skylines/airport"
      $"skylines/{city}" ]

/// Returns the skyline of the given zone from the given templates.
let tryResolveSkyline (templates: Map<string, Template>) cityId zone =
    skylineKeys cityId zone |> List.tryPick templates.TryFind

/// Returns the skyline of the given zone.
let resolveSkyline cityId zone = tryResolveSkyline all.Value cityId zone

/// Returns the key of the landmark with the given tag.
let landmarkKey (tag: string) = $"{landmarksFolder}/{tag}"

/// Returns the landmarks of the given street from the given landmarks.
let tryResolveLandmarks (landmarks: Map<string, Landmark>) (street: Street) =
    street.SceneTags |> List.choose (landmarkKey >> landmarks.TryFind)

/// Returns the landmarks of the given street.
let resolveLandmarks street = tryResolveLandmarks landmarks.Value street
