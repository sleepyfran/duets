module Simulation.Events.Character.Hunger

open Entities
open Simulation

/// Reduces the health of the character the hunger is too low.
let reduceHealth state =
    let character = Queries.Characters.playableCharacter state

    Character.Attribute.conditionalAdd
        character
        CharacterAttribute.Health
        (Character.Attribute.lessThan character CharacterAttribute.Hunger 5)
        Config.LifeSimulation.hungerHealthReduceRage
