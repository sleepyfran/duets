module Simulation.Events.Events

open Entities

/// Retrieves all associated effects with the given one.
let associatedEffects effect =
    Time.run effect @ Skill.run effect @ Character.Character.run effect

/// Retrieves all the effects that have to happen at the end of an effect chain.
let endOfChainEffects = [ Place.ClosingTime.checkCurrentPlace ]
