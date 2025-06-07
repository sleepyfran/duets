module Duets.Simulation.Events.Character.Fame

open Duets.Entities
open Duets.Common
open Duets.Simulation

/// Updates the playable character's fame to be at least half of the band's fame.
let followBandsFame bandId state =
    let currentCharacterFame =
        Queries.Characters.playableCharacterAttribute
            state
            CharacterAttribute.Fame

    let estimatedBandFame = Queries.Bands.estimatedFameLevel state bandId

    let minimumFame = float estimatedBandFame * 0.5 |> Math.ceilToNearest

    if currentCharacterFame < minimumFame then
        Character.Attribute.setToPlayable
            CharacterAttribute.Fame
            minimumFame
            state
    else
        []
