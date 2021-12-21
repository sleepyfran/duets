[<RequireQualifiedAccess>]
module Cli.View.Scenes.InteractiveSpaces.InteractiveSpace

open Cli.View.Actions
open Cli.View.TextConstants
open Common
open Entities
open Simulation

/// Creates an interactive space given a graph of rooms and a bunch of functions
/// that given a room return the description, objects and extra commands of that
/// specific room. Handles things like navigating the graph by creating commands
/// for going north, south, west, etc. and showing the details of the room.
let rec create
    (rooms: Graph<'a>)
    getRoomName
    getRoomDescription
    getRoomObjects
    getRoomCommands
    =
    createRoom
        rooms.StartingNode
        rooms
        getRoomName
        getRoomDescription
        getRoomObjects
        getRoomCommands

and private createRoom
    roomId
    (rooms: Graph<'a>)
    getRoomName
    getRoomDescription
    getRoomObjects
    getRoomCommands
    =
    let state = State.Root.get ()
    let content = Queries.World.contentOf roomId rooms

    let entrances =
        Queries.World.availableDirections roomId rooms

    let description = getRoomDescription state content
    let objects = getRoomObjects state content

    let objectCommands =
        List.collect (fun object -> object.Commands) objects

    let commands =
        getRoomCommands state content
        @ objectCommands
          @ [ (createLookCommand rooms entrances getRoomName description objects) ]
            @ createDirectionCommands
                entrances
                rooms
                getRoomName
                getRoomDescription
                getRoomObjects
                getRoomCommands

    seq {
        yield Message description

        yield
            Prompt
                { Title = TextConstant CommonCommandPrompt
                  Content = CommandPrompt commands }
    }

and createDirectionCommands
    entrances
    (rooms: Graph<'a>)
    getRoomName
    getRoomDescription
    getRoomObjects
    getRoomCommands
    =
    entrances
    |> List.map
        (fun (direction, linkedNodeId) ->
            let commandName =
                match direction with
                | North -> "north"
                | NorthEast -> "north-east"
                | East -> "east"
                | SouthEast -> "south-east"
                | South -> "south"
                | SouthWest -> "south-west"
                | West -> "west"
                | NorthWest -> "north-west"

            { Name = commandName
              Description =
                  TextConstant
                  <| CommandDirectionDescription direction
              Handler =
                  HandlerWithoutNavigation
                      (fun _ ->
                          seq {
                              yield!
                                  createRoom
                                      linkedNodeId
                                      rooms
                                      getRoomName
                                      getRoomDescription
                                      getRoomObjects
                                      getRoomCommands
                          }) })

and createLookCommand rooms entrances getRoomName description objects =
    { Name = "look"
      Description = TextConstant CommandLookDescription
      Handler =
          HandlerWithoutNavigation
              (fun _ ->
                  seq {
                      yield! listObjects description objects
                      yield! listRoomEntrances rooms entrances getRoomName
                  }) }

and listObjects description objects =
    seq {
        if List.isEmpty objects then
            yield Message <| TextConstant CommandLookNoObjectsAround
        else
            yield
                CommandLookEnvironmentDescription description
                |> TextConstant
                |> Message

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

and listRoomEntrances rooms entrances (getRoomName: 'a -> Text) =
    seq {
        yield
            entrances
            |> List.map
                (fun (direction, roomId) ->
                    let roomContent = Queries.World.contentOf roomId rooms
                    (direction, getRoomName roomContent))
            |> (CommandLookEntranceDescription
                >> TextConstant
                >> Message)
    }
