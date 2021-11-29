module Simulation.Studio.ReleaseAlbum

open Common
open Entities
open Simulation.Queries

let private defaultMarketSize = 4000000

let private calculateGenreMarket genreMarket =
    genreMarket.MarketPoint
    * (float defaultMarketSize)

let private calculateUsefulMarket band genreMarket =
    let fameModifier =
        if band.Fame < 10 then 100000.0
        else if band.Fame < 30 then 10000.0
        else if band.Fame < 60 then 1000.0
        else 100.0

    (calculateGenreMarket genreMarket)
    * (float band.Fame / fameModifier)

let private calculateReleaseTypeModifier (album: Album) =
    match album.Type with
    | Single -> 1.0
    | EP -> 0.7
    | LP -> 1.0

let private calculateQualityModifier album =
    album
    |> Albums.quality
    |> fun quality -> (float quality / 100.0)

let private calculateMaxDailyStreams
    state
    (band: Band)
    (UnreleasedAlbum album)
    =
    GenreMarkets.from state band.Genre
    |> calculateUsefulMarket band
    |> (*) (calculateReleaseTypeModifier album)
    |> (*) (calculateQualityModifier album)
    |> Math.roundToNearest

/// Releases an album to the public, which marks the album as released and starts
/// the release chain.
let releaseAlbum state band album =
    Album.Released.fromUnreleased
        album
        (Calendar.today state)
        (calculateMaxDailyStreams state band album)
        1.0
    |> Tuple.two band
    |> AlbumReleased
