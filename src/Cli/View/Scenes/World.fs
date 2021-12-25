module Cli.View.Scenes.World

open Entities
open Cli.View.Actions
open Cli.View.TextConstants
open Cli.View.Scenes.InteractiveSpaces
open Simulation

let private createDirectionCommands entrances =
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
                  HandlerWithNavigation
                      (fun _ ->
                          seq {
                              yield
                                  State.Root.get ()
                                  |> World.Navigation.moveTo linkedNodeId
                                  |> Effect

                              yield Scene Scene.World
                          }) })

let private listObjects description objects =
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

let private listRoomEntrances space rooms entrances getRoomName =
    let state = State.Root.get ()

    seq {
        yield
            entrances
            |> List.map
                (fun (direction, roomId) ->
                    let roomContent = Queries.World.contentOf roomId rooms
                    (direction, getRoomName state space roomContent))
            |> (CommandLookEntranceDescription
                >> TextConstant
                >> Message)
    }

let private createLookCommand
    space
    rooms
    entrances
    getRoomName
    description
    objects
    =
    { Name = "look"
      Description = TextConstant CommandLookDescription
      Handler =
          HandlerWithoutNavigation
              (fun _ ->
                  seq {
                      yield! listObjects description objects
                      yield! listRoomEntrances space rooms entrances getRoomName
                  }) }

let getCityNodeName _ _ nodeContent =
    match nodeContent with
    | Place place ->
        match place with
        | Place.RehearsalSpace (space, _) -> Literal space.Name
        | Place.Studio (studio, _) -> Literal studio.Name
    | Street street -> Literal street.Name

let getStreetNodeDescription _ _ nodeContent =
    match nodeContent with
    | Street street ->
        match street.Descriptor with
        | Boring -> TextConstant(StreetBoringDescription street.Name)
    | _ -> Literal ""

let private createSpace<'space, 'room>
    nodeId
    (space: 'space)
    (graph: Graph<'room>)
    getNodeName
    getNodeDescription
    getNodeObjects
    getNodeCommands
    =
    let state = State.Root.get ()
    let content = Queries.World.contentOf nodeId graph

    let entrances =
        Queries.World.availableDirections nodeId graph

    let description = getNodeDescription state space content
    let objects = getNodeObjects state space content

    let objectCommands =
        List.collect (fun object -> object.Commands) objects

    let commands =
        getNodeCommands state space content
        @ objectCommands
          @ [ (createLookCommand
                  space
                  graph
                  entrances
                  getNodeName
                  description
                  objects) ]
            @ createDirectionCommands entrances

    seq {
        yield Message description
        yield! listRoomEntrances space graph entrances getNodeName

        yield
            Prompt
                { Title = TextConstant CommonCommandPrompt
                  Content = CommandPrompt commands }
    }

let private handlePlace content roomId =
    match content with
    | Place.RehearsalSpace (rehearsalSpace, rooms) ->
        let roomIdOrStarting =
            Option.defaultValue rooms.StartingNode roomId

        createSpace
            roomIdOrStarting
            rehearsalSpace
            rooms
            RehearsalRoom.Root.getRoomName
            RehearsalRoom.Root.getRoomDescription
            RehearsalRoom.Root.getRoomObjects
            RehearsalRoom.Root.getRoomCommands
    | Place.Studio (studio, rooms) ->
        [ Literal $"Inside of {studio.Name}, rooms are {rooms}"
          |> Message ]
        |> Seq.ofList

let private handleStreet () =
    let state = State.Root.get ()
    let currentPosition = Queries.World.currentPosition state

    createSpace
        currentPosition.NodeId
        currentPosition.NodeContent
        currentPosition.City.Graph
        getCityNodeName
        getStreetNodeDescription
        (fun _ _ _ -> [])
        (fun _ _ _ -> [])

let rec worldScene () =
    let state = State.Root.get ()
    let currentPosition = Queries.World.currentPosition state

    seq {
        yield!
            match currentPosition.NodeContent with
            | Place placeContent ->
                handlePlace placeContent currentPosition.RoomId
            | Street _ -> handleStreet ()
    }
