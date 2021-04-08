module Core.Resolvers.Skills.Queries

open Entities
open Entities.State
open Mediator.Queries.Core
open Mediator.Queries.Storage

/// Queries the skills of a given character. Attempts to resolve a value, if
/// the character has no skills associated with the given ID, the resolver
/// will return a skill for the given ID with a level of 0.
let getCharacterSkillWithLevel query input =
  query GetStateQuery
  |> fun state -> state.CharacterSkills
  |> Map.tryFind input.CharacterId
  |> Option.orElse (Some Map.empty)
  |> Option.bind (Map.tryFind input.SkillId)
  |> Option.defaultValue (Skill.createSkillWithLevel input.SkillId)
