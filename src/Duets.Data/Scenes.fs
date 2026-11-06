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
    /// The audience during a concert.
    | Crowd
    /// The rest of the band during a concert.
    | Band
    /// Sign of the restaurant, which depends on its cuisine.
    | Cuisine
    /// Decoration that depends on the quality of the place, from the lowest
    /// tier to the highest. An empty string means nothing gets drawn.
    | Prop of tiers: string list list

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

type private ResultBuilder() =
    member _.Bind(result, binder) = Result.bind binder result
    member _.Return value = Ok value

let private result = ResultBuilder()

module Template =
    let private sectionHeaders = [ "[art]"; "[legend]"; "[slots]" ]

    let private splitAssignment (line: string) =
        match line.IndexOf(" = ") with
        | -1 -> Error $"Expected 'key = value' but got '{line}'"
        | index ->
            Ok(line.Substring(0, index).Trim(), line.Substring(index + 3).Trim())

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
        | [ "crowd" ] -> Ok SlotKind.Crowd
        | [ "band" ] -> Ok SlotKind.Band
        | [ "cuisine" ] -> Ok SlotKind.Cuisine
        | [ "item" ] -> Ok(SlotKind.Item ItemCategory.Any)
        | [ "item"; category ] ->
            parseItemCategory category |> Result.map SlotKind.Item
        | [ "exit"; target ] -> parseDirection target |> Result.map SlotKind.Exit
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
        let lines =
            text.Replace("\r", "").Split('\n') |> List.ofArray

        let isIgnored (line: string) =
            String.IsNullOrWhiteSpace line || line.TrimStart().StartsWith("#")

        let rec readSections section lines (art, legend, slots) =
            match lines with
            | [] -> Ok(List.rev art, List.rev legend, List.rev slots)
            | (line: string) :: rest when
                sectionHeaders |> List.contains (line.Trim())
                ->
                readSections (Some(line.Trim())) rest (art, legend, slots)
            | line :: rest ->
                match section with
                | Some "[art]" ->
                    readSections
                        section
                        rest
                        (line.TrimEnd() :: art, legend, slots)
                | _ when isIgnored line -> readSections section rest (art, legend, slots)
                | Some "[legend]" ->
                    readSections section rest (art, line :: legend, slots)
                | Some "[slots]" ->
                    readSections section rest (art, legend, line :: slots)
                | _ -> Error $"Unexpected line outside of a section: '{line}'"

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

        let trimEmptyLines lines =
            lines
            |> List.skipWhile String.IsNullOrWhiteSpace
            |> List.rev
            |> List.skipWhile String.IsNullOrWhiteSpace
            |> List.rev

        result {
            let! artLines, legendLines, slotLines =
                readSections None lines ([], [], [])

            let art = trimEmptyLines artLines

            do!
                if art.IsEmpty then
                    Error "The template has no art"
                else
                    Ok()

            let width = art |> List.map _.Length |> List.max
            let art = art |> List.map _.PadRight(width)

            let! legend = legendLines |> collect (fun entry -> Ok entry)

            let defaultStyle =
                legend
                |> List.tryFind (fst >> (=) "default")
                |> Option.map snd

            let legend =
                legend
                |> List.filter (fst >> (<>) "default")
                |> List.collect (fun (glyphs, style) ->
                    glyphs |> Seq.map (fun glyph -> glyph, style) |> List.ofSeq)
                |> Map.ofList

            let! slots = slotLines |> collect (fun (key, value) -> parseSlot key value)

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

/// Returns the keys of the templates that could be used for the given room,
/// from the most specific (the place itself) to the most generic (just the
/// room type).
let candidateKeys (cityId: CityId) (place: Place) (roomType: RoomType) =
    let city = Union.caseName cityId |> toKey
    let placeName = toKey place.Name
    let placeType = place.PlaceType |> toIndex |> Union.caseName |> toKey
    let room = Union.caseName roomType |> toKey

    [ $"places/{city}/{placeName}/{room}"
      $"cities/{city}/{placeType}.{room}"
      $"cities/{city}/{room}"
      $"rooms/{placeType}.{room}"
      $"rooms/{room}" ]

/// Returns the template for the given room from the given templates, if any.
let tryResolve (templates: Map<string, Template>) cityId place roomType =
    candidateKeys cityId place roomType |> List.tryPick templates.TryFind

let private scenesFolder = Files.dataFolder "Scenes"

/// Loads all the templates in the given folder, keyed by their path. Throws
/// if any of them is invalid.
let loadFrom folder =
    if Directory.Exists folder then
        Directory.EnumerateFiles(folder, "*.scene", SearchOption.AllDirectories)
        |> Seq.map (fun path ->
            let key =
                Path.ChangeExtension(Path.GetRelativePath(folder, path), null)
                    .Replace('\\', '/')

            match File.ReadAllText path |> Template.parse with
            | Ok template -> key, template
            | Error error -> failwith $"Invalid scene template {key}: {error}")
        |> Map.ofSeq
    else
        Map.empty

/// All the templates of the game.
let all = lazy (loadFrom scenesFolder)

/// Returns the template for the given room, if any.
let resolve cityId place roomType =
    tryResolve all.Value cityId place roomType
