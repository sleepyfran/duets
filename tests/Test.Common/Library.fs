module Test.Common

open Entities.Band
open Mediator.Mutation
open Mediator.Mutations.Setup
open Storage

let dummyCharacter =
  { Name = "Test"
    Age = 18
    Gender = "Male" }

let dummyBand =
  { Name = "Test"
    Genre = "Metal"
    Role = "Drummer" }

/// Initializes the state with a given character and band.
let initStateWith character band =
  Resolvers.All.init ()

  mutate (StartGameMutation character band)
  |> ignore

/// Inits the state with a dummy character and band. Useful when we just want
/// to test something that requires queries and mutations that are not directly
/// related to these characters or bands.
let initStateWithDummies () = initStateWith dummyCharacter dummyBand

/// Inits the state with the given character and a dummy band.
let initStateWithCharacter character = initStateWith character dummyBand

/// Inits the state with a dummy band and the given character.
let initStateWithBand band = initStateWith dummyCharacter band

/// Returns the currently selected band.
let currentBand () =
  State.getState () |> fun s -> List.head s.Bands

/// Returns the unfinished songs by the given band.
let unfinishedSongs band =
  State.getState ()
  |> fun s -> s.UnfinishedSongs
  |> Map.find band.Id
