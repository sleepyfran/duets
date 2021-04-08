module Mediator.Queries.Core

open Entities.Character
open Entities.Skill
open Mediator.Queries.Types

type GetCharacterSkillWithLevelInput =
  { CharacterId: CharacterId
    SkillId: SkillId }

/// Queries the skills of a given character. Attempts to resolve a value, if
/// the character has no skills associated with the given ID, the resolver
/// will return a skill for the given ID with a level of 0.
let CharacterSkillLevel input
                        : Query<GetCharacterSkillWithLevelInput, SkillWithLevel> =
  { Id = QueryId.CharacterSkillLevel
    Parameter = Some input }
