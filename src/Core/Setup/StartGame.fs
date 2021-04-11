module Core.Setup

open Entities
open Entities.Band
open Entities.Character
open Entities.State
open Mediator.Mutation
open Mediator.Mutations

type SetupInput = { Character: Character; Band: Band }

let startGame input =
  let state : State =
    { Character = input.Character
      CharacterSkills = Map.empty
      Band = input.Band
      UnfinishedSongs = Map.empty
      Today = Calendar.fromDayMonth 1 1 }

  mutate <| Storage.SetStateMutation state
  ()
