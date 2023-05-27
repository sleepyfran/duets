module Duets.Simulation.Songs.Composition.FinishSong

open Duets.Common
open Duets.Entities

/// Orchestrates the finishing of a song, which moves it from the map of
/// unfinished songs into the map of finished songs.
let finishSong band (Unfinished(song, _, currentQuality)) =
    Finished(song, currentQuality) |> Tuple.two band |> SongFinished
