﻿namespace Duets.Simulation.Queries


open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Queries.Internal.Interactions

module Interactions =
    /// <summary>
    /// Returns all interactions that are available in the current context. This
    /// effectively returns all available actions in the current context that can
    /// be later transformed into the actual flow.
    /// </summary>
    let availableCurrently state =
        let currentCity = Queries.World.currentCity state
        let currentPlace = Queries.World.currentPlace state

        let inventory = Queries.Inventory.get state

        let itemsInPlace =
            Queries.Items.allIn state (currentCity.Id, currentPlace.Id)

        let itemInteractions =
            inventory @ itemsInPlace |> Items.getItemInteractions

        let careerInteractions = currentPlace |> Career.interactions state

        let defaultInteractions =
            itemInteractions
            @ careerInteractions
              @ [ FreeRoamInteraction.Inventory inventory
                  |> Interaction.FreeRoam
                  FreeRoamInteraction.Look itemsInPlace |> Interaction.FreeRoam
                  Interaction.FreeRoam FreeRoamInteraction.Map
                  Interaction.FreeRoam FreeRoamInteraction.Phone
                  Interaction.FreeRoam FreeRoamInteraction.Wait ]

        match currentPlace.Type with
        | Airport -> Airport.interactions state defaultInteractions
        | Bar bar -> Shop.shopInteractions bar @ defaultInteractions
        | Cafe coffeeShop ->
            Shop.shopInteractions coffeeShop @ defaultInteractions
        | ConcertSpace _ ->
            ConcertSpace.availableCurrently
                state
                defaultInteractions
                currentPlace.Id
        | Home -> defaultInteractions
        | Hospital -> defaultInteractions
        | RehearsalSpace _ ->
            RehearsalSpace.availableCurrently state @ defaultInteractions
        | Restaurant restaurant ->
            Shop.shopInteractions restaurant @ defaultInteractions
        | Studio studio ->
            Studio.availableCurrently state studio @ defaultInteractions
        |> List.map (fun interaction ->
            { Interaction = interaction
              State = InteractionState.Enabled })
        |> HealthRequirements.check state
        |> EnergyRequirements.check state