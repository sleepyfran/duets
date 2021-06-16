module Simulation.Albums.DailyStreams

open Common
open Entities

/// Calculates the total amount of streams that the album generated in the
/// previous day given the album based on its max daily streams and the current
/// hype of the album.
let dailyStreams releasedAlbum =
    float releasedAlbum.MaxDailyStreams
    * releasedAlbum.Hype
    |> Math.roundToNearest
