module Duets.Simulation.Events.Character.Fame

open Duets.Entities
open Duets.Common
open Duets.Simulation

/// Updates the playable character's fame to be always half of the band's fame.
let followBandsFame bandId state =
    let estimatedBandFame = Queries.Bands.estimatedFameLevel state bandId

    let updatedCharacterFame =
        float estimatedBandFame * 0.5 |> Math.ceilToNearest

    Character.Attribute.setToPlayable
        CharacterAttribute.Fame
        updatedCharacterFame
        state
