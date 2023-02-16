namespace Duets.Simulation.Queries

module Situations =
    open Aether
    open Duets.Entities

    /// Returns the current situation that the character is in.
    let current state = Optic.get Lenses.State.situation_ state
