module Simulation.Bands.Queries

open Aether
open Common
open Entities
open Simulation.Character.Queries
open Simulation.Skills.Queries
open Storage

/// Returns the currently selected band in the game.
let currentBand () =
  let state = State.get ()
  state.Bands |> Map.find state.CurrentBandId

/// Returns all current members of the current band.
let currentBandMembers () =
  currentBand () |> Optic.get Lenses.Band.members_

/// Returns all past members of the current band.
let pastBandMembers () =
  currentBand ()
  |> Optic.get Lenses.Band.pastMembers_

/// Returns all the current members of the current band removing the main
/// character out of it. Useful for situations like selections in firing or
/// actions in which the playable character should not be taken into
/// consideration.
let currentBandMembersWithoutPlayableCharacter () =
  let mainCharacter = playableCharacter ()

  currentBandMembers ()
  |> List.filter (fun c -> c.Character.Id <> mainCharacter.Id)

/// Returns the average skill level of the members of the band.
let averageSkillLevel (band: Band) =
  band.Members
  |> List.map (fun mem -> mem.Character.Id)
  |> List.map averageSkillLevel
  |> List.average

/// Returns the average age of the members of the band.
let averageAge (band: Band) =
  band.Members
  |> List.averageBy (fun mem -> float mem.Character.Age)
