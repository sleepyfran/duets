module Duets.Simulation.State.Relationships

open Aether
open Duets.Entities

let change (npcId: CharacterId) (relationship: Relationship option) =
    let changeRelationship = Map.change npcId (fun _ -> relationship)
    Optic.map Lenses.State.relationships_ changeRelationship
