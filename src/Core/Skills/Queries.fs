module Core.Skills.Queries

open Entities
open Storage.State

/// Returns all skills from all characters in the game.
let characterSkills () =
  getState () |> fun state -> state.CharacterSkills

/// Queries the skills of a given character. Attempts to resolve a value, if
/// the character has no skills associated with the given ID, then it will
/// return a skill for the given ID with a level of 0.
let characterSkillWithLevel characterId skillId =
  characterSkills ()
  |> Map.tryFind characterId
  |> Option.defaultValue Map.empty
  |> Map.tryFind skillId
  |> Option.defaultValue (Skill.createSkillWithDefaultLevel skillId)
