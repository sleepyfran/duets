module Duets.Entities.Character

open Duets.Common
open System

type CharacterNameValidationError =
    | NameTooShort
    | NameTooLong

type CharacterAgeValidationError =
    | AgeTooYoung
    | AgeTooOld

let allAttributes = Union.allCasesOf<CharacterAttribute> ()

let defaultAttributes =
    [ (CharacterAttribute.Energy, 100)
      (CharacterAttribute.Health, 100)
      (CharacterAttribute.Hunger, 100)
      (CharacterAttribute.Mood, 100) ]
    |> Map.ofList

/// Base character that has no real properties. Only to be used while
/// populating a character during a transformation.
let empty =
    { Id = CharacterId <| Identity.create ()
      Name = ""
      Birthday = Calendar.gameBeginning |> Calendar.Ops.addYears -25
      Gender = Gender.Other
      Attributes = defaultAttributes
      Moodlets = Set.empty }

/// Creates a character from the given parameters, generating a random
/// ID for it.
let from name gender birthday =
    { Id = CharacterId <| Identity.create ()
      Name = name
      Birthday = birthday
      Gender = gender
      Attributes = defaultAttributes
      Moodlets = Set.empty }

/// Validates whether the name of the character is valid or not.
let validateName (name: string) =
    if String.IsNullOrEmpty name then Error NameTooShort
    else if name.Length > 50 then Error NameTooLong
    else Ok name

/// Validates whether the age of the character is valid or not.
let validateBirthday birthday =
    let age = Calendar.Query.yearsBetween birthday Calendar.gameBeginning

    if age < 18 then Error AgeTooYoung
    else if age > 80 then Error AgeTooOld
    else Ok birthday
