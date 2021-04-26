module Entities.Character

open Entities.Identity

type CharacterId = CharacterId of Identity

type Gender =
  | Male
  | Female
  | Other

module Gender =
  /// Returns a gender from its string representation. Defaults to other if
  /// given an invalid gender.
  let from str =
    match str with
    | "Male" -> Male
    | "Female" -> Female
    | _ -> Other

/// Defines a character, be it the one that the player is controlling or any
/// other NPC of the world.
type Character =
  { Id: CharacterId
    Name: string
    Age: int
    Gender: Gender }

type CharacterValidationError =
  | NameTooShort
  | NameTooLong
  | AgeTooYoung
  | AgeTooOld

/// Creates a character given a name, age and gender, if possible.
let from (name: string) age gender =
  if name.Length < 1 then
    Error NameTooShort
  else if name.Length > 50 then
    Error NameTooLong
  else if age < 18 then
    Error AgeTooYoung
  else if age > 80 then
    Error AgeTooOld
  else
    Ok
      { Id = CharacterId <| create ()
        Name = name
        Age = age
        Gender = gender }

/// Base character that has no real properties. Only to be used while
/// populating a character during a transformation.
let empty =
  { Id = CharacterId(create ())
    Name = ""
    Age = 0
    Gender = Gender.Other }
