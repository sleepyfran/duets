namespace Simulation.Queries

module Albums =
    open Aether
    open Entities

    /// Returns all unreleased albums by the given band. If no unreleased albums
    /// could be found, returns an empty map.
    let unreleasedByBand state bandId =
        let unreleasedAlbumLens =
            Lenses.FromState.Albums.unreleasedByBand_ bandId

        state
        |> Optic.get unreleasedAlbumLens
        |> Option.defaultValue Map.empty

    /// Returns a specific album given the ID of the band that holds it and the
    /// ID of the album to retrieve.
    let unreleasedByBandAndAlbumId state bandId albumId =
        unreleasedByBand state bandId
        |> Map.tryFind albumId

    /// Returns all released albums by the given band. If no released albums
    /// could be found, returns an empty map.
    let releasedByBand state bandId =
        let releasedAlbumLens =
            Lenses.FromState.Albums.releasedByBand_ bandId

        state
        |> Optic.get releasedAlbumLens
        |> Option.defaultValue Map.empty
