namespace Duets.Cli.Components.Commands

open Duets.Agents
open Duets.Cli.Components
open Duets.Cli.SceneIndex
open Duets.Cli.Text
open Duets.Cli.Text.World
open Duets.Entities
open Duets.Simulation

[<RequireQualifiedAccess>]
module LookCommand =
    let private listRoomConnections
        (interactions: InteractionWithMetadata list)
        =
        let state = State.get ()
        let cityId, placeId, _ = state |> Queries.World.currentCoordinates

        let connections =
            interactions
            |> List.choose (fun interaction ->
                match interaction.Interaction with
                | Interaction.FreeRoam(FreeRoamInteraction.Move(direction,
                                                                roomId)) ->
                    let roomType = Queries.World.roomById cityId placeId roomId

                    Some(direction, roomType)
                | _ -> None)

        match connections with
        | [] -> "There are no more rooms connecting to this one."
        | connections ->
            let connectionsDescription =
                Generic.listOf connections (fun (direction, room) ->
                    let roomName = World.roomName room.RoomType

                    $"{Generic.indeterminateArticleFor roomName} {roomName |> Styles.room} to the {World.directionName direction}")

            $"There is {connectionsDescription}."

    let create (interactions: InteractionWithMetadata list) (items: Item list) =
        { Name = "look"
          Description = Command.lookDescription
          Handler =
            (fun _ ->
                let state = State.get ()
                let currentPlace = state |> Queries.World.currentPlace
                let currentRoom = state |> Queries.World.currentRoom

                World.placeDescription currentPlace currentRoom.RoomType
                |> showMessage

                listRoomConnections interactions |> showMessage

                match items with
                | [] -> Command.lookNoObjectsAround |> showMessage
                | items ->
                    items
                    |> List.map (fun item ->
                        Generic.itemName item |> Items.lookItem)
                    |> Items.lookItems
                    |> showMessage

                Scene.World) }
