module Duets.Simulation.Studio.ReleaseAlbum

open Duets.Common
open Duets.Entities
open Duets.Simulation

/// Releases an album to the public, which marks the album as released and starts
/// the release chain.
let releaseAlbum state band album =
    Album.Released.fromUnreleased album (Queries.Calendar.today state) 1.0
    |> Tuple.two band
    |> AlbumReleased
    |> List.singleton
