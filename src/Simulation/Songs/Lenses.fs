module Simulation.Songs.Lenses

open Lenses
open Entities

/// Lenses to the unfinished field of the band repertoire.
let unfinishedSongsLenses =
  StateLenses.BandRepertoire
  << BandRepertoireLenses.Unfinished

/// Lenses to the finished field of the band repertoire.
let finishedSongsLenses =
  StateLenses.BandRepertoire
  << BandRepertoireLenses.Finished
