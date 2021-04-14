module Mediator.Queries.Storage

open Entities.Character
open Entities.Band
open Entities.State
open Mediator.Queries.Types

type SavegameState =
  | NotAvailable
  | Available

/// Retrieves whether there's a savegame available for loading or not.
let SavegameStateQuery : Query<unit, SavegameState> =
  { Id = QueryId.SavegameState
    Parameter = None }

// --- STATE QUERIES ---

/// Returns the current band.
let CurrentBandQuery : Query<unit, Band> = { Id = QueryId.Band; Parameter = None }

/// Returns the current character.
let CharacterQuery : Query<unit, Character> =
  { Id = QueryId.Character
    Parameter = None }

/// Returns all the skills of all the characters in the game.
let CharacterSkillsQuery : Query<unit, CharacterSkills> =
  { Id = QueryId.CharacterSkills
    Parameter = None }

/// Returns all unfinished songs by all bands of the player.
let UnfinishedSongsQuery : Query<unit, UnfinishedSongs> =
  { Id = QueryId.UnfinishedSongs
    Parameter = None }

// --- STATIC DATA ---
type LiteralRole = string
type LiteralGenre = string
type LiteralVocalStyle = string

/// Retrieves the static list of roles available in the game.
let RolesQuery : Query<unit, LiteralRole list> =
  { Id = QueryId.Roles; Parameter = None }

/// Retrieves the static list of genres available in the game.
let GenresQuery : Query<unit, LiteralGenre list> =
  { Id = QueryId.Genres
    Parameter = None }

/// Retrieves the static list of vocal styles available in the game.
let VocalStylesQuery : Query<unit, LiteralVocalStyle list> =
  { Id = QueryId.VocalStyle
    Parameter = None }
