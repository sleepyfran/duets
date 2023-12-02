namespace Duets.Simulation.Queries

open Duets.Entities
open Duets.Entities.SituationTypes
open Duets.Simulation

module InteractionCommon =
    let private filterOutMovement =
        function
        | Interaction.FreeRoam(FreeRoamInteraction.Move _) -> false
        | Interaction.FreeRoam FreeRoamInteraction.Map -> false
        | _ -> true

    let internal filterOutSituationalInteractions state =
        let situation = Situations.current state

        List.filter (fun interaction ->
            match situation with
            | FreeRoam -> true
            | Airport(Flying _) -> filterOutMovement interaction
            | Concert _ ->
                match interaction with (* TODO: Preserve some FreeRoam interaction. *)
                | Interaction.Concert _ -> true
                | Interaction.Item _ -> true
                | _ -> false
            | PlayingMiniGame _ ->
                match interaction with
                | Interaction.MiniGame _ -> true
                | _ -> false
            | Socializing _ ->
                match interaction with
                | Interaction.Social _ -> true
                | _ -> false)
