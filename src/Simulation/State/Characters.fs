module Simulation.State.Characters

open Aether
open Aether.Operators
open Entities

let private keyedCharacterStatus characterId =
    Lenses.State.characters_ >-> Map.key_ characterId
    >?> Lenses.Character.status_

let add (character: Character) =
    let lens = Lenses.State.characters_

    Optic.map lens (Map.add character.Id character)

let setEnergy (characterId: CharacterId) energy =
    let lens =
        keyedCharacterStatus characterId
        >?> Lenses.Character.Status.energy_

    Optic.set lens energy

let setHealth (characterId: CharacterId) health =
    let lens =
        keyedCharacterStatus characterId
        >?> Lenses.Character.Status.health_

    Optic.set lens health
