module Duets.Simulation.State.Relationships

open Aether
open Aether.Operators
open Duets.Common
open Duets.Entities

let changeForCharacterId
    (npcId: CharacterId)
    (relationship: Relationship option)
    =
    let changeById = Map.change npcId (fun _ -> relationship)

    Optic.map
        (Lenses.State.relationships_ >-> Lenses.Relationships.byCharacterId_)
        changeById

let changeForCityId
    (npcId: CharacterId)
    (cityId: CityId)
    (relationship: Relationship option)
    =
    let lens =
        Lenses.State.relationships_
        >-> Lenses.Relationships.byMeetingCityId_
        >-> Map.keyWithDefault_ cityId Set.empty

    let update =
        match relationship with
        | Some relationship -> Set.add relationship.Character
        | None -> Set.remove npcId

    Optic.map lens update
