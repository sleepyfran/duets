module Mediator.Mutations.Setup

open Mediator.Mutations.Types

type CharacterInput = {
  Name: string
  Age: int
  Gender: string
}

type BandInput = {
  FirstMember: CharacterInput
  Name: string
  Genre: string
  Role: string
}

type StartGameMutationInput = {
  Character: CharacterInput
  Band: BandInput
}

/// Creates a new game with the specified character as the first member and band.
let StartGameMutation character band = Mutation.WithParameter {
  Id = MutationId.StartGame
  Parameter = {
    Character = character
    Band = band
  }
}