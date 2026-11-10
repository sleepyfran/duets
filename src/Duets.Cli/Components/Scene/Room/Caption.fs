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

let private hasMerchStand (template: Template) =
    template.Slots
    |> Map.exists (fun _ slot ->
        match slot.Kind with
        | SlotKind.Merch _ -> true
        | _ -> false)

let private moodSentences (show: Show) =
    match show.Mood with
    | CrowdMood.Bored -> SceneCaptions.boredCrowd
    | CrowdMood.Hyped -> SceneCaptions.hypedCrowd
    | CrowdMood.Ecstatic -> SceneCaptions.ecstaticCrowd
    | CrowdMood.Warm -> []

/// Returns the reactions of the crowd to a song.
let songReactions (show: Show) =
    match show.Mood with
    | CrowdMood.Bored -> SceneCaptions.boredSongReactions
    | CrowdMood.Warm -> SceneCaptions.warmSongReactions
    | CrowdMood.Hyped -> SceneCaptions.hypedSongReactions
    | CrowdMood.Ecstatic -> SceneCaptions.ecstaticSongReactions

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
            $"{context.Seed}{lighting.DayMoment}{lighting.Weather}{peopleCount}{context.Show}{context.MerchStand}"
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
            let caption = $"{opening}{closing}"

            match context.MerchStand with
            | Some people when hasMerchStand template ->
                $"{caption} {SceneCaptions.merchStand people |> pick random}"
            | _ -> caption

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
