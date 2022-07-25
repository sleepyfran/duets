module Simulation.State.Characters

open Aether
open Aether.Operators
open Entities

let add (character: Character) =
    let lens = Lenses.State.characters_

    Optic.map lens (Map.add character.Id character)

let setAttribute (characterId: CharacterId) attribute amount =
    let lens =
        Lenses.State.characters_ >-> Map.key_ characterId
        >?> Lenses.Character.attribute_ attribute

    Optic.set lens amount
