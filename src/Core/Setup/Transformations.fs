module Core.Resolvers.Setup.Transformations

open Core.Resolvers.Setup.Mutations
open Entities
open Entities.Character
open Mediator.Mutations.Setup

let validateCharacterName input =
  match input.Character.Name.Length with
  | length when length < 1 -> Error CharacterNameTooShort
  | length when length > 50 -> Error CharacterNameTooLong
  | _ -> Ok input

let validateCharacterAge input =
  match input.Character.Age with
  | age when age < 18 -> Error CharacterAgeTooYoung
  | age when age > 80 -> Error CharacterAgeTooOld
  | _ -> Ok input

let validateCharacterGender input =
  match input.Character.Gender with
  | "Male" -> Ok Gender.Male
  | "Female" -> Ok Gender.Female
  | _ -> Ok Gender.Other

let toCoreCharacter input =
  input
  |> validateCharacterName
  |> Result.bind validateCharacterAge
  |> Result.bind validateCharacterGender
  |> Result.bind (fun gender -> Ok { empty with Gender = gender })
  |> Result.bind (fun character ->
       Ok
         { character with
             Name = input.Character.Name
             Age = input.Character.Age })

let validateBandName input =
  match input.Band.Name.Length with
  | length when length < 1 -> Error BandNameTooShort
  | length when length > 35 -> Error BandNameTooLong
  | _ -> Ok input

let toMember character input =
  Ok
    (character,
     Band.toRole input.Band.Role,
     (Calendar.fromDayMonth 1 1, Calendar.Ongoing))

let toCoreBand input character =
  input
  |> validateBandName
  |> Result.bind (toMember character)
  |> Result.map (fun bandMember ->
       { Band.empty with
           Name = input.Band.Name
           Genre = input.Band.Genre
           Members = [ bandMember ] })

let toValidatedStartGameMutation input
                                 : Result<ValidatedStartGameMutation, ValidationError> =
  input
  |> toCoreCharacter
  |> Result.bind (fun character ->
       toCoreBand input character
       |> Result.map (fun band -> { Character = character; Band = band }))
