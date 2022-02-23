module Cli.Scenes.Studio.Root

open Agents
open Cli.Components.Commands
open Cli.Text
open Entities
open Simulation.Queries

let getPlaceName room =
    match room with
    | MasteringRoom studio -> I18n.constant studio.Name
    | RecordingRoom studio -> I18n.constant studio.Name

let getRoomName room =
    match room with
    | MasteringRoom _ -> I18n.translate (StudioText StudioMasteringRoomName)
    | RecordingRoom _ -> I18n.translate (StudioText StudioRecordingRoomName)

let getRoomDescription room =
    match room with
    | MasteringRoom studio ->
        StudioMasteringRoomDescription studio
        |> StudioText
        |> I18n.translate
    | RecordingRoom _ ->
        I18n.translate (StudioText StudioRecordingRoomDescription)

let getRoomObjects _ = []

let getRoomCommands room =
    let state = State.get ()
    let currentBand = Bands.currentBand state

    let unreleasedAlbums = Albums.unreleasedByBand state currentBand.Id

    let hasUnreleasedAlbums = not (Map.isEmpty unreleasedAlbums)

    match room with
    | MasteringRoom studio ->
        let createRecordOption =
            (StudioText StudioTalkCreateRecord
             |> I18n.translate),
            fun () -> CreateRecord.createRecordSubscene studio |> Some

        let continueRecordOption =
            (StudioText StudioTalkContinueRecord
             |> I18n.translate),
            fun () ->
                ContinueRecord.continueRecordSubscene studio
                |> Some

        let talkOptions =
            [ createRecordOption
              if hasUnreleasedAlbums then
                  continueRecordOption ]

        [ TalkCommand.create [ { Npc = studio.Producer
                                 Prompt =
                                   StudioTalkIntroduction(
                                       studio.Name,
                                       studio.PricePerSong
                                   )
                                   |> StudioText
                                   |> I18n.translate
                                 Options = talkOptions } ] ]
    | RecordingRoom _ -> []
