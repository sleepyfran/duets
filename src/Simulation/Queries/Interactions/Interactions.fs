namespace Simulation.Queries


open Entities
open Simulation
open Simulation.Queries.Internal.Interactions

module Interactions =
    let private getOutToInteractions state destinationId =
        let cityNode =
            Queries.World.Common.coordinates state (Node destinationId)

        let outsideCoordinates =
            match cityNode.Content with
            | ResolvedOutsideCoordinates coords -> coords
            | _ -> failwith "Can't go out to a inside node"

        FreeRoamInteraction.GoOut(destinationId, outsideCoordinates)
        |> Interaction.FreeRoam

    let private getOutsideInteractions nodeId city =
        Queries.World.Common.availableDirections nodeId city.Graph
        |> List.map (fun (direction, destinationId) ->
            FreeRoamInteraction.Move(direction, Node destinationId)
            |> Interaction.FreeRoam)

    let private getNavigationInteractions state placeId roomId place =
        let moveInteractions =
            Queries.World.Common.availableDirections roomId place.Rooms
            |> List.map (fun (direction, destinationId) ->
                FreeRoamInteraction.Move(
                    direction,
                    Room(placeId, destinationId)
                )
                |> Interaction.FreeRoam)

        let exitInteraction =
            place.Exits
            |> Map.tryFind roomId
            |> Option.map (fun exitId -> [ getOutToInteractions state exitId ])
            |> Option.defaultValue []

        moveInteractions @ exitInteraction

    let private getGenericRoomInteractions room =
        match room with
        | RoomType.Bar bar -> Shop.barInteractions bar
        | _ -> []

    let private getItemInteractions (items: Item list) =
        items
        |> Set.ofList
        |> Set.map (fun item ->
            match item.Type with
            | ItemType.Drink _ -> ItemInteraction.Drink |> Interaction.Item
            | ItemType.Food _ -> ItemInteraction.Eat |> Interaction.Item)
        |> List.ofSeq

    /// <summary>
    /// Returns all interactions that are available in the current context. This
    /// effectively returns all available actions in the current context that can
    /// be later transformed into the actual flow.
    /// </summary>
    let availableCurrently state =
        let currentPosition =
            Queries.World.Common.currentPosition state

        let currentWorldCoords =
            Queries.World.Common.currentWorldCoordinates state

        let inventory = Queries.Inventory.get state

        let itemsInPlace =
            Queries.Items.allIn state currentWorldCoords

        let itemInteractions =
            inventory @ itemsInPlace |> getItemInteractions

        let defaultInteractions =
            itemInteractions
            @ [ FreeRoamInteraction.Inventory inventory
                |> Interaction.FreeRoam
                FreeRoamInteraction.Look itemsInPlace
                |> Interaction.FreeRoam
                Interaction.FreeRoam FreeRoamInteraction.Phone
                Interaction.FreeRoam FreeRoamInteraction.Wait ]

        let allAvailableInteractions =
            match currentPosition.Content with
            | ResolvedPlaceCoordinates coords ->
                let placeId, roomId = coords.Coordinates

                let navigationInteractions =
                    getNavigationInteractions state placeId roomId coords.Place

                let genericRoomInteractions =
                    getGenericRoomInteractions coords.Room

                match coords.Place.SpaceType with
                | ConcertSpace _ ->
                    ConcertSpace.availableCurrently
                        state
                        coords.Room
                        navigationInteractions
                        genericRoomInteractions
                        defaultInteractions
                | Home ->
                    Home.availableCurrently coords.Room
                    @ navigationInteractions @ defaultInteractions
                | Hospital -> navigationInteractions @ defaultInteractions
                | RehearsalSpace _ ->
                    RehearsalSpace.availableCurrently state coords.Room
                    @ navigationInteractions
                      @ genericRoomInteractions @ defaultInteractions
                | Studio studio ->
                    Studio.availableCurrently state studio coords.Room
                    @ navigationInteractions @ defaultInteractions
            | ResolvedOutsideCoordinates coords ->
                getOutsideInteractions coords.Coordinates currentPosition.City
                @ defaultInteractions

        allAvailableInteractions
        |> List.map (fun interaction ->
            ({ Interaction = interaction
               State = InteractionState.Enabled }))
        |> HealthRequirements.check state
