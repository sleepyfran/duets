﻿module Simulation.Interactions.Requirements.Health

open Entities
open Simulation

let private minimumHealth = 10

let private disable interactionWithState =
    { interactionWithState with
        State =
            (InteractionDisabledReason.NotEnoughHealth minimumHealth)
            |> InteractionState.Disabled }

/// Checks that the player has more than 10 points of health or disables
/// all interactions that cannot be performed with low health.
let check state interactions =
    let character =
        Queries.Characters.playableCharacter state

    if character.Status.Health > minimumHealth then
        interactions
    else
        interactions
        |> List.map (fun interactionWithState ->
            match interactionWithState.State with
            | InteractionState.Enabled ->
                match interactionWithState.Interaction with
                | Interaction.Concert concertInteraction ->
                    match concertInteraction with
                    | ConcertInteraction.FinishConcert _ -> interactionWithState
                    | _ -> disable interactionWithState
                | Interaction.Rehearsal rehearsalInteraction ->
                    match rehearsalInteraction with
                    | RehearsalInteraction.FireMember _
                    | RehearsalInteraction.HireMember
                    | RehearsalInteraction.ListMembers _ -> interactionWithState
                    | _ -> disable interactionWithState
                | _ -> interactionWithState
            | _ -> interactionWithState)