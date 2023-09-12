namespace Duets.Simulation.Queries

open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Queries.Internal.Interactions

module Interactions =
    let private getMovementInteractions (_, _, roomId) place =
        Queries.World.availableDirections roomId place.Rooms
        |> List.map (fun (direction, destinationId) ->
            FreeRoamInteraction.Move(direction, destinationId)
            |> Interaction.FreeRoam)

    /// <summary>
    /// Returns all interactions that are available in the current context. This
    /// effectively returns all available actions in the current context that can
    /// be later transformed into the actual flow.
    /// </summary>
    let availableCurrently state =
        let currentCoords = Queries.World.currentCoordinates state
        let cityId, _, _ = currentCoords
        let currentPlace = Queries.World.currentPlace state
        let currentRoom = Queries.World.currentRoom state

        let inventory = Inventory.get state

        let itemsInPlace = Queries.Items.allIn state currentCoords

        let itemInteractions =
            inventory @ itemsInPlace |> Items.getItemInteractions

        let careerInteractions = currentPlace |> Career.interactions state

        let movementInteractions =
            getMovementInteractions currentCoords currentPlace

        let defaultInteractions =
            itemInteractions
            @ movementInteractions
            @ careerInteractions
            @ [ FreeRoamInteraction.Inventory inventory |> Interaction.FreeRoam
                FreeRoamInteraction.Look itemsInPlace |> Interaction.FreeRoam
                Interaction.FreeRoam FreeRoamInteraction.Map
                Interaction.FreeRoam FreeRoamInteraction.Phone
                Interaction.FreeRoam FreeRoamInteraction.Wait ]

        match currentPlace.PlaceType with
        | Airport -> Airport.interactions state defaultInteractions
        | Bar ->
            Bar.interactions cityId currentRoom.RoomType @ defaultInteractions
        | Cafe -> Cafe.interactions currentRoom.RoomType @ defaultInteractions
        | Casino -> defaultInteractions
        | ConcertSpace _ ->
            ConcertSpace.interactions
                state
                currentRoom.RoomType
                defaultInteractions
                cityId
                currentPlace.Id
        | Gym ->
            Gym.interactions cityId currentPlace currentRoom.RoomType
            @ defaultInteractions
        | Home -> defaultInteractions
        | Hospital -> defaultInteractions
        | Hotel _ -> defaultInteractions
        | RehearsalSpace _ ->
            RehearsalSpace.interactions state cityId currentRoom.RoomType
            @ defaultInteractions
        | Restaurant ->
            Restaurant.interactions cityId currentRoom.RoomType
            @ defaultInteractions
        | Studio studio ->
            Studio.interactions state studio @ defaultInteractions
        |> List.map (fun interaction ->
            { Interaction = interaction
              State = InteractionState.Enabled
              TimeAdvance = InteractionTime.timeRequired interaction })
        |> HealthRequirements.check state
        |> EnergyRequirements.check state
