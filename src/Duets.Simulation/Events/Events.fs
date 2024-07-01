module Duets.Simulation.Events.Events

open Duets.Simulation.Events.Band
open Duets.Simulation.Events.Character
open Duets.Simulation.Events.Moodlets

/// Retrieves all associated effects with the given one.
let associatedEffects effect =
    [ Band.run effect
      Career.run effect
      Character.run effect
      Concert.run effect
      Moodlets.run effect
      NonInteractiveGame.run effect
      Skill.run effect
      Time.run effect
      World.run effect ]
    |> List.choose id

/// Retrieves all the effects that have to happen at the end of an effect chain.
let endOfChainEffects = [ Place.ClosingTime.checkCurrentPlace ]
