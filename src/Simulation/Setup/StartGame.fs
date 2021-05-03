module Simulation.Setup

open Entities
open Storage.State

/// Sets up the initial game state based on the data provided by the user in
/// the setup wizard.
let startGame character (band: Band) =
  { Character = character
    CharacterSkills = Map.empty
    CurrentBandId = band.Id
    Bands = [ (band.Id, band) ] |> Map.ofList
    BandRepertoire = Band.Repertoire.empty
    Today = Calendar.fromDayMonth 1 1 }
  |> setState
