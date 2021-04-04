module Core.Resolvers.Setup.Mutations

open Entities
open Entities.Band
open Entities.Character
open Entities.State
open Mediator.Mutations

type ValidatedStartGameMutation = { Character: Character; Band: Band }

let startGame _ mutate input =
  let state: State =
    { Character = input.Character
      Bands = [ input.Band ]
      Today = Calendar.fromDayMonth 1 1 }

  mutate <| Storage.SetStateMutation state
  ()
