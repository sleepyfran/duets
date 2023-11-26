namespace Duets.Simulation.Queries

open Aether
open Aether.Operators
open Duets.Entities

module Relationship =
    /// Returns the relationship between the current character and the given
    /// character ID. If no relationship exists, returns `None`.
    let withCharacter characterId =
        let lens = Lenses.State.relationships_ >-> Map.key_ characterId

        Optic.get lens
