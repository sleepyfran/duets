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

let getPlaceName room =
    match room with
    | Lobby space -> Literal space.Name
    | Bar space -> Literal space.Name
    | RehearsalRoom space -> Literal space.Name

let getRoomName room =
    match room with
    | Lobby _ -> TextConstant RehearsalSpaceLobbyName
    | Bar _ -> TextConstant RehearsalSpaceBarName
    | RehearsalRoom _ -> TextConstant RehearsalSpaceRehearsalRoomName

let getRoomDescription room =
    match room with
    | Lobby _ -> TextConstant RehearsalSpaceLobbyDescription
    | Bar _ -> TextConstant RehearsalSpaceBarDescription
    | RehearsalRoom _ -> TextConstant RehearsalSpaceRehearsalRoomDescription

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
            Description = TextConstant RehearsalRoomManageDescription
            Handler =
                HandlerWithNavigation(fun _ -> seq { Scene Scene.Management }) } ]
