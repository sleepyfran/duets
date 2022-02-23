module Cli.Scenes.World

open Agents
open Cli
open Cli.Components
open Cli.Components.Commands
open Cli.SceneIndex
open Cli.Text
open Cli.Scenes.InteractiveSpaces
open Entities
open Simulation

let private listObjects objects =
    if List.isEmpty objects then
        I18n.translate (CommandText CommandLookNoObjectsAround)
        |> showMessage
    else
        I18n.translate (CommandText CommandLookVisibleObjectsPrefix)
        |> showMessage

        objects
        |> List.map (fun object ->
            let commandNames =
                object.Commands
                |> List.map (fun command -> command.Name)

            (object.Type, commandNames))
        |> List.iter (
            CommandLookObjectEntry
            >> CommandText
            >> I18n.translate
            >> showMessage
        )

let private listRoomConnections entrances exit =
    entrances
    |> List.map (fun (a, b, _) -> a, b)
    |> CommandLookEntrances
    |> CommandText
    |> I18n.translate
    |> showMessage

    match exit with
    | Some (_, exitName) ->
        CommandLookExit exitName
        |> CommandText
        |> I18n.translate
        |> showMessage
    | _ -> ()

let private createLookCommand entrances exit description objects =
    { Name = "look"
      Description = I18n.translate (CommandText CommandLookDescription)
      Handler =
        (fun _ ->
            showMessage description
            listObjects objects
            lineBreak ()
            listRoomConnections entrances exit

            None) }

let private createOutCommand coordinates =
    { Name = "out"
      Description = I18n.translate (CommandText CommandOutDescription)
      Handler =
        (fun _ ->
            State.get ()
            |> World.Navigation.moveTo coordinates
            |> Effect.apply

            Some Scene.World) }

let private getPlaceName nodeContent =
    match nodeContent with
    | ConcertPlace place -> Literal place.Space.Name
    | RehearsalPlace place -> Literal place.Space.Name
    | StudioPlace place -> Literal place.Space.Name
    | OutsideNode node -> Literal node.Name

let private nodeInformation city nodeContent placeId roomId =
    let getEntrances nodeId graph getNodeName getCoordinates =
        Queries.World.availableDirections nodeId graph
        |> List.map (fun (direction, roomId) ->
            Queries.World.contentOf graph roomId
            |> getNodeName
            |> fun name -> (direction, name, getCoordinates roomId))

    let getExit nodeId exits =
        exits
        |> Map.tryFind nodeId
        |> Option.map (fun exitNodeId ->
            let exitNodeName =
                Queries.World.contentOf city.Graph exitNodeId
                |> getPlaceName

            Node exitNodeId, exitNodeName)

    match nodeContent with
    | ConcertPlace place ->
        let roomId =
            roomId
            |> Option.defaultValue place.Rooms.StartingNode

        let room = Queries.World.contentOf place.Rooms roomId

        (getEntrances
            roomId
            place.Rooms
            ConcertSpace.getRoomName
            (fun connectingId -> Room(placeId, connectingId)),
         (getExit roomId place.Exits),
         ConcertSpace.getRoomDescription room,
         ConcertSpace.getRoomObjects room,
         ConcertSpace.getRoomCommands room)
    | RehearsalPlace place ->
        let roomId =
            roomId
            |> Option.defaultValue place.Rooms.StartingNode

        let room = Queries.World.contentOf place.Rooms roomId

        (getEntrances
            roomId
            place.Rooms
            RehearsalRoom.Root.getRoomName
            (fun connectingId -> Room(placeId, connectingId)),
         (getExit roomId place.Exits),
         RehearsalRoom.Root.getRoomDescription room,
         RehearsalRoom.Root.getRoomObjects room,
         RehearsalRoom.Root.getRoomCommands room)
    | StudioPlace place ->
        let roomId =
            roomId
            |> Option.defaultValue place.Rooms.StartingNode

        let room = Queries.World.contentOf place.Rooms roomId

        (getEntrances
            roomId
            place.Rooms
            Studio.Root.getRoomName
            (fun connectingId -> Room(placeId, connectingId)),
         (getExit roomId place.Exits),
         Studio.Root.getRoomDescription room,
         Studio.Root.getRoomObjects room,
         Studio.Root.getRoomCommands room)
    | OutsideNode node ->
        let description =
            match node.Type with
            | Street -> WorldStreetDescription(node.Name, node.Descriptors)
            | Boulevard ->
                WorldBoulevardDescription(node.Name, node.Descriptors)
            | Square -> WorldSquareDescription(node.Name, node.Descriptors)
            |> WorldText
            |> I18n.translate

        (getEntrances placeId city.Graph getPlaceName Node,
         None,
         description,
         [],
         [])

let rec worldScene () =
    let currentPosition = State.get () |> Queries.World.currentPosition

    let placeId, roomId =
        match currentPosition.Coordinates with
        | Room (placeId, roomId) -> placeId, Some roomId
        | Node nodeId -> nodeId, None

    let (entrances, exit, description, objects, commands) =
        nodeInformation
            currentPosition.City
            currentPosition.NodeContent
            placeId
            roomId

    let objectCommands = List.collect (fun object -> object.Commands) objects

    let commands =
        commands
        @ objectCommands
          @ [ (createLookCommand entrances exit description objects) ]
            @ NavigationCommand.create entrances
              @ [ match exit with
                  | Some (coordinates, _) -> yield createOutCommand coordinates
                  | None -> () ]

    showMessage description
    listRoomConnections entrances exit

    showCommandPrompt
        (CommandText CommandCommonPrompt |> I18n.translate)
        commands
