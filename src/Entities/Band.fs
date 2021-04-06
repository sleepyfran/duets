module Entities.Band

open System
open Entities.Identity
open Entities.Character
open Entities.Genre

type BandId = BandId of Identity

/// Defines all possible roles that a character can take inside a band.
type Role =
  | Singer
  | Guitarist
  | Bassist
  | Drummer

/// Relates a specific character with its role in the band for a specific
/// region of time.
type Member = Character * Role * Calendar.Period

/// Represents any band inside the game, be it one that is controlled by the
/// player or the ones that are created automatically to fill the game world.
type Band =
  { Id: BandId
    Name: string
    Genre: Genre
    Members: Member list }

/// Returns default values for a band to serve as a placeholder to build a band
/// upon. Generates a valid ID.
let empty =
  { Id = BandId(Guid.NewGuid())
    Name = ""
    Genre = ""
    Members = [] }

/// Attempts to transform a string into a Role. Defaults to Singer if an
/// invalid value is given.
let toRole str =
  match str with
  | "Singer" -> Singer
  | "Guitarist" -> Guitarist
  | "Bassist" -> Bassist
  | "Drummer" -> Drummer
  | _ -> Singer
