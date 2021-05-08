module Simulation.Songs.Composition.DiscardSong

open Entities

/// Removes a song from the band's unfinished repertoire.
let discardSong band unfinishedSong = (band, unfinishedSong) |> SongDiscarded
