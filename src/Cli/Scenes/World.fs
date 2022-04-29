module Cli.Scenes.World

open Agents
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

let private getEntrances nodeId graph getNodeName getCoordinates =
    Queries.World.Common.availableDirections nodeId graph
    |> List.map
        (fun (direction, roomId) ->
            Queries.World.Common.contentOf graph roomId
            |> getNodeName
            |> fun name -> (direction, name, getCoordinates roomId))

let rec worldScene () =
    let currentPosition =
        State.get ()
        |> Queries.World.Common.currentPosition

    let placeId, roomId =
        match currentPosition.Coordinates with
        | Room (placeId, roomId) -> placeId, Some roomId
        | Node nodeId -> nodeId, None

    match currentPosition.NodeContent with
    | ConcertPlace place ->
        ConcertSpace.Root.concertSpace currentPosition.City place placeId roomId
    | RehearsalPlace place ->
        RehearsalRoom.Root.rehearsalSpace
            currentPosition.City
            place
            placeId
            roomId
    | StudioPlace place ->
        Studio.Root.studioSpace currentPosition.City place placeId roomId
    | OutsideNode node -> outsideNode currentPosition.City node placeId

and private outsideNode city node placeId =
    let description =
        match node.Type with
        | Street -> WorldStreetDescription(node.Name, node.Descriptors)
        | Boulevard -> WorldBoulevardDescription(node.Name, node.Descriptors)
        | Square -> WorldSquareDescription(node.Name, node.Descriptors)
        |> WorldText
        |> I18n.translate

    let entrances =
        getEntrances placeId city.Graph getPlaceName Node

    showWorldCommandPrompt entrances None description [] []
