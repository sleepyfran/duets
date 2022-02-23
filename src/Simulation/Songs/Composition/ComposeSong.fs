module Simulation.Songs.Composition.ComposeSong

open Common
open Simulation.Queries
open Entities

/// Orchestrates the song composition, which calculates the qualities of a song
/// and adds them with the song to the band's unfinished songs.
let composeSong state song =
    let band = Bands.currentBand state
    let maximumQuality = qualityForBand state band

    let initialQuality = calculateQualityIncreaseOf maximumQuality

    (UnfinishedSong song, maximumQuality, initialQuality)
    |> Tuple.two band
    |> SongStarted
