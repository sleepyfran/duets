module Cli.View.Scenes.InteractiveSpaces.RehearsalRoom.Root

open Cli.View.Actions
open Cli.View.Scenes.InteractiveSpaces
open Cli.View.TextConstants
open Entities
open Simulation.Queries.Bands

let private instrumentFromType space rooms instrumentType =
    let create fn =
        fn
            (TextConstant RehearsalRoomInstrumentPlayDescription)
            (seq { yield! Compose.composeSubScene space rooms })

    match instrumentType with
    | InstrumentType.Bass -> create Objects.bass
    | InstrumentType.Drums -> create Objects.drums
    | InstrumentType.Guitar -> create Objects.guitar
    | InstrumentType.Vocals -> create Objects.microphone

/// Creates the rehearsal room which allows to access the compose and managing
/// section.
let rec rehearsalRoomScene space rooms =
    seq {
        yield!
            InteractiveSpace.create
                rooms
                (describeExit space rooms)
                (roomDescription space rooms)
                (roomObjects space rooms)
                (roomCommands space rooms)
    }

and describeExit _ _ room =
    match room with
    | Lobby -> TextConstant RehearsalSpaceLobbyName
    | Bar -> TextConstant RehearsalSpaceBarName
    | RehearsalRoom -> TextConstant RehearsalSpaceRehearsalRoomName

and roomDescription _ _ _ room =
    match room with
    | Lobby -> TextConstant RehearsalSpaceLobbyDescription
    | Bar -> TextConstant RehearsalSpaceBarDescription
    | RehearsalRoom -> TextConstant RehearsalSpaceRehearsalRoomDescription

and roomObjects space rooms state room =
    let characterInstrument =
        currentPlayableMember state
        |> fun bandMember -> bandMember.Role
        |> instrumentFromType space rooms

    match room with
    | Lobby -> []
    | Bar -> []
    | RehearsalRoom -> [ characterInstrument ]

and roomCommands space rooms _ room =
    match room with
    | Lobby -> []
    | Bar -> []
    | RehearsalRoom ->
        [ { Name = "manage"
            Description = TextConstant RehearsalRoomManageDescription
            Handler =
                HandlerWithNavigation
                    (fun _ -> seq { Scene(Management(space, rooms)) }) } ]
