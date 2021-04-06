module Core.Resolvers.Songs.Composition.Transformations

open Entities.Song
open Mediator.Mutations.Songs

let validateName input =
  match input.Name.Length with
  | length when length < 1 -> Error NameTooShort
  | length when length > 50 -> Error NameTooLong
  | _ -> Ok input

let TwentySeconds = 20
let ThirtyMinutes = 60 * 30

let validateLength input =
  match input.Length with
  | length when length < TwentySeconds -> Error LengthTooShort
  | length when length > ThirtyMinutes -> Error LengthTooLong
  | _ -> Ok input

let validateVocalStyle input =
  match input.VocalStyle with
  | "Instrumental" -> Ok VocalStyle.Instrumental
  | "Normal" -> Ok VocalStyle.Normal
  | "Growl" -> Ok VocalStyle.Growl
  | "Screamo" -> Ok VocalStyle.Screamo
  | _ -> Error VocalStyleInvalid

let toValidatedSong input: Result<Song, ValidationError> =
  input
  |> validateName
  |> Result.bind validateLength
  |> Result.bind validateVocalStyle
  |> Result.bind (fun vocalStyle ->
       Ok
         { empty with
             Name = input.Name
             Length = input.Length
             VocalStyle = vocalStyle })
