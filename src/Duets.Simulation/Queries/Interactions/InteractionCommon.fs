namespace Duets.Simulation.Queries

open Duets.Entities
open Duets.Entities.SituationTypes

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
            | Airport(Flying _) ->
                filterOutMovement interaction
                && match interaction with
                   | Interaction.FreeRoam FreeRoamInteraction.Wait -> false
                   | _ -> true
            | Concert(Preparing _) ->
                (*
                Since we keep the status of the concert in the situation, disallow
                the player from leaving the venue until the concert is done.
                TODO: Figure out a better way to keep the state?
                *)
                match interaction with
                | Interaction.FreeRoam FreeRoamInteraction.Map -> false
                | _ -> true
            | Concert(InConcert _) ->
                match interaction with
                | Interaction.FreeRoam(FreeRoamInteraction.Look _) -> true
                | Interaction.Concert _ -> true
                | _ -> false
            | PlayingMiniGame _ ->
                match interaction with
                | Interaction.MiniGame _ -> true
                | _ -> false
            | Socializing _ ->
                match interaction with
                | Interaction.Social _ -> true
                | _ -> false
            | Travelling _ ->
                match interaction with
                | Interaction.FreeRoam(FreeRoamInteraction.Clock _)
                | Interaction.FreeRoam(FreeRoamInteraction.Inventory _)
                | Interaction.FreeRoam FreeRoamInteraction.Phone -> true
                | Interaction.FreeRoam _ -> false
                | Interaction.Travel _ -> true
                | _ -> false)

    let internal filterOutPlaceSpecificInteractions place =
        List.filter (fun interaction ->
            match place.PlaceType with
            | MetroStation ->
                (*
                If the player is in a metro station, wait is replaced with another
                action that waits for the next train instead of passing a day moment.
                *)
                match interaction with
                | Interaction.FreeRoam FreeRoamInteraction.Wait -> false
                | _ -> true
            | _ -> true)
