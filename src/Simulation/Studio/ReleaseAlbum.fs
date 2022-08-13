module Simulation.Studio.ReleaseAlbum

open Common
open Entities
open Simulation

/// Releases an album to the public, which marks the album as released and starts
/// the release chain.
let releaseAlbum state band album =
    Album.Released.fromUnreleased album (Queries.Calendar.today state) 1.0
    |> Tuple.two band
    |> AlbumReleased
