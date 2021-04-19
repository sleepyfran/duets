module Test.Common

open Entities.Band
open Entities.Character
open Entities.Skill
open Mediator.Mutation
open Mediator.Mutations.Setup
open Mediator.Mutations.Songs
open Mediator.Mutations.Storage
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

/// Returns the main character.
let currentCharacter () =
  State.getState () |> fun s -> s.Character

/// Returns the currently selected band.
let currentBand () =
  State.getState () |> fun s -> List.head s.Bands

/// Returns the unfinished songs by the given band.
let unfinishedSongs (band: Band) =
  State.getState ()
  |> fun s -> s.UnfinishedSongs
  |> Map.find band.Id

/// Adds a new song to the default band.
let addSong song =
  ComposeSongMutation song |> mutate |> ignore

/// Adds a given skill to the given character.
let addSkillTo (character: Character) (skillWithLevel: SkillWithLevel) =
  let (skill, _) = skillWithLevel

  ModifyStateMutation
    (fun state ->
      state.CharacterSkills
      |> Map.tryFind character.Id
      |> Option.defaultValue Map.empty
      |> Map.add skill.Id skillWithLevel
      |> fun updatedSkills ->
           Map.add character.Id updatedSkills state.CharacterSkills
      |> fun characterSkills ->
           { state with
               CharacterSkills = characterSkills })
  |> mutate
