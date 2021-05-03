module Simulation.Songs.Composition.DiscardSong

open Simulation.Bands.Queries
open Simulation.Songs.Composition.Common

/// Removes a song from the band's unfinished repertoire.
let discardSong unfinishedSong =
  removeUnfinishedSong (currentBand ()) unfinishedSong
  unfinishedSong
