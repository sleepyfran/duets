module Cli.View.Scenes.InteractiveSpaces.RehearsalRoom.Root

open Agents
open Cli.View.Actions
open Cli.View.Scenes.InteractiveSpaces
open Cli.View.Text
open Entities
open Simulation.Queries.Bands

let private instrumentFromType instrumentType =
    let create fn =
        fn
            (I18n.translate (
                RehearsalSpaceText RehearsalRoomInstrumentPlayDescription
            ))
            (seq { yield! ChooseAction.createMenu () })

    match instrumentType with
    | InstrumentType.Bass -> create Objects.bass
    | InstrumentType.Drums -> create Objects.drums
    | InstrumentType.Guitar -> create Objects.guitar
    | InstrumentType.Vocals -> create Objects.microphone

let getPlaceName room =
    match room with
    | RehearsalSpaceRoom.Lobby space -> Literal space.Name
    | RehearsalSpaceRoom.Bar space -> Literal space.Name
    | RehearsalRoom space -> Literal space.Name

let getRoomName room =
    match room with
    | RehearsalSpaceRoom.Lobby _ -> I18n.translate (CommonText CommonLobbyName)
    | RehearsalSpaceRoom.Bar _ -> I18n.translate (CommonText CommonBarName)
    | RehearsalRoom _ ->
        I18n.translate (RehearsalSpaceText RehearsalSpaceRehearsalRoomName)

let getRoomDescription room =
    match room with
    | RehearsalSpaceRoom.Lobby _ ->
        I18n.translate (RehearsalSpaceText RehearsalSpaceLobbyDescription)
    | RehearsalSpaceRoom.Bar _ ->
        I18n.translate (RehearsalSpaceText RehearsalSpaceBarDescription)
    | RehearsalRoom _ ->
        I18n.translate (
            RehearsalSpaceText RehearsalSpaceRehearsalRoomDescription
        )

let getRoomObjects room =
    let state = State.get ()

    let characterInstrument =
        currentPlayableMember state
        |> fun bandMember -> bandMember.Role
        |> instrumentFromType

    match room with
    | RehearsalSpaceRoom.Lobby _ -> []
    | RehearsalSpaceRoom.Bar _ -> []
    | RehearsalRoom _ -> [ characterInstrument ]

let getRoomCommands room =
    match room with
    | RehearsalSpaceRoom.Lobby _ -> []
    | RehearsalSpaceRoom.Bar _ -> []
    | RehearsalRoom _ ->
        [ { Name = "manage"
            Description =
                I18n.translate (
                    RehearsalSpaceText RehearsalRoomManageDescription
                )
            Handler =
                HandlerWithNavigation(fun _ -> seq { Scene Scene.Management }) } ]
