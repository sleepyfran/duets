module Simulation.Setup

open Entities
open Storage

/// Sets up the initial game state based on the data provided by the user in
/// the setup wizard.
let startGame character (band: Band) =
  { Character = character
    CharacterSkills = [ (character.Id, Map.empty) ] |> Map.ofSeq
    CurrentBandId = band.Id
    Bands = [ (band.Id, band) ] |> Map.ofList
    BandRepertoire = Band.Repertoire.emptyFor band.Id
    Today = Calendar.fromDayMonth 1 1 }
  |> GameCreated
