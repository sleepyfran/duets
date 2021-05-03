module Test.Common

open Common
open Entities
open Simulation.Bands.Queries
open Simulation.Setup
open Simulation.Songs.Composition.ImproveSong
open Simulation.Songs.Queries
open Storage

let dummyCharacter =
  Character.from "Test" 24 Other |> Result.unwrap

let dummyBand =
  { Band.empty with
      Members =
        [ Band.Member.from dummyCharacter Guitar (Calendar.fromDayMonth 1 1) ] }

let dummySong = Song.empty

/// Initializes the state with a given character and band.
let initStateWith character band = startGame character band

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
  State.get () |> fun s -> s.Character


/// Returns the currently selected band.
let currentBand = currentBand

/// Adds a given member to the given band.
let addMember (band: Band) bandMember =
  State.map
    (fun state ->
      { state with
          Bands =
            Map.add
              band.Id
              { band with
                  Members = List.append [ bandMember ] band.Members }
              state.Bands })

/// Retrieves the last unfinished song from the state.
let lastUnfinishedSong () =
  currentBand ()
  |> fun band -> band.Id
  |> unfinishedSongsByBand
  |> Seq.head
  |> fun kvp -> kvp.Value

/// Improves the last unfinished song a given number of times.
let improveLastUnfinishedSongTimes times =
  for _ in 1 .. times do
    lastUnfinishedSong () |> improveSong |> ignore

/// Adds a given skill to the given character.
let addSkillTo (character: Character) (skillWithLevel: SkillWithLevel) =
  let (skill, _) = skillWithLevel

  State.map
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
