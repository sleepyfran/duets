namespace Simulation.Queries


open Entities
open Simulation
open Simulation.Queries.Internal.Interactions

module Interactions =
    /// <summary>
    /// Returns all interactions that are available in the current context. This
    /// effectively returns all available actions in the current context that can
    /// be later transformed into the actual flow.
    /// </summary>
    let availableCurrently state =
        let currentPlace =
            Queries.World.currentPlace state

        let inventory = Queries.Inventory.get state

        let itemInteractions =
            Items.getItemInteractions inventory

        let defaultInteractions =
            itemInteractions
            @ [ FreeRoamInteraction.Inventory inventory
                |> Interaction.FreeRoam
                Interaction.FreeRoam FreeRoamInteraction.Map
                Interaction.FreeRoam FreeRoamInteraction.Phone
                Interaction.FreeRoam FreeRoamInteraction.Wait ]

        match currentPlace.Type with
        | Airport -> Airport.interactions state defaultInteractions
        | Bar bar -> Shop.barInteractions bar @ defaultInteractions
        | ConcertSpace _ ->
            ConcertSpace.availableCurrently state defaultInteractions
        | Home -> defaultInteractions
        | Hospital -> defaultInteractions
        | RehearsalSpace _ ->
            RehearsalSpace.availableCurrently state
            @ defaultInteractions
        | Studio studio ->
            Studio.availableCurrently state studio
            @ defaultInteractions
        |> List.map (fun interaction ->
            { Interaction = interaction
              State = InteractionState.Enabled })
        |> HealthRequirements.check state
