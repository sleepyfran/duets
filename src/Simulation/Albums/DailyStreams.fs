module Simulation.Albums.DailyStreams

open Common
open Entities
open Simulation

let private calculateFanStreams band =
    (float band.Fans
     * Config.MusicSimulation.fanStreamingPercentage)
    |> Math.ceil

let private calculateNonFanStreams state band album genreMarket =
    let albumQuality = Queries.Albums.quality album |> float

    let fameLevel = Queries.Bands.estimatedFameLevel state band

    let capModifier =
        match fameLevel with
        | level when level < 10 -> 1.0
        | level when level < 30 -> 10.0
        | level when level < 50 -> 100.0
        | level when level < 70 -> 1000.0
        | _ -> 10000.0

    let amountCap =
        Config.MusicSimulation.baseNonFanStreamCap
        / capModifier

    (*
    This should give a tiny bit of the whole market, just so that little bands
    have a chance of catching some people from the beginning. For example, for
    a genre market of 12.5 million people this should give 12 streams for an
    album with 95% quality. This amount increases the more famous a band is.
    *)
    genreMarket * (albumQuality / amountCap)
    |> Math.ceil

let private calculateReleaseTypeModifier (album: Album) =
    match album.Type with
    | Single -> 1.0
    | EP -> 0.7
    | LP -> 1.0

let private calculateQualityModifier album =
    album
    |> Queries.Albums.quality
    |> fun quality -> (float quality / 100.0)

let private applyStreamModifiers album streams =
    streams
    |> (*) (calculateReleaseTypeModifier album)
    |> (*) (calculateQualityModifier album)

let private calculateMaxDailyStreams state releasedAlbum =
    let band = Queries.Bands.currentBand state
    let album = Album.fromReleased releasedAlbum

    let usefulMarket = Queries.GenreMarkets.usefulMarketOf state band.Genre

    let fanStreams =
        calculateFanStreams band
        |> applyStreamModifiers album

    let nonFanStreams =
        usefulMarket
        |> calculateNonFanStreams state band album
        |> applyStreamModifiers album

    (fanStreams, nonFanStreams)

/// Calculates the total amount of streams that the album generated in the
/// previous day given the album based on its max daily streams and the current
/// hype of the album.
let dailyStreams state releasedAlbum =
    let fanStreams, nonFanStreams = calculateMaxDailyStreams state releasedAlbum

    (fanStreams * releasedAlbum.Hype
     |> Math.roundToNearest,
     nonFanStreams * releasedAlbum.Hype
     |> Math.ceilToNearest)
