module Cli.Scenes.InteractiveSpaces.ConcertSpace.Root

open Aether
open Agents
open Cli.Components
open Cli.Scenes.InteractiveSpaces.Components
open Cli.Scenes.InteractiveSpaces.ConcertSpace.Commands
open Cli.Text
open Entities
open Simulation

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

    let _characterInstrument =
        Queries.Bands.currentPlayableMember state
        |> Optic.get Lenses.Band.CurrentMember.role_

    match room with
    | Lobby -> []
    | Bar -> []
    | Stage -> []

let private getRoomCommands _ = []

let private showConcertSpace city place placeId roomId =
    let room = Queries.World.Common.contentOf place.Rooms roomId

    let entrances =
        Queries.World.Common.availableDirections roomId place.Rooms
        |> List.map (fun (direction, connectedRoomId) ->
            Queries.World.Common.contentOf place.Rooms connectedRoomId
            |> getRoomName
            |> fun name -> (direction, name, Room(placeId, connectedRoomId)))

    let exit = exitOfNode city roomId place.Exits
    let description = getRoomDescription place.Space room
    let objects = getRoomObjects room
    let commands = getRoomCommands room

    showWorldCommandPrompt entrances exit description objects commands

let rec private showOngoingConcert ongoingConcert =
    let commands = [ PlaySongCommand.create ongoingConcert showOngoingConcert ]

    lineBreak ()

    Optic.get Lenses.Concerts.Ongoing.points_ ongoingConcert
    |> ConcertPoints
    |> ConcertText
    |> I18n.translate
    |> showMessage

    showCommandPromptWithoutDefaults
        (ConcertText ConcertActionPrompt |> I18n.translate)
        commands

/// Creates an interactive scene inside of a concert space in the given city,
/// place and room.
let rec concertSpace city place placeId roomId =
    let roomId =
        roomId
        |> Option.defaultValue place.Rooms.StartingNode

    let room = Queries.World.Common.contentOf place.Rooms roomId

    match room with
    | Stage ->
        if Queries.World.ConcertSpace.canEnterStage (State.get ()) placeId then
            ConcertSpaceStageDescription place.Space
            |> ConcertText
            |> I18n.translate
            |> showMessage

            showOngoingConcert { Events = []; Points = 0<quality> }
        else
            WorldText WorldConcertSpaceKickedOutOfStage
            |> I18n.translate
            |> showMessage

            Node placeId |> moveCharacter
    | _ -> showConcertSpace city place placeId roomId
