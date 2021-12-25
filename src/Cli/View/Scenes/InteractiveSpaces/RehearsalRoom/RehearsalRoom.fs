module Cli.View.Scenes.InteractiveSpaces.RehearsalRoom.Root

open Cli.View.Actions
open Cli.View.Scenes.InteractiveSpaces
open Cli.View.TextConstants
open Entities
open Simulation.Queries.Bands

let private instrumentFromType instrumentType =
    let create fn =
        fn
            (TextConstant RehearsalRoomInstrumentPlayDescription)
            (seq { yield! Compose.composeSubScene () })

    match instrumentType with
    | InstrumentType.Bass -> create Objects.bass
    | InstrumentType.Drums -> create Objects.drums
    | InstrumentType.Guitar -> create Objects.guitar
    | InstrumentType.Vocals -> create Objects.microphone

let getRoomName _ _ room =
    match room with
    | Lobby -> TextConstant RehearsalSpaceLobbyName
    | Bar -> TextConstant RehearsalSpaceBarName
    | RehearsalRoom -> TextConstant RehearsalSpaceRehearsalRoomName

let getRoomDescription _ _ room =
    match room with
    | Lobby -> TextConstant RehearsalSpaceLobbyDescription
    | Bar -> TextConstant RehearsalSpaceBarDescription
    | RehearsalRoom -> TextConstant RehearsalSpaceRehearsalRoomDescription

let getRoomObjects state _ room =
    let characterInstrument =
        currentPlayableMember state
        |> fun bandMember -> bandMember.Role
        |> instrumentFromType

    match room with
    | Lobby -> []
    | Bar -> []
    | RehearsalRoom -> [ characterInstrument ]

let getRoomCommands _ _ room =
    match room with
    | Lobby -> []
    | Bar -> []
    | RehearsalRoom ->
        [ { Name = "manage"
            Description = TextConstant RehearsalRoomManageDescription
            Handler =
                HandlerWithNavigation(fun _ -> seq { Scene Scene.Management }) } ]
