namespace Simulation.Queries

module Repertoire =
    open Entities

    /// Returns all finished songs, whether they're in an album or not, from
    /// the given band.
    let allFinishedSongsByBand state bandId =
        let finishedWithNoAlbum =
            Songs.finishedByBand state bandId
            |> Map.toList
            |> List.map snd

        let finishedWithAlbum =
            Albums.releasedByBand state bandId
            |> List.map (fun album -> album.Album.TrackList)
            |> List.concat

        finishedWithNoAlbum @ finishedWithAlbum

    /// Returns a specific finished song from either the album collection
    /// or the finished song collection that are not included in any album yet.
    let finishedFromAllByBandAndSongId state bandId songId =
        allFinishedSongsByBand state bandId
        |> List.tryFind (fun (FinishedSong s, _) -> s.Id = songId)
