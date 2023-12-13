namespace Duets.Simulation.Queries

open Aether
open Aether.Operators
open Duets.Entities

module Relationship =
    /// Returns all the relationships of the current character.
    let all state = state.Relationships |> _.ByCharacterId

    /// Returns the relationship between the current character and the given
    /// character ID. If no relationship exists, returns `None`.
    let withCharacter characterId =
        let lens =
            Lenses.State.relationships_
            >-> Lenses.Relationships.byCharacterId_
            >-> Map.key_ characterId

        Optic.get lens

    /// Returns a list of all the relationships of the current character that
    /// were met in the given city.
    let fromCity cityId state =
        let lens =
            Lenses.State.relationships_
            >-> Lenses.Relationships.byMeetingCityId_
            >-> Map.key_ cityId

        Optic.get lens state
        |> Option.defaultValue Set.empty
        |> List.ofSeq
        |> List.choose (fun characterId -> withCharacter characterId state)
