module Cli.Scenes.Studio.Root

open Agents
open Cli.Components.Commands
open Cli.Text
open Entities
open Simulation

let getRoomName room =
    match room with
    | MasteringRoom _ -> I18n.translate (StudioText StudioMasteringRoomName)
    | RecordingRoom _ -> I18n.translate (StudioText StudioRecordingRoomName)

let getRoomDescription studio room =
    match room with
    | MasteringRoom ->
        StudioMasteringRoomDescription studio
        |> StudioText
        |> I18n.translate
    | RecordingRoom ->
        I18n.translate (StudioText StudioRecordingRoomDescription)

let getRoomObjects _ = []

let getRoomCommands studio room =
    let state = State.get ()

    let currentBand =
        Queries.Bands.currentBand state

    let unreleasedAlbums =
        Queries.Albums.unreleasedByBand state currentBand.Id

    let hasUnreleasedAlbums =
        not (Map.isEmpty unreleasedAlbums)

    match room with
    | MasteringRoom ->
        let createRecordOption =
            (StudioText StudioTalkCreateRecord
             |> I18n.translate),
            fun () -> CreateRecord.createRecordSubscene studio

        let continueRecordOption =
            (StudioText StudioTalkContinueRecord
             |> I18n.translate),
            fun () -> ContinueRecord.continueRecordSubscene studio

        let talkOptions =
            [ createRecordOption
              if hasUnreleasedAlbums then
                  continueRecordOption ]

        [ TalkCommand.create [
              { Npc = studio.Producer
                Prompt =
                  StudioTalkIntroduction(studio.Name, studio.PricePerSong)
                  |> StudioText
                  |> I18n.translate
                Options = talkOptions }
          ] ]
    | RecordingRoom -> []
