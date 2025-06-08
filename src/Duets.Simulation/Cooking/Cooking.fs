module Duets.Simulation.Cooking

open Duets.Simulation
open Duets.Entities
open Duets.Simulation.Skills.Improve.Common

/// Attempts to cook an item, checking if the character has enough money, and
/// granting a 25% chance of increasing their cooking skills.
let cook state itemWithPrice =
    let orderResult = Shop.order state itemWithPrice

    match orderResult with
    | Error e -> Error e
    | Ok shoppingEffects ->
        let character = Queries.Characters.playableCharacter state

        let skillImprovementEffects =
            applySkillModificationChance
                state
                {| CharacterId = character.Id
                   Skills = [ SkillId.Cooking ]
                   Chance = 25
                   ImprovementAmount = 1 |}

        Ok(shoppingEffects @ skillImprovementEffects)
