module Duets.Simulation.Songs.Composition.FinishSong

open Duets.Entities
open Duets.Simulation

/// Orchestrates the finishing of a song, which moves it from the map of
/// unfinished songs into the map of finished songs.
let finishSong state band (Unfinished(song, _, currentQuality)) =
    let currentDate = Queries.Calendar.today state

    (band, Finished(song, currentQuality), currentDate)
    |> SongFinished
    |> List.singleton
