module Duets.Simulation.Events.Events

open Duets.Entities
open Duets.Simulation.Events.Band
open Duets.Simulation.Events.Character

/// Retrieves all associated effects with the given one.
let associatedEffects effect =
    [ Time.run effect; Skill.run effect; Character.run effect; Band.run effect ]
    |> List.choose id

/// Retrieves all the effects that have to happen at the end of an effect chain.
let endOfChainEffects = [ Place.ClosingTime.checkCurrentPlace ]
