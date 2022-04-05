module Cli.Scenes.Studio.Root

open Agents
open Cli.Components.Commands
open Cli.Scenes.InteractiveSpaces.Components
open Cli.Text
open Entities
open Simulation

let private getRoomName room =
    match room with
    | MasteringRoom _ -> I18n.translate (StudioText StudioMasteringRoomName)
    | RecordingRoom _ -> I18n.translate (StudioText StudioRecordingRoomName)

let private getRoomDescription studio room =
    match room with
    | MasteringRoom ->
        StudioMasteringRoomDescription studio
        |> StudioText
        |> I18n.translate
    | RecordingRoom ->
        I18n.translate (StudioText StudioRecordingRoomDescription)

let private getRoomObjects _ = []

let private getRoomCommands studio room =
    let state = State.get ()
    let currentBand = Queries.Bands.currentBand state

    let unreleasedAlbums =
        Queries.Albums.unreleasedByBand state currentBand.Id

    let hasUnreleasedAlbums = not (Map.isEmpty unreleasedAlbums)

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

/// Creates an interactive scene inside of a studio in the given city, place
/// and room.
let studioSpace city place placeId roomId =
    let roomId =
        roomId
        |> Option.defaultValue place.Rooms.StartingNode

    let room =
        Queries.World.Common.contentOf place.Rooms roomId

    let entrances =
        Queries.World.Common.availableDirections roomId place.Rooms
        |> List.map
            (fun (direction, connectedRoomId) ->
                Queries.World.Common.contentOf place.Rooms connectedRoomId
                |> getRoomName
                |> fun name -> (direction, name, Room(placeId, connectedRoomId)))

    let exit = exitOfNode city roomId place.Exits
    let description = getRoomDescription place.Space room
    let objects = getRoomObjects room
    let commands = getRoomCommands place.Space room

    showWorldCommandPrompt entrances exit description objects commands
