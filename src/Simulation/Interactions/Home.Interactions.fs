module Simulation.Interactions.Home

open Entities
open Simulation

/// Eating provides health back to the user. For now it gives just 5 of health
/// but later will evolve to actually depend on the food.
let eat state =
    let character =
        Queries.Characters.playableCharacter state

    [ Character.Attribute.add character CharacterAttribute.Health 5 ]

/// Sleeping restores energy and health per hour of sleep.
let sleep state hours =
    let character =
        Queries.Characters.playableCharacter state

    [ Character.Attribute.add character CharacterAttribute.Energy (hours * 10)
      Character.Attribute.add character CharacterAttribute.Health (hours * 2) ]
    @ Time.AdvanceTime.advanceDayMoment' state (hours / 4)

/// Watching TV restores a tiny bit amount of mood.
let watchTv state =
    let character =
        Queries.Characters.playableCharacter state

    [ Character.Attribute.add character CharacterAttribute.Mood 5 ]
    @ Time.AdvanceTime.advanceDayMoment' state 1

/// Playing Xbox restores a tiny bit amount of mood.
let playXbox state =
    let character =
        Queries.Characters.playableCharacter state

    [ Character.Attribute.add character CharacterAttribute.Mood 6 ]
    @ Time.AdvanceTime.advanceDayMoment' state 1
