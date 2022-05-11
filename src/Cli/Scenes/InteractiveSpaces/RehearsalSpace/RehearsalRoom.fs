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
    | Room.Lobby -> I18n.translate (CommonText CommonLobbyName)
    | Room.Bar -> I18n.translate (CommonText CommonBarName)
    | Room.RehearsalRoom ->
        I18n.translate (RehearsalSpaceText RehearsalSpaceRehearsalRoomName)
    | _ -> Literal ""

let getRoomDescription room =
    match room with
    | Room.Lobby ->
        I18n.translate (RehearsalSpaceText RehearsalSpaceLobbyDescription)
    | Room.Bar ->
        I18n.translate (RehearsalSpaceText RehearsalSpaceBarDescription)
    | Room.RehearsalRoom ->
        I18n.translate (
            RehearsalSpaceText RehearsalSpaceRehearsalRoomDescription
        )
    | _ -> Literal ""

let getRoomObjects room =
    let state = State.get ()

    let characterInstrument =
        Queries.Bands.currentPlayableMember state
        |> fun bandMember -> bandMember.Role
        |> instrumentFromType

    match room with
    | Room.RehearsalRoom -> [ characterInstrument ]
    | _ -> []

let getRoomCommands room =
    match room with
    | Room.RehearsalRoom ->
        [ { Name = "manage"
            Description =
              I18n.translate (RehearsalSpaceText RehearsalRoomManageDescription)
            Handler = fun _ -> Scene.Management } ]
    | _ -> []
