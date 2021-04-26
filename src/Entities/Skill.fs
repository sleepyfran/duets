module Entities.Skill

open Entities.Instrument

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
let create id = { Id = id; Category = categoryFor id }

/// Creates a new skill for a given ID with the level set to the given level. Its
/// category is automatically populated based on the type of skill given.
let createWithLevel id level = (create id, level)

/// Creates a new skill for a given ID with the level set to 0. Its category is
/// automatically populated based on the type of skill given.
let createWithDefaultLevel id = createWithLevel id 0
