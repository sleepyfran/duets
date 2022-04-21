module Simulation.State.Characters

open Aether
open Entities

let add (character: Character) =
    let lens = Lenses.State.characters_

    Optic.map lens (Map.add character.Id character)
