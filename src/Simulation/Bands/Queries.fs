module Simulation.Bands.Queries

open Common
open Entities
open Simulation.Skills.Queries
open Storage.State

/// Returns the currently selected band in the game.
let currentBand () =
  let state = getState ()
  state.Bands |> Map.find state.CurrentBandId

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
