module Cli.Scenes.InteractiveSpaces.RehearsalRoom.Root

open Agents
open Cli.Components.Commands
open Cli.SceneIndex
open Cli.Scenes.InteractiveSpaces
open Cli.Scenes.InteractiveSpaces.Components
open Cli.Text
open Entities
open Simulation

let private instrumentFromType instrumentType =
    let create fn =
        fn
            (I18n.translate (
                RehearsalSpaceText RehearsalRoomInstrumentPlayDescription
            ))
            (fun _ -> ChooseAction.createMenu ())

    match instrumentType with
    | InstrumentType.Bass -> create Objects.bass
    | InstrumentType.Drums -> create Objects.drums
    | InstrumentType.Guitar -> create Objects.guitar
    | InstrumentType.Vocals -> create Objects.microphone

let private getRoomName room =
    match room with
    | RehearsalSpaceRoom.Lobby -> I18n.translate (CommonText CommonLobbyName)
    | RehearsalSpaceRoom.Bar -> I18n.translate (CommonText CommonBarName)
    | RehearsalRoom ->
        I18n.translate (RehearsalSpaceText RehearsalSpaceRehearsalRoomName)

let private getRoomDescription room =
    match room with
    | RehearsalSpaceRoom.Lobby ->
        I18n.translate (RehearsalSpaceText RehearsalSpaceLobbyDescription)
    | RehearsalSpaceRoom.Bar ->
        I18n.translate (RehearsalSpaceText RehearsalSpaceBarDescription)
    | RehearsalSpaceRoom.RehearsalRoom ->
        I18n.translate (
            RehearsalSpaceText RehearsalSpaceRehearsalRoomDescription
        )

let private getRoomObjects room =
    let state = State.get ()

    let characterInstrument =
        Queries.Bands.currentPlayableMember state
        |> fun bandMember -> bandMember.Role
        |> instrumentFromType

    match room with
    | RehearsalSpaceRoom.Lobby -> []
    | RehearsalSpaceRoom.Bar -> []
    | RehearsalSpaceRoom.RehearsalRoom -> [ characterInstrument ]

let private getRoomCommands room =
    match room with
    | RehearsalSpaceRoom.Lobby -> []
    | RehearsalSpaceRoom.Bar -> []
    | RehearsalSpaceRoom.RehearsalRoom ->
        [ { Name = "manage"
            Description =
                I18n.translate (
                    RehearsalSpaceText RehearsalRoomManageDescription
                )
            Handler = fun _ -> Scene.Management } ]

/// Creates an interactive scene inside of a rehearsal space in the given city,
/// place and room.
let rehearsalSpace city place placeId roomId =
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

    let description = getRoomDescription room

    let objects = getRoomObjects room
    let commands = getRoomCommands room

    showWorldCommandPrompt entrances exit description objects commands
