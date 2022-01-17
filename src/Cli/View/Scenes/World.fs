module Cli.View.Scenes.World

open Agents
open Cli.View.Commands
open Cli.View.Actions
open Cli.View.Text
open Cli.View.Scenes.InteractiveSpaces
open Entities
open Simulation

let private listObjects objects =
    seq {
        if List.isEmpty objects then
            yield
                I18n.translate (CommandText CommandLookNoObjectsAround)
                |> Message
        else
            yield
                I18n.translate (CommandText CommandLookVisibleObjectsPrefix)
                |> Message

            yield!
                objects
                |> List.map
                    (fun object ->
                        let commandNames =
                            object.Commands
                            |> List.map (fun command -> command.Name)

                        (object.Type, commandNames))
                |> List.map (
                    CommandLookObjectEntry
                    >> CommandText
                    >> I18n.translate
                    >> Message
                )
    }

/// Returns the name of the place that contains a given node. This means that,
/// in the case of a room, it'll return the name of the space itself instead
/// of the room (for example, the name of a rehearsal space instead of the room
/// name). Useful when the player is outside to describe places that they can
/// potentially enter instead of naming the room.
let private getPlaceName nodeContent =
    match nodeContent with
    | InsideNode room ->
        match room with
        | ConcertSpaceRoom room -> ConcertSpace.getPlaceName room
        | RehearsalSpaceRoom room -> RehearsalRoom.Root.getPlaceName room
        | StudioRoom room -> Studio.Root.getPlaceName room
    | OutsideNode street -> Literal street.Name


/// Returns the name of a room. If the node is a street then returns the name
/// of the street.
let private getRoomName nodeContent =
    match nodeContent with
    | InsideNode room ->
        match room with
        | RehearsalSpaceRoom room -> RehearsalRoom.Root.getRoomName room
        | StudioRoom room -> Studio.Root.getRoomName room
        | ConcertSpaceRoom room -> ConcertSpace.getRoomName room
    | OutsideNode street -> Literal street.Name

type private NodeKey =
    | InsideNodeKey
    | OutsideNodeKey

let private listRoomEntrances entrances =
    let currentPosition =
        State.get () |> Queries.World.currentPosition

    let entrancesByNodeKey =
        entrances
        |> List.fold
            (fun acc (direction, roomId) ->
                let connectingNode =
                    Queries.World.contentOf roomId currentPosition.City.Graph

                let addToMapList key nodeName =
                    Map.change
                        key
                        (fun x ->
                            match x with
                            | Some entrances ->
                                entrances @ [ (direction, nodeName) ] |> Some
                            | None -> Some [ (direction, nodeName) ])

                match currentPosition.NodeContent with
                // When the player is outside every connection counts as
                // outside since we're listing the name of the building
                // and not the room name.
                | OutsideNode _ ->
                    addToMapList
                        OutsideNodeKey
                        (getPlaceName connectingNode)
                        acc
                | InsideNode _ ->
                    // When the player is inside a room we need to list the name
                    // of other rooms that connect with the current one, but when
                    // they are in a street then we need to display the name of
                    // the place itself (example: Rehearsal room's name instead
                    // of the lobby room)
                    match connectingNode with
                    | InsideNode _ ->
                        addToMapList
                            InsideNodeKey
                            (getRoomName connectingNode)
                            acc
                    | OutsideNode _ ->
                        addToMapList
                            OutsideNodeKey
                            (getPlaceName connectingNode)
                            acc)
            Map.empty

    let insideEntrances =
        Map.tryFind InsideNodeKey entrancesByNodeKey

    let outsideEntrances =
        Map.tryFind OutsideNodeKey entrancesByNodeKey

    seq {
        match insideEntrances with
        | Some entrances ->
            yield
                CommandLookInsideEntrances entrances
                |> CommandText
                |> I18n.translate
                |> Message
        | _ -> ()

        match outsideEntrances with
        | Some entrances ->
            yield
                CommandLookOutsideEntrances entrances
                |> CommandText
                |> I18n.translate
                |> Message
        | _ -> ()
    }

let private createLookCommand entrances description objects =
    { Name = "look"
      Description = I18n.translate (CommandText CommandLookDescription)
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
    | InsideNode room ->
        match room with
        | RehearsalSpaceRoom room -> RehearsalRoom.Root.getRoomDescription room
        | StudioRoom room -> Studio.Root.getRoomDescription room
        | ConcertSpaceRoom room -> ConcertSpace.getRoomDescription room
    | OutsideNode street ->
        match street.Type with
        | Street -> WorldStreetDescription(street.Name, street.Descriptors)
        | Boulevard ->
            WorldBoulevardDescription(street.Name, street.Descriptors)
        | Square -> WorldSquareDescription(street.Name, street.Descriptors)
        |> WorldText
        |> I18n.translate

let private getNodeObjects nodeContent =
    match nodeContent with
    | InsideNode room ->
        match room with
        | RehearsalSpaceRoom room -> RehearsalRoom.Root.getRoomObjects room
        | StudioRoom room -> Studio.Root.getRoomObjects room
        | ConcertSpaceRoom room -> ConcertSpace.getRoomObjects room
    | OutsideNode street ->
        match street.Descriptors with
        | _ -> []

let private getNodeCommands nodeContent =
    match nodeContent with
    | InsideNode room ->
        match room with
        | RehearsalSpaceRoom room -> RehearsalRoom.Root.getRoomCommands room
        | StudioRoom room -> Studio.Root.getRoomCommands room
        | ConcertSpaceRoom room -> ConcertSpace.getRoomCommands room
    | OutsideNode street ->
        match street.Descriptors with
        | _ -> []

let rec worldScene () =
    let currentPosition =
        State.get () |> Queries.World.currentPosition

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
        yield! listRoomEntrances entrances

        yield
            Prompt
                { Title = I18n.translate (CommandText CommandCommonPrompt)
                  Content = CommandPrompt commands }
    }
