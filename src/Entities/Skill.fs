module Entities.Skill

open Entities.Band
open Entities.Genre
open Entities.Instrument

type SkillId =
  | Composition
  | Genre of Genre
  | Instrument of Instrument

/// Defines all possible categories to which skills can be related to.
type Category =
  | Music
  | Production

/// Represents a skill that the character can have. This only includes the base
/// fields of the skill, more specific types are available depending on what
/// information we need.
type Skill = { Id: SkillId; Category: Category }

/// Defines the relation between a skill and its level.
type SkillWithLevel = Skill * int

/// Maps each role that a member takes in a band with its related skill.
let skillIdForRole role =
  match role with
  | Singer -> Instrument <| createInstrument Vocals
  | Guitarist -> Instrument <| createInstrument Guitar
  | Bassist -> Instrument <| createInstrument Bass
  | Drummer -> Instrument <| createInstrument Drums

/// Maps each type of skill with its category.
let categoryFor id =
  match id with
  | Composition -> Music
  | Genre _ -> Music
  | Instrument _ -> Music

/// Creates a new skill for a given ID. Its category is automatically populated
/// based on the type of skill given.
let createSkill id = { Id = id; Category = categoryFor id }

/// Creates a new skill for a given ID with the level set to 0. Its category is
/// automatically populated based on the type of skill given.
let createSkillWithLevel id = (createSkill id, 0)
