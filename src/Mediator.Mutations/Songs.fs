module Mediator.Mutations.Songs

open Entities.Song
open Mediator.Mutations.Types

type SongMutationInput =
  { Name: string
    Length: int
    VocalStyle: string }

type ValidationError =
  | NameTooShort
  | NameTooLong
  | LengthTooShort
  | LengthTooLong
  | VocalStyleInvalid

let ComposeSongMutation
  input
  : Mutation<SongMutationInput, Result<unit, ValidationError>> =
  { Id = MutationId.ComposeSong
    Parameter = Some input }

let ImproveSongMutation input : Mutation<UnfinishedSongWithQualities, SongStatus> =
  { Id = MutationId.ImproveSong
    Parameter = Some input }
