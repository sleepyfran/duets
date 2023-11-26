namespace Duets.Simulation.Queries

open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Queries.Internal.Interactions

module Interactions =
    let private getClockInteraction state =
        let currentDate = Queries.Calendar.today state
        let dayMoments = Queries.Calendar.nextDates state

        dayMoments
        |> Seq.takeWhile (fun date -> date.Day = currentDate.Day)
        |> Seq.append (seq { currentDate })
        |> Seq.collect (fun date ->
            let dayMoment = Calendar.Query.dayMomentOf date

            let dayMomentWithEvents =
                Queries.CalendarEvents.forDayMoment state date dayMoment
                |> Seq.map (fun ((_, dayMoment), events) -> dayMoment, events)
                |> List.ofSeq

            match dayMomentWithEvents with
            | [] -> [ dayMoment, [] ]
            | _ -> dayMomentWithEvents)
        |> List.ofSeq
        |> FreeRoamInteraction.Clock
        |> Interaction.FreeRoam

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

        let freeRoamInteractions =
            FreeRoam.interactions
                state
                currentCoords
                currentPlace
                inventory
                itemsInPlace

        let itemInteractions =
            inventory @ itemsInPlace |> Items.getItemInteractions

        let careerInteractions = currentPlace |> Career.interactions state

        let clockInteraction = getClockInteraction state

        let defaultInteractions =
            clockInteraction :: itemInteractions
            @ freeRoamInteractions
            @ careerInteractions

        match currentPlace.PlaceType with
        | Airport ->
            Airport.interactions
                state
                cityId
                currentRoom.RoomType
                defaultInteractions
        | Bar ->
            Bar.interactions cityId currentRoom.RoomType @ defaultInteractions
        | Cafe -> Cafe.interactions currentRoom.RoomType @ defaultInteractions
        | Casino ->
            Casino.interactions state currentRoom.RoomType defaultInteractions
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
