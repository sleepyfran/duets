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
    | CityNode.Place place ->
        match place.Space with
        | ConcertSpace space -> Literal space.Name
        | RehearsalSpace space -> Literal space.Name
        | Studio space -> Literal space.Name
    | CityNode.OutsideNode node -> Literal node.Name

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

    let localizedEntrances, description, objects, commands =
        match currentPosition.Content with
        | ResolvedPlaceCoordinates coords ->
            let placeId, roomId = coords.Coordinates

            match coords.Place.Space with
            | ConcertSpace space ->
                let room =
                    Queries.World.Common.contentOf coords.Place.Rooms roomId

                ((localizeEntrance
                    entrances
                    coords.Place.Rooms
                    ConcertSpace.Root.getRoomName),
                 ConcertSpace.Root.getRoomDescription space room,
                 ConcertSpace.Root.getRoomObjects room,
                 ConcertSpace.Root.getRoomCommands room)
            | RehearsalSpace space ->
                let room =
                    Queries.World.Common.contentOf coords.Place.Rooms roomId

                ((localizeEntrance
                    entrances
                    coords.Place.Rooms
                    RehearsalRoom.Root.getRoomName),
                 RehearsalRoom.Root.getRoomDescription room,
                 RehearsalRoom.Root.getRoomObjects room,
                 RehearsalRoom.Root.getRoomCommands room)
            | Studio space ->
                let room =
                    Queries.World.Common.contentOf coords.Place.Rooms roomId

                ((localizeEntrance
                    entrances
                    coords.Place.Rooms
                    Studio.Root.getRoomName),
                 Studio.Root.getRoomDescription space room,
                 Studio.Root.getRoomObjects room,
                 Studio.Root.getRoomCommands space room)
        | ResolvedOutsideCoordinates coords ->
            let description =
                match coords.Node.Type with
                | Street ->
                    WorldStreetDescription(
                        coords.Node.Name,
                        coords.Node.Descriptors
                    )
                | Boulevard ->
                    WorldBoulevardDescription(
                        coords.Node.Name,
                        coords.Node.Descriptors
                    )
                | Square ->
                    WorldSquareDescription(
                        coords.Node.Name,
                        coords.Node.Descriptors
                    )
                |> WorldText
                |> I18n.translate

            ((localizeEntrance entrances currentPosition.City.Graph getPlaceName),
             description,
             [],
             [])

    showWorldCommandPrompt localizedEntrances exit description objects commands
