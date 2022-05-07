module Cli.Scenes.InteractiveSpaces.RehearsalRoom.Root

open Agents
open Cli.Components.Commands
open Cli.SceneIndex
open Cli.Scenes.InteractiveSpaces
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

let getRoomName room =
    match room with
    | RehearsalSpaceRoom.Lobby -> I18n.translate (CommonText CommonLobbyName)
    | RehearsalSpaceRoom.Bar -> I18n.translate (CommonText CommonBarName)
    | RehearsalRoom ->
        I18n.translate (RehearsalSpaceText RehearsalSpaceRehearsalRoomName)

let getRoomDescription room =
    match room with
    | RehearsalSpaceRoom.Lobby ->
        I18n.translate (RehearsalSpaceText RehearsalSpaceLobbyDescription)
    | RehearsalSpaceRoom.Bar ->
        I18n.translate (RehearsalSpaceText RehearsalSpaceBarDescription)
    | RehearsalSpaceRoom.RehearsalRoom ->
        I18n.translate (
            RehearsalSpaceText RehearsalSpaceRehearsalRoomDescription
        )

let getRoomObjects room =
    let state = State.get ()

    let characterInstrument =
        Queries.Bands.currentPlayableMember state
        |> fun bandMember -> bandMember.Role
        |> instrumentFromType

    match room with
    | RehearsalSpaceRoom.Lobby -> []
    | RehearsalSpaceRoom.Bar -> []
    | RehearsalSpaceRoom.RehearsalRoom -> [ characterInstrument ]

let getRoomCommands room =
    match room with
    | RehearsalSpaceRoom.Lobby -> []
    | RehearsalSpaceRoom.Bar -> []
    | RehearsalSpaceRoom.RehearsalRoom ->
        [ { Name = "manage"
            Description =
              I18n.translate (RehearsalSpaceText RehearsalRoomManageDescription)
            Handler = fun _ -> Scene.Management } ]
