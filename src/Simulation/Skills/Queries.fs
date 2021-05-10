module Simulation.Skills.Queries

open Aether
open Common
open Entities
open Storage

/// Returns all skills from all characters in the game.
let characterSkills state =
  state
  |> Optic.get Lenses.State.characterSkills_

/// Queries all the skills of a given character.
let characterSkillsWithLevel state characterId =
  characterSkills state
  |> Map.tryFind characterId
  |> Option.defaultValue Map.empty

/// Queries the skills of a given character. Attempts to resolve a value, if
/// the character has no skills associated with the given ID, then it will
/// return a skill for the given ID with a level of 0.
let characterSkillWithLevel state characterId skillId =
  characterSkillsWithLevel state characterId
  |> Map.tryFind skillId
  |> Option.defaultValue (Skill.createWithDefaultLevel skillId)

/// Calculates the average skill level of the given character.
let averageSkillLevel state characterId =
  characterSkillsWithLevel state characterId
  |> List.ofMapValues
  |> List.averageByOrDefault (snd >> float) 0
