module Simulation.Events.Character.Drunkenness

open Entities
open Simulation

/// Sobers up the character after each passing time unit.
let soberUpAfterTime state =
    let character = Queries.Characters.playableCharacter state

    Character.Attribute.conditionalAdd
        character
        CharacterAttribute.Drunkenness
        (Character.Attribute.moreThanZero
            character
            CharacterAttribute.Drunkenness)
        Config.LifeSimulation.drunkennessReduceRate

/// Reduces the health of the character when they're too drunk.
let reduceHealth state =
    let character = Queries.Characters.playableCharacter state

    Character.Attribute.conditionalAdd
        character
        CharacterAttribute.Health
        (Character.Attribute.moreThan
            character
            CharacterAttribute.Drunkenness
            85)
        Config.LifeSimulation.drunkHealthReduceRate
