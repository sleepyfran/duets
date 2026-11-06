[<RequireQualifiedAccess>]
module Duets.Cli.Components.Scene.Room.Caption

open Duets.Cli.Components.Scene.Common
open Duets.Cli.Text
open Duets.Data.Scenes
open Duets.Entities

let private pick (random: System.Random) (options: string list) =
    options |> List.item (random.Next(options.Length))

let private hasWindows (template: Template) =
    template.Slots |> Map.exists (fun _ slot -> slot.Kind = SlotKind.Window)

/// Creates one line of flavor text for the room. Like the street caption, it
/// only depends on the room, the quality of the place, the time of the day,
/// the weather and how many people are around, so it stays the same on every
/// visit with the same conditions.
let create
    (context: RoomRenderer.RoomContext)
    (template: Template)
    roomType
    =
    let lighting = context.Lighting
    let peopleCount = context.KnownPeople + context.UnknownPeople

    let random =
        seedOf
            $"{context.Seed}{lighting.DayMoment}{lighting.Weather}{peopleCount}{context.ConcertOngoing}"
        |> System.Random

    let phase = SceneCaptions.phaseOf lighting.DayMoment

    let firstSentence =
        match roomType with
        | RoomType.Stage when context.ConcertOngoing ->
            SceneCaptions.crowd context.Crowd |> pick random
        | _ ->
            let opening = SceneCaptions.roomCondition context.Quality |> pick random
            let closing = SceneCaptions.roomActivity peopleCount |> pick random
            $"{opening}{closing}"

    let windowSentence =
        if hasWindows template then
            match SceneCaptions.throughWindow lighting.Weather phase with
            | [] -> None
            | sentences -> pick random sentences |> Some
        else
            None

    match windowSentence with
    | Some sentence -> $"{firstSentence} {sentence}"
    | None -> firstSentence
