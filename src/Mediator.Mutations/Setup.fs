module Mediator.Mutations.Setup

open Mediator.Mutations.Types

type CharacterInput = {
  Name: string
  Age: int
  Gender: string
}

type BandInput = {
  Name: string
  Genre: string
  Role: string
}

type StartGameMutationInput = {
  Character: CharacterInput
  Band: BandInput
}

/// Creates a new game with the specified character as the first member and band.
let StartGameMutation character band : Mutation<StartGameMutationInput, unit> = {
  Id = MutationId.StartGame
  Parameter = Some {
    Character = character
    Band = band
  }
}