module Cli.Scenes.InteractiveSpaces.ConcertSpace

open Agents
open Cli.Components
open Cli.Scenes.InteractiveSpaces
open Cli.Scenes.InteractiveSpaces.Components
open Cli.Text
open Entities
open Simulation

let private instrumentFromType instrumentType =
    let create fn =
        fn
            (I18n.translate (ConcertText ConcertSpaceStartConcert))
            (fun _ -> None)

    match instrumentType with
    | InstrumentType.Bass -> create Objects.bass
    | InstrumentType.Drums -> create Objects.drums
    | InstrumentType.Guitar -> create Objects.guitar
    | InstrumentType.Vocals -> create Objects.microphone

let private getRoomName room =
    match room with
    | Lobby _ -> I18n.translate (CommonText CommonLobbyName)
    | Bar _ -> I18n.translate (CommonText CommonBarName)
    | Stage _ -> I18n.translate (ConcertText ConcertSpaceStageName)

let private getRoomDescription space room =
    match room with
    | Lobby ->
        I18n.translate (ConcertSpaceLobbyDescription space |> ConcertText)
    | Bar -> I18n.translate (ConcertSpaceBarDescription space |> ConcertText)
    | Stage ->
        I18n.translate (ConcertSpaceStageDescription space |> ConcertText)

let private getRoomObjects room =
    let state = State.get ()

    let characterInstrument =
        Queries.Bands.currentPlayableMember state
        |> fun bandMember -> bandMember.Role
        |> instrumentFromType

    match room with
    | Lobby -> []
    | Bar -> []
    | Stage -> [ characterInstrument ]

let private getRoomCommands _ = []

let private showConcertSpace city place placeId roomId =
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
    let description = getRoomDescription place.Space room
    let objects = getRoomObjects room
    let commands = getRoomCommands room

    showWorldCommandPrompt entrances exit description objects commands

/// Creates an interactive scene inside of a concert space in the given city,
/// place and room.
let rec concertSpace city place placeId roomId =
    let roomId =
        roomId
        |> Option.defaultValue place.Rooms.StartingNode

    let room =
        Queries.World.Common.contentOf place.Rooms roomId

    match room with
    | Stage ->
        if Queries.World.ConcertSpace.canEnterStage (State.get ()) placeId then
            showConcertSpace city place placeId roomId
        else
            WorldText WorldConcertSpaceKickedOutOfStage
            |> I18n.translate
            |> showMessage

            Node placeId |> moveCharacter
    | _ -> showConcertSpace city place placeId roomId
