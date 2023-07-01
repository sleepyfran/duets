namespace Duets.Simulation.Queries

open Duets.Entities

module InteractionCommon =
    /// Filters out all interactions that would make the character move, travel
    /// or otherwise pass time.
    let internal filterOutMovementAndTime =
        List.filter (fun interaction ->
            match interaction with
            | Interaction.FreeRoam(FreeRoamInteraction.Move _) -> false
            | Interaction.FreeRoam FreeRoamInteraction.Map -> false
            | Interaction.FreeRoam FreeRoamInteraction.Wait -> false
            | _ -> true)
