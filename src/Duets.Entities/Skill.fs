module Duets.Entities.Skill

open Duets.Common
open Duets.Entities

/// Maps each type of skill with its category.
let categoryFor id =
    match id with
    | SkillId.Composition
    | SkillId.Genre _
    | SkillId.Instrument _ -> SkillCategory.Music
    | SkillId.MusicProduction -> SkillCategory.Production
    | SkillId.Cooking
    | SkillId.Fitness
    | SkillId.Speech -> SkillCategory.Character
    | SkillId.Barista
    | SkillId.Bartending
    | SkillId.Presenting -> SkillCategory.Job

/// Creates a new skill for a given ID. Its category is automatically populated
/// based on the type of skill given.
let create id = { Id = id; Category = categoryFor id }

/// Creates a new skill for a given ID with the level set to the given level. Its
/// category is automatically populated based on the type of skill given.
let createWithLevel id level = (create id, level)

/// Creates a new skill for a given ID with the level set to 0. Its category is
/// automatically populated based on the type of skill given.
let createWithDefaultLevel id = createWithLevel id 0

/// Creates a new skill for a given ID with the level set to a random +-5 (if
/// possible) of the given average. Its category is automatically populated
/// based on the type of the skill given.
let createFromAverageLevel id averageLevel =
    System.Random().Next(-5, 5)
    |> (+) averageLevel
    |> Math.clamp 1 100
    |> createWithLevel id
