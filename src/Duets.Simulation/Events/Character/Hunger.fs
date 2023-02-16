module Duets.Simulation.Events.Character.Hunger

open Duets.Entities
open Duets.Simulation

/// Reduces the health of the character the hunger is too low.
let reduceHealth state =
    let character = Queries.Characters.playableCharacter state

    Character.Attribute.conditionalAdd
        character
        CharacterAttribute.Health
        (Character.Attribute.lessThan character CharacterAttribute.Hunger 5)
        Config.LifeSimulation.hungerHealthReduceRage
