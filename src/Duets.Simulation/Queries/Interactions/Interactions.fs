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
                CalendarEvents.forDayMoment state date dayMoment
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

        let inventory = Inventory.character state

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

        let socialInteractions = Social.interactions state

        let clockInteraction = getClockInteraction state

        let defaultInteractions =
            clockInteraction :: itemInteractions
            @ freeRoamInteractions
            @ careerInteractions
            @ socialInteractions

        let placeSpecificInteractions =
            match currentPlace.PlaceType with
            | Airport -> Airport.interactions state cityId currentRoom.RoomType
            | Bar -> Bar.interactions cityId currentRoom.RoomType
            | Bookstore -> Bookstore.interactions currentRoom.RoomType
            | Cafe -> Cafe.interactions currentRoom.RoomType
            | Casino -> Casino.interactions state currentRoom.RoomType
            | ConcertSpace _ ->
                ConcertSpace.interactions
                    state
                    currentRoom.RoomType
                    cityId
                    currentPlace.Id
            | Gym -> Gym.interactions cityId currentPlace currentRoom.RoomType
            | Home -> []
            | Hospital -> []
            | Hotel _ -> []
            | MerchandiseWorkshop ->
                MerchandiseWorkshop.interactions
                    state
                    currentCoords
                    currentRoom.RoomType
            | RehearsalSpace _ ->
                RehearsalSpace.interactions state cityId currentRoom.RoomType
            | Restaurant -> Restaurant.interactions cityId currentRoom.RoomType
            | Studio studio -> Studio.interactions state studio

        placeSpecificInteractions
        |> (@) defaultInteractions
        |> InteractionCommon.filterOutSituationalInteractions state
        |> List.map (fun interaction ->
            { Interaction = interaction
              State = InteractionState.Enabled
              TimeAdvance = InteractionTime.timeRequired interaction })
        |> HealthRequirements.check state
        |> EnergyRequirements.check state
