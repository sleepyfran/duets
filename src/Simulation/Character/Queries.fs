module Simulation.Character.Queries

open Entities
open Lenses
open Storage

/// Returns the character that the player is playing with.
let playableCharacter () =
  State.getState ()
  |> Lens.get StateLenses.Character
