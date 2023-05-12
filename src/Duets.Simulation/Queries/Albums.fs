namespace Duets.Simulation.Queries

module Albums =
    open Aether
    open Duets.Common
    open Duets.Entities
    open Duets.Simulation

    /// Returns all unreleased albums by the given band. If no unreleased albums
    /// could be found, returns an empty map.
    let unreleasedByBand state bandId =
        let unreleasedAlbumLens =
            Lenses.FromState.Albums.unreleasedByBand_ bandId

        state |> Optic.get unreleasedAlbumLens |> Option.defaultValue Map.empty

    /// Returns a specific album given the ID of the band that holds it and the
    /// ID of the album to retrieve.
    let unreleasedByBandAndAlbumId state bandId albumId =
        unreleasedByBand state bandId |> Map.tryFind albumId

    /// Returns all released albums by the given band ordered by release date.
    /// If no released albums could be found, returns an empty list.
    let releasedByBand state bandId =
        let releasedAlbumLens = Lenses.FromState.Albums.releasedByBand_ bandId

        state
        |> Optic.get releasedAlbumLens
        |> Option.defaultValue Map.empty
        |> List.ofSeq
        |> List.map (fun kvp -> kvp.Value)
        |> List.sortBy (fun album -> album.ReleaseDate)

    /// Returns all released albums by all bands, organized by each band.
    let allReleased state =
        state.BandAlbumRepertoire.ReleasedAlbums
        |> Map.map (fun _ -> List.ofMapValues)

    /// Returns all released albums by all bands that were released in the last
    /// given days.
    let releaseInLast state (days: int<days>) =
        let currentDate =
            Queries.Calendar.today state |> Calendar.Transform.resetDayMoment

        state.BandAlbumRepertoire.ReleasedAlbums
        |> Map.map (fun _ albums ->
            List.ofMapValues albums
            |> List.takeWhile (fun album ->
                let normalizedReleaseDate =
                    album.ReleaseDate |> Calendar.Transform.resetDayMoment

                let daysSinceRelease =
                    Calendar.Query.daysBetween
                        currentDate
                        normalizedReleaseDate
                    * 1<days>

                daysSinceRelease = days))

    /// Returns the average quality of the songs in the album.
    let quality album =
        album.TrackList
        |> List.map (fun (_, quality) -> float quality)
        |> List.average
        |> Math.roundToNearest

    /// Calculates the generated revenue of the album.
    let revenue album =
        float album.Streams * Config.Revenue.revenuePerStream
        |> decimal
        |> (*) 1m<dd>
