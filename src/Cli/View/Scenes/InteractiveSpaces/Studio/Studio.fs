module Cli.View.Scenes.Studio.Root

open Cli.View.Commands
open Cli.View.TextConstants
open Entities
open Simulation.Queries

let getPlaceName room =
    match room with
    | MasteringRoom studio -> Literal studio.Name
    | RecordingRoom studio -> Literal studio.Name

let getRoomName room =
    match room with
    | MasteringRoom _ -> TextConstant StudioMasteringRoomName
    | RecordingRoom _ -> TextConstant StudioRecordingRoomName

let getRoomDescription room =
    match room with
    | MasteringRoom studio ->
        TextConstant
        <| StudioMasteringRoomDescription studio
    | RecordingRoom _ -> TextConstant StudioRecordingRoomDescription

let getRoomObjects _ = []

let getRoomCommands room =
    let state = State.Root.get ()
    let currentBand = Bands.currentBand state

    let unreleasedAlbums =
        Albums.unreleasedByBand state currentBand.Id

    let hasUnreleasedAlbums = not (Map.isEmpty unreleasedAlbums)

    match room with
    | MasteringRoom studio ->
        let createRecordOption =
            (TextConstant StudioTalkCreateRecord,
             CreateRecord.createRecordSubscene studio)

        let continueRecordOption =
            (TextConstant StudioTalkContinueRecord,
             ContinueRecord.continueRecordSubscene studio)

        [ TalkCommand.create [ { Npc = studio.Producer
                                 Prompt =
                                     StudioTalkIntroduction(
                                         studio.Name,
                                         studio.PricePerSong
                                     )
                                     |> TextConstant
                                 Options =
                                     [ yield createRecordOption
                                       if hasUnreleasedAlbums then
                                           yield continueRecordOption ] } ] ]
    | RecordingRoom _ -> []
