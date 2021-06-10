module Simulation.Studio.ReleaseAlbum

open Common
open Entities
open Simulation.Queries

/// Releases an album to the public, which marks the album as released and starts
/// the release chain.
let releaseAlbum state band album =
    Album.Released.fromUnreleased album (Calendar.today state)
    |> Tuple.two band
    |> AlbumReleased
