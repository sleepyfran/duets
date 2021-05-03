module Simulation.Songs.Lenses

open Aether
open Aether.Operators
open Entities

/// Lenses to the unfinished field of a specific band in its repertoire.
let unfinishedSongs_ bandId =
  Lenses.State.bandRepertoire_
  >-> Lenses.BandRepertoire.unfinished_
  >-> Map.key_ bandId

/// Lenses to the finished field of a specific band in its repertoire.
let finishedSongs_ bandId =
  Lenses.State.bandRepertoire_
  >-> Lenses.BandRepertoire.finished_
  >-> Map.key_ bandId
