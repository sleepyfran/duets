module Entities.State

open Entities.Band
open Entities.Calendar
open Entities.Character
open Entities.Skill
open Entities.Song

/// Shared state of the game. Contains all state that is common to every part
/// of the game.
type State =
  { Band: Band
    Character: Character
    CharacterSkills: Map<CharacterId, Map<SkillId, SkillWithLevel>>
    UnfinishedSongs: Map<BandId, UnfinishedWithQualities list>
    Today: Date }
