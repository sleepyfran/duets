namespace Duets.Simulation.Queries

module Songs =
    open Aether
    open Duets.Entities

    /// Returns all unfinished songs by the given band. If no unfinished songs
    /// could be found, returns an empty map.
    let unfinishedByBand state bandId =
        let unfinishedSongLens = Lenses.FromState.Songs.unfinishedByBand_ bandId

        state |> Optic.get unfinishedSongLens |> Option.defaultValue Map.empty

    /// Returns a specific song given the ID of the band that holds it and the ID
    /// of the song to retrieve.
    let unfinishedByBandAndSongId state bandId songId =
        unfinishedByBand state bandId |> Map.tryFind songId

    let private toFinishedSongs songs =
        songs
        |> Map.map (fun _ song -> song |> Song.Finished.fromFinishedWithStatus)

    /// Returns all finished songs by the given band. If no finished songs could
    /// be found, returns an empty map.
    let finishedByBand state bandId =
        let finishedSongLens = Lenses.FromState.Songs.finishedByBand_ bandId

        state
        |> Optic.get finishedSongLens
        |> Option.map toFinishedSongs
        |> Option.defaultValue Map.empty

    /// Returns all finished songs by the given band that have not been recorded.
    let finishedNonRecordedByBand state bandId =
        let finishedSongLens = Lenses.FromState.Songs.finishedByBand_ bandId

        state
        |> Optic.get finishedSongLens
        |> Option.map (fun songs ->
            songs
            |> Map.filter (fun _ finishedWithMetadata ->
                let recorded =
                    Song.Finished.Metadata.recorded finishedWithMetadata

                recorded = false))
        |> Option.defaultValue Map.empty

    /// Returns a specific finished song given its ID and the band's ID.
    let finishedByBandAndSongId state bandId songId =
        finishedByBand state bandId |> Map.tryFind songId
