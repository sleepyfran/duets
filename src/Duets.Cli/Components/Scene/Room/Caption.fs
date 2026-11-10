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

let private moodSentences (show: Show) =
    match show.Mood with
    | CrowdMood.Bored -> SceneCaptions.boredCrowd
    | CrowdMood.Hyped -> SceneCaptions.hypedCrowd
    | CrowdMood.Ecstatic -> SceneCaptions.ecstaticCrowd
    | CrowdMood.Warm -> []

/// Creates a caption for the current room.
let create
    (context: RoomRenderer.RoomContext)
    (template: Template)
    roomType
    =
    let lighting = context.Lighting
    let peopleCount = context.KnownPeople + context.UnknownPeople

    let random =
        seedOf
            $"{context.Seed}{lighting.DayMoment}{lighting.Weather}{peopleCount}{context.Show}"
        |> System.Random

    let phase = SceneCaptions.phaseOf lighting.DayMoment

    let firstSentence =
        match roomType, context.Show with
        | RoomType.Stage, Some show ->
            let attendance = SceneCaptions.crowd show.Attendance |> pick random

            match moodSentences show with
            | [] -> attendance
            | sentences -> $"{attendance} {pick random sentences}"
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
