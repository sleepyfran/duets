module Simulation.Interactions.Requirements.Health

open Entities
open Simulation

let private minimumHealth = 10

let private disabledReason =
    InteractionDisabledReason.NotEnoughHealth minimumHealth

/// Checks that the player has more than 10 points of health or disables
/// all interactions that cannot be performed with low health.
let check state interactions =
    let character =
        Queries.Characters.playableCharacter state

    if character.Status.Health > minimumHealth then
        interactions
    else
        interactions
        |> List.map (fun interaction ->
            match interaction with
            | InteractionState.Enabled enabledInteraction ->
                match enabledInteraction with
                | Interaction.Concert concertInteraction ->
                    match concertInteraction with
                    | ConcertInteraction.FinishConcert _ -> interaction
                    | _ ->
                        InteractionState.Disabled(
                            enabledInteraction,
                            disabledReason
                        )
                | Interaction.Rehearsal rehearsalInteraction ->
                    match rehearsalInteraction with
                    | RehearsalInteraction.FireMember _
                    | RehearsalInteraction.HireMember
                    | RehearsalInteraction.ListMembers _ -> interaction
                    | _ ->
                        InteractionState.Disabled(
                            enabledInteraction,
                            disabledReason
                        )
                | _ -> interaction
            | _ -> interaction)
