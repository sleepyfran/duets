module Duets.Simulation.Songs.Composition.ComposeSong

open Common
open Duets.Common
open Duets.Simulation.Queries
open Duets.Entities
open Duets.Simulation
open Duets.Simulation.Time

/// Orchestrates the song composition, which calculates the qualities of a song
/// and adds them with the song to the band's unfinished songs.
let composeSong state song =
    let band = Bands.currentBand state
    let maximumQuality = qualityForBand state band

    let initialUnfinishedSong = Unfinished(song, maximumQuality, 0<quality>)

    let initialQuality = calculateQualityIncreaseOf initialUnfinishedSong

    [ Unfinished(song, maximumQuality, initialQuality)
      |> Tuple.two band
      |> SongStarted ]
