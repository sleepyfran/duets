module Entities.State

open Entities.Band
open Entities.Calendar
open Entities.Character
open Entities.Skill
open Entities.Song

type CharacterSkills = Map<CharacterId, Map<SkillId, SkillWithLevel>>
type UnfinishedSongs = Map<BandId, UnfinishedWithQualities list>

/// Shared state of the game. Contains all state that is common to every part
/// of the game.
type State =
  { Band: Band
    Character: Character
    CharacterSkills: CharacterSkills
    UnfinishedSongs: UnfinishedSongs
    Today: Date }
