module Simulation.Studio.ReleaseAlbum

open Common
open Entities

/// Releases an album to the public, which marks the album as released and starts
/// the release chain.
let releaseAlbum band album =
    Album.Released.fromUnreleased album
    |> Tuple.two band
    |> AlbumReleased
