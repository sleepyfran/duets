module Simulation.Skills.Queries

open Common
open Entities
open Storage.State

/// Returns all skills from all characters in the game.
let characterSkills () =
  getState () |> fun state -> state.CharacterSkills

/// Queries all the skills of a given character.
let characterSkillsWithLevel characterId =
  characterSkills ()
  |> Map.tryFind characterId
  |> Option.defaultValue Map.empty

/// Queries the skills of a given character. Attempts to resolve a value, if
/// the character has no skills associated with the given ID, then it will
/// return a skill for the given ID with a level of 0.
let characterSkillWithLevel characterId skillId =
  characterSkillsWithLevel characterId
  |> Map.tryFind skillId
  |> Option.defaultValue (Skill.createWithDefaultLevel skillId)

/// Calculates the average skill level of the given character.
let averageSkillLevel characterId =
  characterSkillsWithLevel characterId
  |> List.ofMapValues
  |> List.averageByOrDefault (snd >> float) 0
