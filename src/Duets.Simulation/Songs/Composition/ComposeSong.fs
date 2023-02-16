module Duets.Simulation.Songs.Composition.ComposeSong

open Common
open Duets.Common
open Duets.Simulation.Queries
open Duets.Entities
open Duets.Simulation.Time

/// Orchestrates the song composition, which calculates the qualities of a song
/// and adds them with the song to the band's unfinished songs.
let composeSong state song =
    let band = Bands.currentBand state
    let maximumQuality = qualityForBand state band

    let initialQuality = calculateQualityIncreaseOf maximumQuality

    let songStartedEffect =
        (UnfinishedSong song, maximumQuality, initialQuality)
        |> Tuple.two band
        |> SongStarted

    [ songStartedEffect
      yield! AdvanceTime.advanceDayMoment' state 1<dayMoments> ]
