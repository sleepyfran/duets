module Core.Songs.Composition.ComposeSong

open Core.Songs.Composition.Common
open Entities.Band
open Entities.State
open Entities.Song
open Mediator.Mutations.Storage
open Mediator.Mutation
open Mediator.Queries.Storage
open Mediator.Query

/// Orchestrates the song composition, which calculates the qualities of a song
/// and adds them with the song to the band's unfinished songs.
let composeSong input =
  let band = query CurrentBandQuery
  let maximumScore = scoreForBand band

  let initialScore = calculateScoreIncreaseOf maximumScore

  let unfinishedSongs = query UnfinishedSongsQuery

  let songWithQuality =
    (UnfinishedSong input, MaxQuality maximumScore, Quality initialScore)

  let unfinishedSongsByBand =
    Map.tryFind band.Id unfinishedSongs
    |> Option.defaultValue []

  let unfinishedWithSong =
    unfinishedSongsByBand
    @ [ UnfinishedWithQualities songWithQuality ]

  mutate (
    ModifyStateMutation
      (fun state ->
        { state with
            UnfinishedSongs =
              Map.add band.Id unfinishedWithSong state.UnfinishedSongs })
  )
