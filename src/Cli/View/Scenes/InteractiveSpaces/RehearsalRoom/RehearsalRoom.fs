module Cli.View.Scenes.InteractiveSpaces.RehearsalRoom.Root

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
            (seq { yield! Compose.composeSubScene () })

    match instrumentType with
    | InstrumentType.Bass -> create Objects.bass
    | InstrumentType.Drums -> create Objects.drums
    | InstrumentType.Guitar -> create Objects.guitar
    | InstrumentType.Vocals -> create Objects.microphone

let getPlaceName room =
    match room with
    | Lobby space -> Literal space.Name
    | Bar space -> Literal space.Name
    | RehearsalRoom space -> Literal space.Name

let getRoomName room =
    match room with
    | Lobby _ -> I18n.translate (RehearsalSpaceText RehearsalSpaceLobbyName)
    | Bar _ -> I18n.translate (RehearsalSpaceText RehearsalSpaceBarName)
    | RehearsalRoom _ ->
        I18n.translate (RehearsalSpaceText RehearsalSpaceRehearsalRoomName)

let getRoomDescription room =
    match room with
    | Lobby _ ->
        I18n.translate (RehearsalSpaceText RehearsalSpaceLobbyDescription)
    | Bar _ -> I18n.translate (RehearsalSpaceText RehearsalSpaceBarDescription)
    | RehearsalRoom _ ->
        I18n.translate (
            RehearsalSpaceText RehearsalSpaceRehearsalRoomDescription
        )

let getRoomObjects room =
    let state = State.Root.get ()

    let characterInstrument =
        currentPlayableMember state
        |> fun bandMember -> bandMember.Role
        |> instrumentFromType

    match room with
    | Lobby _ -> []
    | Bar _ -> []
    | RehearsalRoom _ -> [ characterInstrument ]

let getRoomCommands room =
    match room with
    | Lobby _ -> []
    | Bar _ -> []
    | RehearsalRoom _ ->
        [ { Name = "manage"
            Description =
                I18n.translate (
                    RehearsalSpaceText RehearsalRoomManageDescription
                )
            Handler =
                HandlerWithNavigation(fun _ -> seq { Scene Scene.Management }) } ]
