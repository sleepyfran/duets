module Core.Skills

open Entities
open Entities.State
open Mediator.Queries.Storage
open Mediator.Query

/// Queries the skills of a given character. Attempts to resolve a value, if
/// the character has no skills associated with the given ID, the resolver
/// will return a skill for the given ID with a level of 0.
let getCharacterSkillWithLevel (characterId, skillId) =
  query CharacterSkillsQuery
  |> Map.tryFind characterId
  |> Option.orElse (Some Map.empty)
  |> Option.bind (Map.tryFind skillId)
  |> Option.defaultValue (Skill.createSkillWithLevel skillId)
