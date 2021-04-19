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
  let maximumQuality = qualityForBand band

  let initialQuality = calculateQualityIncreaseOf maximumQuality

  let unfinishedSongs = query UnfinishedSongsQuery

  let songWithQualities =
    (UnfinishedSong input, MaxQuality maximumQuality, Quality initialQuality)

  let unfinishedSongsByBand =
    Map.tryFind band.Id unfinishedSongs
    |> Option.defaultValue []

  let unfinishedWithSong =
    unfinishedSongsByBand
    @ [ UnfinishedWithQualities songWithQualities ]

  mutate (
    ModifyStateMutation
      (fun state ->
        { state with
            UnfinishedSongs =
              Map.add band.Id unfinishedWithSong unfinishedSongs })
  )
