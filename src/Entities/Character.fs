module Entities.Character

type CharacterNameValidationError =
    | NameTooShort
    | NameTooLong

type CharacterAgeValidationError =
    | AgeTooYoung
    | AgeTooOld

/// Base character that has no real properties. Only to be used while
/// populating a character during a transformation.
let empty =
    { Id = CharacterId <| Identity.create ()
      Name = ""
      Age = 0
      Gender = Gender.Other
      Status =
          { Mood = 100
            Health = 100
            Energy = 100
            Fame = 0 } }

/// Creates a character from the given parameters, generating a random
/// ID for it.
let from name gender age =
    { Id = CharacterId <| Identity.create ()
      Name = name
      Age = age
      Gender = gender
      Status =
          { Mood = 100
            Health = 100
            Energy = 100
            Fame = 0 } }

/// Validates whether the name of the character is valid or not.
let validateName (name: string) =
    if name.Length < 1 then
        Error NameTooShort
    else if name.Length > 50 then
        Error NameTooLong
    else
        Ok name

/// Validates whether the age of the character is valid or not.
let validateAge age =
    if age < 18 then Error AgeTooYoung
    else if age > 80 then Error AgeTooOld
    else Ok age
