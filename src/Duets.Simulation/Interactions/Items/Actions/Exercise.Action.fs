module rec Duets.Simulation.Interactions.Actions.Exercise

open Duets.Entities
open Duets.Simulation

/// Applies the effects of exercising with an item.
let exercise item character state =
    [ yield Exercised item
      yield!
          Character.Attribute.add
              character
              CharacterAttribute.Energy
              Config.LifeSimulation.Energy.exerciseIncrease
      yield!
          Character.Attribute.add
              character
              CharacterAttribute.Health
              Config.LifeSimulation.Health.exerciseIncrease
      yield!
          Skills.Improve.Common.applySkillModificationChance
              state
              {| Chance = 30
                 CharacterId = character.Id
                 ImprovementAmount = 1
                 Skills = [ SkillId.Fitness ] |} ]
