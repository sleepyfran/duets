module Cli.View.Scenes.World

open Cli.View.Commands
open Cli.View.Actions
open Cli.View.TextConstants
open Cli.View.Scenes.InteractiveSpaces
open Entities
open Simulation

let private listObjects objects =
    seq {
        if List.isEmpty objects then
            yield Message <| TextConstant CommandLookNoObjectsAround
        else
            yield
                Message
                <| TextConstant CommandLookVisibleObjectsPrefix

            yield!
                objects
                |> List.map
                    (fun object ->
                        let commandNames =
                            object.Commands
                            |> List.map (fun command -> command.Name)

                        (object.Type, commandNames))
                |> List.map (CommandLookObjectEntry >> TextConstant >> Message)
    }

/// Returns the name of the place that contains a given node. This means that,
/// in the case of a room, it'll return the name of the space itself instead
/// of the room (for example, the name of a rehearsal space instead of the room
/// name). Useful when the player is outside to describe places that they can
/// potentially enter instead of naming the room.
let private getPlaceName nodeContent =
    match nodeContent with
    | Room room ->
        match room with
        | RehearsalSpaceRoom room -> RehearsalRoom.Root.getPlaceName room
        | StudioRoom room -> Studio.Root.getPlaceName room
    | Street street -> Literal street.Name


/// Returns the name of a room. If the node is a street then returns the name
/// of the street.
let private getRoomName nodeContent =
    match nodeContent with
    | Room room ->
        match room with
        | RehearsalSpaceRoom room -> RehearsalRoom.Root.getRoomName room
        | StudioRoom room -> Studio.Root.getRoomName room
    | Street street -> Literal street.Name

let private listRoomEntrances entrances =
    let currentPosition =
        State.Root.get () |> Queries.World.currentPosition

    seq {
        yield
            entrances
            |> List.map
                (fun (direction, roomId) ->
                    let connectingNode =
                        Queries.World.contentOf
                            roomId
                            currentPosition.City.Graph

                    // When the player is inside a room we need to list the name
                    // of other rooms that connect with the current one, but when
                    // they are in a street then we need to display the name of
                    // the place itself (example: Rehearsal room's name instead
                    // of the lobby room)
                    let nodeName =
                        match currentPosition.NodeContent with
                        | Room _ -> getRoomName connectingNode
                        | Street _ -> getPlaceName connectingNode

                    (direction, nodeName))
            |> (CommandLookEntranceDescription
                >> TextConstant
                >> Message)
    }

let private createLookCommand entrances description objects =
    { Name = "look"
      Description = TextConstant CommandLookDescription
      Handler =
          HandlerWithoutNavigation
              (fun _ ->
                  seq {
                      yield Message description
                      yield! listObjects objects
                      yield NewLine
                      yield! listRoomEntrances entrances
                  }) }

let private getNodeDescription nodeContent =
    match nodeContent with
    | Room room ->
        match room with
        | RehearsalSpaceRoom room -> RehearsalRoom.Root.getRoomDescription room
        | StudioRoom room -> Studio.Root.getRoomDescription room
    | Street street ->
        match street.Descriptor with
        | Boring -> TextConstant(StreetBoringDescription street.Name)

let private getNodeObjects nodeContent =
    match nodeContent with
    | Room room ->
        match room with
        | RehearsalSpaceRoom room -> RehearsalRoom.Root.getRoomObjects room
        | StudioRoom room -> Studio.Root.getRoomObjects room
    | Street street ->
        match street.Descriptor with
        | Boring -> []

let private getNodeCommands nodeContent =
    match nodeContent with
    | Room room ->
        match room with
        | RehearsalSpaceRoom room -> RehearsalRoom.Root.getRoomCommands room
        | StudioRoom room -> Studio.Root.getRoomCommands room
    | Street street ->
        match street.Descriptor with
        | Boring -> []

let rec worldScene () =
    let currentPosition =
        State.Root.get () |> Queries.World.currentPosition

    let entrances =
        Queries.World.availableDirections
            currentPosition.NodeId
            currentPosition.City.Graph

    let description =
        getNodeDescription currentPosition.NodeContent

    let objects =
        getNodeObjects currentPosition.NodeContent

    let objectCommands =
        List.collect (fun object -> object.Commands) objects

    let commands =
        getNodeCommands currentPosition.NodeContent
        @ objectCommands
          @ [ (createLookCommand entrances description objects) ]
            @ DirectionsCommand.create entrances

    seq {
        yield Message description

        yield
            Prompt
                { Title = TextConstant CommonCommandPrompt
                  Content = CommandPrompt commands }
    }
