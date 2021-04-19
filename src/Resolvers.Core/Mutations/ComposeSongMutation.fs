module Resolvers.Core.Mutations.ComposeSong

open Core.Songs.Composition.ComposeSong
open Entities.Song
open Mediator.Mutations.Songs
open Mediator.Mutations.Types
open Mediator.Registries
open Resolvers.Common

let private validateName input =
  match input.Name.Length with
  | length when length < 1 -> Error NameTooShort
  | length when length > 50 -> Error NameTooLong
  | _ -> Ok input

let private TwentySeconds = 20
let private ThirtyMinutes = 60 * 30

let private validateLength input =
  match input.Length with
  | length when length < TwentySeconds -> Error LengthTooShort
  | length when length > ThirtyMinutes -> Error LengthTooLong
  | _ -> Ok input

let private validateVocalStyle input =
  match input.VocalStyle with
  | "Instrumental" -> Ok VocalStyle.Instrumental
  | "Normal" -> Ok VocalStyle.Normal
  | "Growl" -> Ok VocalStyle.Growl
  | "Screamo" -> Ok VocalStyle.Screamo
  | _ -> Error VocalStyleInvalid

let private toValidatedSong input: Result<Song, ValidationError> =
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

let register () =
  Registries.MutationRegistry.AddHandler
    MutationId.ComposeSong
    (boxed (fun input -> input |> toValidatedSong |> Result.map composeSong))
