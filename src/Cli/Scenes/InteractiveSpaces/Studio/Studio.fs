module Cli.Scenes.Studio.Root

open Agents
open Cli.Components.Commands
open Cli.Scenes.InteractiveSpaces.Components
open Cli.Text
open Entities
open Simulation

let private getPlaceName room =
    match room with
    | MasteringRoom studio -> I18n.constant studio.Name
    | RecordingRoom studio -> I18n.constant studio.Name

let private getRoomName room =
    match room with
    | MasteringRoom _ -> I18n.translate (StudioText StudioMasteringRoomName)
    | RecordingRoom _ -> I18n.translate (StudioText StudioRecordingRoomName)

let private getRoomDescription room =
    match room with
    | MasteringRoom studio ->
        StudioMasteringRoomDescription studio
        |> StudioText
        |> I18n.translate
    | RecordingRoom _ ->
        I18n.translate (StudioText StudioRecordingRoomDescription)

let private getRoomObjects _ = []

let private getRoomCommands room =
    let state = State.get ()
    let currentBand = Queries.Bands.currentBand state

    let unreleasedAlbums =
        Queries.Albums.unreleasedByBand state currentBand.Id

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

        [ TalkCommand.create [
              { Npc = studio.Producer
                Prompt =
                    StudioTalkIntroduction(studio.Name, studio.PricePerSong)
                    |> StudioText
                    |> I18n.translate
                Options = talkOptions }
          ] ]
    | RecordingRoom _ -> []

/// Creates an interactive scene inside of a studio in the given city, place
/// and room.
let studioSpace city place placeId roomId =
    let roomId =
        roomId
        |> Option.defaultValue place.Rooms.StartingNode

    let room =
        Queries.World.contentOf place.Rooms roomId

    let entrances =
        Queries.World.availableDirections roomId place.Rooms
        |> List.map
            (fun (direction, connectedRoomId) ->
                Queries.World.contentOf place.Rooms connectedRoomId
                |> getRoomName
                |> fun name -> (direction, name, Room(placeId, connectedRoomId)))

    let exit = exitOfNode city roomId place.Exits
    let description = getRoomDescription room
    let objects = getRoomObjects room
    let commands = getRoomCommands room

    showWorldCommandPrompt entrances exit description objects commands
