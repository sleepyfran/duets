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

module Role =
  /// Returns a role given its string representation. Defaults to singer if
  /// given an invalid value.
  let from str =
    match str with
    | "Guitarist" -> Guitarist
    | "Bassist" -> Bassist
    | "Drummer" -> Drummer
    | _ -> Singer

/// Relates a specific character with its role in the band for a specific
/// region of time.
type Member = Character * Role * Calendar.Period

module Member =
  /// Creates a member from a character and a role from today onwards.
  let from character role today : Member =
    (character, role, (today, Calendar.Ongoing))

/// Represents any band inside the game, be it one that is controlled by the
/// player or the ones that are created automatically to fill the game world.
type Band =
  { Id: BandId
    Name: string
    Genre: Genre
    Members: Member list }

type BandValidationError =
  | NameTooShort
  | NameTooLong
  | NoMembersGiven

/// Creates a band given its name, genre and members, if possible.
let from name genre members =
  if String.length name < 1 then
    Error NameTooShort
  else if String.length name > 35 then
    Error NameTooLong
  else if List.length members < 1 then
    Error NoMembersGiven
  else
    Ok
      { Id = BandId <| create ()
        Name = name
        Genre = genre
        Members = members }

/// Returns default values for a band to serve as a placeholder to build a band
/// upon. Generates a valid ID.
let empty =
  { Id = BandId <| create ()
    Name = ""
    Genre = ""
    Members = [] }
