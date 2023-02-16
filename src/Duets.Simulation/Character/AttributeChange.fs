module Duets.Simulation.Character.AttributeChange

open Duets.Entities
open Duets.Simulation

/// Applies the attribute change that happens after every time advance.
let applyAfterTimeChange state =
    let playableCharacter = Queries.Characters.playableCharacter state

    Attribute.add playableCharacter CharacterAttribute.Hunger -20

/// Applies the attribute change that happens after waiting.
let applyAfterWait dayMoments state =
    let playableCharacter = Queries.Characters.playableCharacter state
    let multiplier = dayMoments / 1<dayMoments>

    Attribute.add playableCharacter CharacterAttribute.Energy (multiplier * -20)
