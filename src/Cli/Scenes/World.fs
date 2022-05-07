module Cli.Scenes.World

open Agents
open Cli.Components
open Cli.Text
open Cli.Scenes.InteractiveSpaces
open Cli.Scenes.InteractiveSpaces.Components
open Entities
open Simulation

let private getPlaceName nodeContent =
    match nodeContent with
    | ConcertPlace place -> Literal place.Space.Name
    | RehearsalPlace place -> Literal place.Space.Name
    | StudioPlace place -> Literal place.Space.Name
    | OutsideNode node -> Literal node.Name

let private chooseFreeRoamInteraction chooser interactions =
    interactions
    |> List.choose (fun interaction ->
        match interaction with
        | Interaction.FreeRoam freeRoamInteraction ->
            chooser freeRoamInteraction
        | _ -> None)

let private localizeEntrance entrances graph getNodeName =
    entrances
    |> List.map (fun (direction, coordinates) ->
        let nodeId =
            match coordinates with
            | Room (_, roomId) -> roomId
            | Node nodeId -> nodeId

        Queries.World.Common.contentOf graph nodeId
        |> getNodeName
        |> fun name -> (direction, name, coordinates))

/// Creates the world scene, which displays information about the current place
/// where the character is located as well as allowing the actions available
/// as given by the simulation layer.
let worldScene () =
    lineBreak ()

    let interactions =
        Interactions.availableCurrently (State.get ())

    let entrances =
        interactions
        |> chooseFreeRoamInteraction (fun freeRoamInteraction ->
            match freeRoamInteraction with
            | FreeRoamInteraction.Move (direction, nodeId) ->
                Some(direction, nodeId)
            | _ -> None)

    let exit =
        interactions
        |> chooseFreeRoamInteraction (fun freeRoamInteraction ->
            match freeRoamInteraction with
            | FreeRoamInteraction.GoOut (exitId, cityNode) ->
                Some(exitId, cityNode)
            | _ -> None)
        |> List.tryHead // There's only one exit per room/place.
        |> Option.map (fun (nodeId, cityNode) ->
            let nodeName = getPlaceName cityNode

            Node nodeId, nodeName)

    let currentPosition =
        State.get ()
        |> Queries.World.Common.currentPosition

    let _, roomId =
        match currentPosition.Coordinates with
        | Room (placeId, roomId) -> placeId, Some roomId
        | Node nodeId -> nodeId, None

    let localizedEntrances, description, objects, commands =
        match currentPosition.NodeContent with
        | ConcertPlace place ->
            let roomId =
                roomId
                |> Option.defaultValue place.Rooms.StartingNode

            let room =
                Queries.World.Common.contentOf place.Rooms roomId

            ((localizeEntrance
                entrances
                place.Rooms
                ConcertSpace.Root.getRoomName),
             ConcertSpace.Root.getRoomDescription place.Space room,
             ConcertSpace.Root.getRoomObjects room,
             ConcertSpace.Root.getRoomCommands room)
        | RehearsalPlace place ->
            let roomId =
                roomId
                |> Option.defaultValue place.Rooms.StartingNode

            let room =
                Queries.World.Common.contentOf place.Rooms roomId

            ((localizeEntrance
                entrances
                place.Rooms
                RehearsalRoom.Root.getRoomName),
             RehearsalRoom.Root.getRoomDescription room,
             RehearsalRoom.Root.getRoomObjects room,
             RehearsalRoom.Root.getRoomCommands room)
        | StudioPlace place ->
            let roomId =
                roomId
                |> Option.defaultValue place.Rooms.StartingNode

            let room =
                Queries.World.Common.contentOf place.Rooms roomId

            ((localizeEntrance entrances place.Rooms Studio.Root.getRoomName),
             Studio.Root.getRoomDescription place.Space room,
             Studio.Root.getRoomObjects room,
             Studio.Root.getRoomCommands place.Space room)
        | OutsideNode node ->
            let description =
                match node.Type with
                | Street -> WorldStreetDescription(node.Name, node.Descriptors)
                | Boulevard ->
                    WorldBoulevardDescription(node.Name, node.Descriptors)
                | Square -> WorldSquareDescription(node.Name, node.Descriptors)
                |> WorldText
                |> I18n.translate

            ((localizeEntrance entrances currentPosition.City.Graph getPlaceName),
             description,
             [],
             [])

    showWorldCommandPrompt localizedEntrances exit description objects commands
