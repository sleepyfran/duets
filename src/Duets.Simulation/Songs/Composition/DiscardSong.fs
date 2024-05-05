module Duets.Simulation.Songs.Composition.DiscardSong

open Duets.Entities

/// Removes a song from the band's unfinished repertoire.
let discardSong band unfinishedSong =
    (band, unfinishedSong) |> SongDiscarded |> List.singleton
