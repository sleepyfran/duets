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
            (fun _ -> ChooseAction.createMenu () |> Some)

    match instrumentType with
    | InstrumentType.Bass -> create Objects.bass
    | InstrumentType.Drums -> create Objects.drums
    | InstrumentType.Guitar -> create Objects.guitar
    | InstrumentType.Vocals -> create Objects.microphone

let private getPlaceName room =
    match room with
    | RehearsalSpaceRoom.Lobby space -> Literal space.Name
    | RehearsalSpaceRoom.Bar space -> Literal space.Name
    | RehearsalRoom space -> Literal space.Name

let private getRoomName room =
    match room with
    | RehearsalSpaceRoom.Lobby _ -> I18n.translate (CommonText CommonLobbyName)
    | RehearsalSpaceRoom.Bar _ -> I18n.translate (CommonText CommonBarName)
    | RehearsalRoom _ ->
        I18n.translate (RehearsalSpaceText RehearsalSpaceRehearsalRoomName)

let private getRoomDescription room =
    match room with
    | RehearsalSpaceRoom.Lobby _ ->
        I18n.translate (RehearsalSpaceText RehearsalSpaceLobbyDescription)
    | RehearsalSpaceRoom.Bar _ ->
        I18n.translate (RehearsalSpaceText RehearsalSpaceBarDescription)
    | RehearsalRoom _ ->
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
    | RehearsalSpaceRoom.Lobby _ -> []
    | RehearsalSpaceRoom.Bar _ -> []
    | RehearsalRoom _ -> [ characterInstrument ]

let private getRoomCommands room =
    match room with
    | RehearsalSpaceRoom.Lobby _ -> []
    | RehearsalSpaceRoom.Bar _ -> []
    | RehearsalRoom _ ->
        [ { Name = "manage"
            Description =
                I18n.translate (
                    RehearsalSpaceText RehearsalRoomManageDescription
                )
            Handler = fun _ -> Some Scene.Management } ]

/// Creates an interactive scene inside of a rehearsal space in the given city,
/// place and room.
let rehearsalSpace city place placeId roomId =
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
