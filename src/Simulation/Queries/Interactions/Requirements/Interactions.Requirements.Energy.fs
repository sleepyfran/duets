namespace Simulation.Queries.Internal.Interactions


open Entities
open Simulation

module EnergyRequirements =
    let private disableIfLessEnergy
        interactionWithState
        currentEnergy
        energyRequired
        =
        { interactionWithState with
            State =
                if currentEnergy <= energyRequired then
                    InteractionDisabledReason.NotEnoughAttribute(
                        CharacterAttribute.Energy,
                        energyRequired
                    )
                    |> InteractionState.Disabled
                else
                    InteractionState.Enabled }

    let private disableIfLessEnergyThanMinimum
        interactionWithState
        currentEnergy
        =
        disableIfLessEnergy
            interactionWithState
            currentEnergy
            Config.LifeSimulation.Interactions.minimumEnergyRequired

    let private disableRehearsalInteraction
        interactionWithState
        characterEnergy
        rehearsalInteraction
        =
        match rehearsalInteraction with
        | RehearsalInteraction.FireMember _
        | RehearsalInteraction.HireMember
        | RehearsalInteraction.ListMembers _
        | RehearsalInteraction.ListSongs _ -> interactionWithState
        | _ ->
            disableIfLessEnergy
                interactionWithState
                characterEnergy
                Config.LifeSimulation.Interactions.minimumEnergyRequired

    let private disableCareerInteraction
        interactionWithState
        characterEnergy
        careerInteraction
        =
        match careerInteraction with
        | CareerInteraction.Work job ->
            let requiredEnergy =
                job.ShiftAttributeEffect
                |> List.tryFind (fun (attribute, _) ->
                    match attribute with
                    | CharacterAttribute.Energy -> true
                    | _ -> false)
                |> Option.map snd
                (* Effects on attrs are negative, so flip them to be able to compare them properly *)
                |> Option.map ((*) -1)

            match requiredEnergy with
            | Some requiredEnergy ->
                disableIfLessEnergy
                    interactionWithState
                    characterEnergy
                    requiredEnergy
            | _ -> interactionWithState

    /// Checks that the character has enough energy to perform an action. There's
    /// no "global" minimum energy like we do have in health, so it depends on
    /// the interaction that is to be performed.
    let check state interactions =
        let characterEnergy =
            Queries.Characters.playableCharacterAttribute
                state
                CharacterAttribute.Energy

        interactions
        |> List.map (fun interactionWithState ->
            match interactionWithState.State with
            | InteractionState.Enabled ->
                match interactionWithState.Interaction with
                | Interaction.Rehearsal rehearsalInteraction ->
                    disableRehearsalInteraction
                        interactionWithState
                        characterEnergy
                        rehearsalInteraction
                | Interaction.Career careerInteraction ->
                    disableCareerInteraction
                        interactionWithState
                        characterEnergy
                        careerInteraction
                | Interaction.FreeRoam (FreeRoamInteraction.Wait _) ->
                    disableIfLessEnergyThanMinimum
                        interactionWithState
                        characterEnergy
                | Interaction.FreeRoam _ -> interactionWithState
                | Interaction.Item (ItemInteraction.Consumable _) ->
                    interactionWithState
                | Interaction.Item (ItemInteraction.Interactive InteractiveItemInteraction.Sleep) ->
                    interactionWithState
                | _ ->
                    disableIfLessEnergyThanMinimum
                        interactionWithState
                        characterEnergy
            | _ -> interactionWithState)
