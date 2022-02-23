namespace Simulation.Queries

module Songs =
    open Aether
    open Entities

    /// Returns all unfinished songs by the given band. If no unfinished songs
    /// could be found, returns an empty map.
    let unfinishedByBand state bandId =
        let unfinishedSongLens = Lenses.FromState.Songs.unfinishedByBand_ bandId

        state
        |> Optic.get unfinishedSongLens
        |> Option.defaultValue Map.empty

    /// Returns a specific song given the ID of the band that holds it and the ID
    /// of the song to retrieve.
    let unfinishedByBandAndSongId state bandId songId =
        unfinishedByBand state bandId
        |> Map.tryFind songId

    /// Returns all finished songs by the given band. If no finished songs could
    /// be found, returns an empty map.
    let finishedByBand state bandId =
        let finishedSongLens = Lenses.FromState.Songs.finishedByBand_ bandId

        state
        |> Optic.get finishedSongLens
        |> Option.defaultValue Map.empty

    /// Returns a specific finished song given its ID and the band's ID.
    let finishedByBandAndSongId state bandId songId =
        finishedByBand state bandId |> Map.tryFind songId
